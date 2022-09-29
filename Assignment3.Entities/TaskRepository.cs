using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using Assignment3.Core;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Entities;

public class TaskRepository : ITaskRepository
{
    private KanbanContext _context;
    public TaskRepository(KanbanContext context)
    {
        _context = context;
    }

    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {   

        var tagRepo = new TagRepository(_context);
        var tags = new List<Tag>();
        foreach(var tag in task.Tags) {
            var tagInfo = tagRepo.Create(new TagCreateDTO(tag));
            var tagEntity = _context.Tags.SingleOrDefault(t => t.Id == tagInfo.TagId);
            if (tagEntity != null) tags.Add(tagEntity);
        }

        User assignedTo = null!;
        if (task.AssignedToId != null) {
            assignedTo = _context.Users.Find(task.AssignedToId)!;
            if (assignedTo == null) return (Response.BadRequest, 0);
        }

        var taskEntity = new Task {
            CreatedDate = DateTime.Now,
            StateUpdated = DateTime.Now,
            Description = task.Description!,
            Title = task.Title,
            State = State.New,
            Tags = tags,
            AssignedTo = assignedTo
        };
        
        _context.Tasks.Add(taskEntity);
        _context.SaveChanges();
        return (Response.Created, taskEntity.Id);
    
    }

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        var queryResult = _context.Tasks
            .Include(t => t.AssignedTo)
            .Include(t => t.Tags)
            .ToList();
        var taskDTOs = queryResult.Select(task => new TaskDTO(task.Id, task.Title, task.AssignedTo.Name,
            task.Tags.Select(t => t.Name).ToImmutableList(), task.State)).ToImmutableList();
        return taskDTOs;

    }

    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        var queryResult = _context.Tasks
            .Include(t => t.AssignedTo)
            .Include(t => t.Tags)
            .Where(t => t.State == State.Removed)
            .ToList();
        var taskDTOs = queryResult.Select(task => new TaskDTO(task.Id, task.Title, task.AssignedTo.Name,
            task.Tags.Select(t => t.Name).ToImmutableList(), task.State)).ToImmutableList();
        return taskDTOs;
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        
        var queryResult = _context.Tags.Include(t => t.Tasks).SingleOrDefault(t => t.Name == tag);

        if (queryResult == null) return null!;

        var taskDTOs = queryResult.Tasks.Select(task => new TaskDTO(task.Id, task.Title, task.AssignedTo.Name,
            task.Tags.Select(t => t.Name).ToImmutableList(), task.State)).ToImmutableList();
        
        return taskDTOs;
        
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        var queryResult = _context.Users.Include(u => u.Tasks).SingleOrDefault(u => u.Id == userId);
        
        if (queryResult == null) return null!;

        var taskDTOs = queryResult.Tasks.Select(task => new TaskDTO(task.Id, task.Title, task.AssignedTo.Name,
            task.Tags.Select(t => t.Name).ToImmutableList(), task.State)).ToImmutableList();
        
        return taskDTOs;
        
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
    {
        var queryResult = _context.Tasks
            .Include(t => t.AssignedTo)
            .Include(t => t.Tags)
            .Where(t => t.State == state)
            .ToList();
        var taskDTOs = queryResult.Select(task => new TaskDTO(task.Id, task.Title, task.AssignedTo.Name,
            task.Tags.Select(t => t.Name).ToImmutableList(), task.State)).ToImmutableList();
        return taskDTOs;
    }

    public TaskDetailsDTO Read(int taskId)
    {
        //var task = _context.Tasks.Include(t => t.AssignedTo).Include(t => t.Tags).SingleOrDefault(t => t.Id == taskId);
        var task = _context.Tasks.Find(taskId);
        if (task == null) return null!;
        
        //var userName = task.AssignedTo.Name;
        var userName = "";

        return new TaskDetailsDTO(task.Id, task.Title, task.Description, task.CreatedDate, userName, task.Tags.Select(tag => tag.Name).ToImmutableList(),
            task.State, task.StateUpdated);
        
       
    }

    public Response Update(TaskUpdateDTO task)
    {
        //var query = _context.Tasks.Include(t => t.Tags).SingleOrDefault(t => t.Id == task.Id);
        var query = _context.Tasks.Find(task.Id);


        if (query == null) 
            return Response.NotFound;
        
        query.Description = task.Description!;
        query.Title = task.Title;

        if (task.AssignedToId != null) {
            query.AssignedTo = _context.Users.Find(task.AssignedToId)!;
            if (query.AssignedTo == null) return Response.BadRequest;
        }

        if (query.State != task.State) {
            query.StateUpdated = DateTime.Now;
        }

        query.State = task.State;

        foreach (var tag in task.Tags.Except(query.Tags.Select(t => t.Name)))
            query.Tags.Add(_context.Tags.Single(t => t.Name == tag));
        
        foreach (var tag in query.Tags.ExceptBy(task.Tags, t => t.Name))
            query.Tags.Remove(_context.Tags.Single(t => t.Name == tag.Name));
        
        
        _context.SaveChanges();
        return Response.Updated;
    }

    public Response Delete(int taskId)
    {
        
        var query = _context.Tasks.Find(taskId);
        if (query == null) return Response.NotFound;
        var state = query.State;
        
        if (state == State.New)
        {
            _context.Tasks.Remove(query);
            _context.SaveChanges();
            return Response.Deleted;
        }

        if (state == State.Active)
        {
            query.State = State.Removed;
            _context.SaveChanges();
            
            return Response.Deleted;
        }

        if (state == State.Resolved || state == State.Closed || state == State.Removed) return Response.Conflict;

        return Response.BadRequest;

    }
}
