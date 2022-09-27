using System.Diagnostics.Tracing;
using Assignment3.Core;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Entities;

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
        throw new NotImplementedException();
    }

    public TagDTO Read(int tagId)
    {
        throw new NotImplementedException();
    }

    public Response Update(TagUpdateDTO tag)
    {
        throw new NotImplementedException();
    }

    public Response Delete(int tagId, bool force = false)
    {
        throw new NotImplementedException();
    }
}
