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
        if (tagExists.Any()) return (Response.Conflict, tagExists.First().Id);

        var tagEntity = new Tag{ Name = tag.Name };
        _context.Tags.Add(tagEntity);
        _context.SaveChanges();

        return (Response.Created, tagEntity.Id);

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
        if (tag == null) return null!;
        
        return new TagDTO(tag.Id,tag.Name);

    }

    public Response Update(TagUpdateDTO tag)
    {

        var tagEntity = _context.Tags.Find(tag.Id);

        if (tagEntity == null) return Response.NotFound;

        tagEntity.Name = tag.Name;
        
        _context.SaveChanges();
        return Response.Updated;
    }

    public Response Delete(int tagId, bool force = false)
    {
        var tag = _context.Tags.Include(t => t.Tasks ).SingleOrDefault(t => t.Id == tagId );
        //var tag = _context.Tags.Find(tagId);
        
        if (tag == null)
            return Response.NotFound;
        
        if (tag.Tasks.Any() && !force)
            return Response.Conflict;

        if (force)
        {
            foreach (var t in tag.Tasks)
            {
                if (t.State == State.Active)
                    return Response.Conflict;

            }
            foreach (var t in tag.Tasks)
            {
                t.Tags.Remove(tag);
            }
        }

        _context.Tags.Remove(tag);
        _context.SaveChanges();
            
        return Response.Deleted;
        
    }
}
