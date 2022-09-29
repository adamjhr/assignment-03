namespace Assignment3.Entities.Tests;

public class TaskRepositoryTests
{

    [Fact]
    public void DeleteWithStateNewReturnsDeleted() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> {}));
        
        var response = taskRepo.Delete(task.TaskId);

        response.Should().Be(Core.Response.Deleted);
    }

    [Fact]
    public void StateIsNewUponCreation() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> {}));
        
        var response = taskRepo.Read(task.TaskId).State;

        response.Should().Be(Core.State.New);
    }

    [Fact]
    public void CreationTimeIsCorrectTime() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);

        var expectedTime = DateTime.Now;
        var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> {}));
        var response = taskRepo.Read(task.TaskId).Created;

        response.Should().BeCloseTo(expectedTime, TimeSpan.FromSeconds(5));
    }


    [Fact]
    public void UpdateTimeIsCorrectTimeAfterCreate() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);

        var expectedTime = DateTime.Now;
        var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> {}));
        var response = taskRepo.Read(task.TaskId).StateUpdated;

        response.Should().BeCloseTo(expectedTime, TimeSpan.FromSeconds(5));
    }
    [Fact]
    public void UpdateTimeIsCorrectTimeAfterUpdated() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> {}));
        
        var expectedTime = DateTime.Now;
        taskRepo.Update(new Core.TaskUpdateDTO(task.TaskId, "testTask", null, null, new List<string> {}, Core.State.Active));
        var response = taskRepo.Read(task.TaskId).StateUpdated;

        response.Should().BeCloseTo(expectedTime, TimeSpan.FromSeconds(5));
    }

    // [Theory]
    // [InlineData(Core.State.Active)]
    // [InlineData(Core.State.Closed)]
    // [InlineData(Core.State.Resolved)]
    // public void UpdateStateReturnsCorrectState(Core.State state) {

    //     var factory = new KanbanContextFactory();
    //     var context = factory.CreateDbTestContext(null);
    //     context.Database.EnsureDeleted();
    //     var taskRepo = new TaskRepository(context);
    //     var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> {}));
    //     var updated = taskRepo.Update(new Core.TaskUpdateDTO(task.TaskId, "testTask", null, null, new List<string> {}, state));
        
    //     var response = taskRepo.Read(task.TaskId).State;

    //     response.Should().Be(state);
    // }

    [Fact]
    public void UpdateStateToResolvedeReturnsResolved() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null!);
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> {}));
        var updated = taskRepo.Update(new Core.TaskUpdateDTO(task.TaskId, "testTask", null, null, new List<string> {}, Core.State.Resolved));
        
        var response = taskRepo.Read(task.TaskId).State;

        response.Should().Be(Core.State.Resolved);
    }

        [Fact]
    public void UpdateStateToActiveReturnsActive() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> {}));
        var updated = taskRepo.Update(new Core.TaskUpdateDTO(task.TaskId, "testTask", null, null, new List<string> {}, Core.State.Active));
        
        var response = taskRepo.Read(task.TaskId).State;

        response.Should().Be(Core.State.Active);
    }

        [Fact]
    public void UpdateStateToClosedReturnsClosed() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> {}));
        var updated = taskRepo.Update(new Core.TaskUpdateDTO(task.TaskId, "testTask", null, null, new List<string> {}, Core.State.Closed));
        
        var response = taskRepo.Read(task.TaskId).State;

        response.Should().Be(Core.State.Closed);
    }

    [Fact]
    public void DeleteWithStateActiveSetsStateToRemoved() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> {}));
        taskRepo.Update(new Core.TaskUpdateDTO(task.TaskId, "testTask", null, null, new List<string> {}, Core.State.Active));

        taskRepo.Delete(task.TaskId);
        var response = taskRepo.Read(task.TaskId).State;

        response.Should().Be(Core.State.Removed);
    }

    [Fact]
    public void DeleteWithStateRemovedReturnsConflict() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> {}));
        taskRepo.Update(new Core.TaskUpdateDTO(task.TaskId, "testTask", null, null, new List<string> {}, Core.State.Active));

        taskRepo.Delete(task.TaskId);
        var response = taskRepo.Delete(task.TaskId);

        response.Should().Be(Core.Response.Conflict);
    }

    [Fact]
    public void DeleteWithStateResolvedReturnsConflict() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> {}));
        taskRepo.Update(new Core.TaskUpdateDTO(task.TaskId, "testTask", null, null, new List<string> {}, Core.State.Resolved));

        var response = taskRepo.Delete(task.TaskId);

        response.Should().Be(Core.Response.Conflict);
    }

    [Fact]
    public void DeleteWithStateClosedReturnsConflict() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> {}));
        taskRepo.Update(new Core.TaskUpdateDTO(task.TaskId, "testTask", null, null, new List<string> {}, Core.State.Closed));

        var response = taskRepo.Delete(task.TaskId);

        response.Should().Be(Core.Response.Conflict);
    }

    [Fact]
    public void AssignNonExistantUserGivesBadRequest() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        
        var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", 1, null, new List<string> {}));
        var response = task.Response;

        response.Should().Be(Core.Response.BadRequest);
    }
    
    [Fact]
    public void DeleteNonExistantReturnsNotFound() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        
        var response = taskRepo.Delete(0);

        response.Should().Be(Core.Response.NotFound);
    }

    [Fact]
    public void UpdateNonExistantReturnsNotFound() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        
        var response = taskRepo.Update(new Core.TaskUpdateDTO(0, "testTask", null, null, new List<string> {}, Core.State.Closed));

        response.Should().Be(Core.Response.NotFound);
    }

    [Fact]
    public void ReadNonExistantReturnsNull() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        
        var response = taskRepo.Read(0);

        response.Should().Be(null);
    }

}
