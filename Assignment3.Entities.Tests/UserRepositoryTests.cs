namespace Assignment3.Entities.Tests;

public class UserRepositoryTests
{
    [Fact]
    public void ReadNonExistantReturnsNull() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var userRepo = new UserRepository(context);
        
        var response = userRepo.Read(0);

        response.Should().Be(null);
    }

    [Fact]
    public void UpdateNonExistantReturnsNonFound() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var userRepo = new UserRepository(context);
        
        var response = userRepo.Update(new Core.UserUpdateDTO(0, "", ""));

        response.Should().Be(Core.Response.NotFound);
    }

    [Fact]
    public void DeleteNonExistantReturnsNotFound() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var userRepo = new UserRepository(context);
        
        var response = userRepo.Delete(0);

        response.Should().Be(Core.Response.NotFound);
    }

    [Fact]
    public void UpdateNonExistantReturnsNotFound() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var userRepo = new UserRepository(context);
        
        var response = userRepo.Update(new Core.UserUpdateDTO(0, "", ""));

        response.Should().Be(Core.Response.NotFound);
    }

    [Fact]
    public void ForceDeleteUserInUseReturnsDeleted() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var userRepo = new UserRepository(context);
        var taskRepo = new TaskRepository(context);
        var user = userRepo.Create(new Core.UserCreateDTO("username", "email"));
        taskRepo.Create(new Core.TaskCreateDTO("task", user.UserId, null, new List<string> {}));
        
        var response = userRepo.Delete(user.UserId, true);

        response.Should().Be(Core.Response.Deleted);
    }

    [Fact]
    public void NoForceDeleteUserInUseReturnsConflict() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var userRepo = new UserRepository(context);
        var taskRepo = new TaskRepository(context);
        var user = userRepo.Create(new Core.UserCreateDTO("username", "email"));
        taskRepo.Create(new Core.TaskCreateDTO("task", user.UserId, null, new List<string> {}));
        
        var response = userRepo.Delete(user.UserId, false);

        response.Should().Be(Core.Response.Conflict);
    }

    [Fact]
    public void SameEmailTwiceReturnsConflict() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var userRepo = new UserRepository(context);
        
        userRepo.Create(new Core.UserCreateDTO("username", "email"));
        var user = userRepo.Create(new Core.UserCreateDTO("username2", "email"));
        var response = user.Response;

        response.Should().Be(Core.Response.Conflict);
    }

    [Fact]
    public void UserCreatedReturnsCreated() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var userRepo = new UserRepository(context);
        
        var user = userRepo.Create(new Core.UserCreateDTO("username", "email"));
        var response = user.Response;

        response.Should().Be(Core.Response.Created);
    }

    [Fact]
    public void UserReadReturnsCorrectData() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var userRepo = new UserRepository(context);
        var user = userRepo.Create(new Core.UserCreateDTO("username", "email"));
        
        var readUser = userRepo.Read(user.UserId);
        var response = ( readUser.Id, readUser.Name, readUser.Email );

        response.Should().Be(( user.UserId, "username", "email" ));
    }

    [Fact]
    public void UserReadAfterUpdateReturnsCorrectData() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var userRepo = new UserRepository(context);
        var user = userRepo.Create(new Core.UserCreateDTO("username", "email"));
        userRepo.Update(new Core.UserUpdateDTO(user.UserId, "newname", "newmail"));
        
        var readUser = userRepo.Read(user.UserId);
        var response = ( readUser.Id, readUser.Name, readUser.Email );

        response.Should().Be(( user.UserId, "newname", "newmail" ));
    }
}
