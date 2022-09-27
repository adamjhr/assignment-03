using System.Collections.Immutable;
using System.Diagnostics.Tracing;
using Assignment3.Core;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Entities;
// mangler: Trying to delete a tag in use without the force should return Conflict.
public class TagRepository : ITagRepository
{
    private KanbanContext _context;
    public TagRepository(KanbanContext context)
    {
        _context = context;
        
    }

    public (Response Response, int TagId) Create(TagCreateDTO tag)
    {   
        if (_context.Tags.Find(tag) != null) return (Response.Conflict, 0);
        
        var t = _context.Tags.Add(new Tag());
        _context.SaveChanges();
        return (Response.Created, t.Entity.Id);

    }

    public IReadOnlyCollection<TagDTO> ReadAll()
    {
        var list = new List<TagDTO>();
        foreach (var tag in _context.Tags)
        {
            var dto = new TagDTO(tag.Id, tag.Name);
            list.Add(dto);
        }
        

        return list.ToImmutableList();

    }

    public TagDTO Read(int tagId)
    {
        var tag = _context.Tags.Find(tagId);
       
        return new TagDTO(tag.Id, tag.Name);

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
        if (force)
        {
            if (_context.Tags.Find(tagId) != null)
            {
                _context.Tags.Remove(_context.Tags.Find(tagId));
                _context.SaveChanges();
            }

            return Response.NotFound;

        }

        return Response.Conflict;
    }
}
