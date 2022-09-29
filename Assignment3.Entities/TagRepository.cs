using System.Collections.Immutable;
using System.Diagnostics.Tracing;
using Assignment3.Core;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Entities;
// mangler: Trying to delete a tag in use without the force should return Conflict.
// Tags which are assigned to a task may only be deleted using the force.
public class TagRepository : ITagRepository
{
    private KanbanContext _context;
    public TagRepository(KanbanContext context)
    {
        _context = context;
        
    }

    public (Response Response, int TagId) Create(TagCreateDTO tag)
    {   
        var tagExists = from t in _context.Tags where t.Name == tag.Name select t;
        if (tagExists.Count() != 0) return (Response.Conflict, tagExists.First().Id);
        var newTag = _context.Tags.Add(new Tag{ Name = tag.Name });
        _context.SaveChanges();
        return (Response.Created, newTag.Entity.Id);

    }

    public IReadOnlyCollection<TagDTO> ReadAll()
    {
        var queryResult = _context.Tags
            .ToList();
        var taskDTOs = queryResult.Select(t => new TagDTO(t.Id,t.Name)).ToImmutableList();
        return taskDTOs;

    }

    public TagDTO Read(int tagId)
    {
        var tag = _context.Tags.SingleOrDefault(t => t.Id == tagId);
        if (tag == null) return null;
        
        return new TagDTO(tag.Id,tag.Name);

    }

    public Response Update(TagUpdateDTO tag)
    {
        if (_context.Tags.Find(tag.Id) != null)
        {
            _context.Tags.Find(tag.Id).Name = tag.Name;
            _context.Tags.Find(tag.Id).Id = tag.Id;
            _context.SaveChanges();
            return Response.Updated;
        }

        return Response.NotFound;
        
    }

    public Response Delete(int tagId, bool force = false)
    {
        //var tag = _context.Tags.Include(t => t.Tasks ).SingleOrDefault(t => t.Id == tagId );
        var tag = _context.Tags.Find(tagId);
        if (_context.Tags.Count() != 0) return Response.BadRequest;
        
        if (tag == null)
            return Response.NotFound;
        
        var tasks = from t in _context.Tasks where t.Tags.Contains(tag) select t;

        if (tasks.Count() != 0 && !force)
            return Response.Conflict;

        
        if (force)
        {
            foreach (var t in tasks)
            {
                if (t.State == State.Active)
                    return Response.Conflict;

            }
            foreach (var t in tasks)
            {
                //t.Tags.Remove(tag);
            }
        }

        _context.Tags.Remove(tag);
        _context.SaveChanges();
            
        return Response.NotFound;
            
        
    }
}
