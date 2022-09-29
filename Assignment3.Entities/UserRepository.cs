using Assignment3.Core;
using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Entities;

public class UserRepository : IUserRepository
{

    private KanbanContext _context;
    public UserRepository(KanbanContext context)
    {
        _context = context;   
    }

    public (Response Response, int UserId) Create(UserCreateDTO user) {

        if (_context.Users.Any(u => u.Email == user.Email)) return (Response.Conflict, 0); // HVILKET ID?        

        var userEntity = new User { Email = user.Email, Name = user.Name };

        _context.Users.Add(userEntity);
        _context.SaveChanges();
        
        return (Response.Created, userEntity.Id);
    }

    public IReadOnlyCollection<UserDTO> ReadAll() {
        var queryResult = _context.Users.ToList();
        var userDTOs = queryResult.Select(user => new UserDTO(user.Id, user.Name, user.Email)).ToImmutableList();
        return userDTOs;
    }

    public UserDTO Read(int userId) {

        var user = from u in _context.Users where u.Id == userId select new UserDTO( u.Id, u.Name, u.Email );

        if (user.Count() == 0) return null!;
        return user.First();

    }
    public Response Update(UserUpdateDTO user) {

        var userEntity = _context.Users.Find(user.Id);

        if (userEntity == null) return Response.NotFound;

        userEntity.Email = user.Email;
        userEntity.Name = user.Name;
        
        _context.SaveChanges();
        return Response.Updated;
    }

    public Response Delete(int userId, bool force = false) {

        var query = _context.Users.Find(userId);
        if (query == null) return Response.NotFound;

        var tasksWithUser = from t in _context.Tasks where t.AssignedTo.Id == userId select t;

        if (!force && tasksWithUser.Count() != 0) {
            return Response.Conflict;
        }

        foreach(var task in tasksWithUser) {
            task.AssignedTo = null!;
        }

        _context.Users.Remove(query);
        _context.SaveChanges();

        return Response.Deleted;

    }
}
 