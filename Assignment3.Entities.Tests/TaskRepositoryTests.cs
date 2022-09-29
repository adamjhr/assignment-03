namespace Assignment3.Entities.Tests;

public class TaskRepositoryTests
{

    [Fact]
    public void DeleteWithStateNewReturnsDeleted() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> {}));
        
        var response = taskRepo.Delete(task.TaskId);

        response.Should().Be(Core.Response.Deleted);
    }

    [Fact]
    public void StateIsNewUponCreation() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> {}));
        
        var response = taskRepo.Read(task.TaskId).State;

        response.Should().Be(Core.State.New);
    }

    [Fact]
    public void CreationTimeIsCorrectTime() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
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
        var context = factory.CreateDbTestContext(new string[] {});
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
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> {}));
        
        var expectedTime = DateTime.Now;
        taskRepo.Update(new Core.TaskUpdateDTO(task.TaskId, "testTask", null, null, new List<string> {}, Core.State.Active));
        var response = taskRepo.Read(task.TaskId).StateUpdated;

        response.Should().BeCloseTo(expectedTime, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void UpdateStateToResolvedeReturnsResolved() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
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
        var context = factory.CreateDbTestContext(new string[] {});
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
        var context = factory.CreateDbTestContext(new string[] {});
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
        var context = factory.CreateDbTestContext(new string[] {});
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
        var context = factory.CreateDbTestContext(new string[] {});
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
        var context = factory.CreateDbTestContext(new string[] {});
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
        var context = factory.CreateDbTestContext(new string[] {});
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
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        
        var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", 1, null, new List<string> {}));
        var response = task.Response;

        response.Should().Be(Core.Response.BadRequest);
    }
    
    [Fact]
    public void DeleteNonExistantReturnsNotFound() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        
        var response = taskRepo.Delete(0);

        response.Should().Be(Core.Response.NotFound);
    }

    [Fact]
    public void UpdateNonExistantReturnsNotFound() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        
        var response = taskRepo.Update(new Core.TaskUpdateDTO(0, "testTask", null, null, new List<string> {}, Core.State.Closed));

        response.Should().Be(Core.Response.NotFound);
    }

    [Fact]
    public void ReadNonExistantReturnsNull() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        
        var response = taskRepo.Read(0);

        response.Should().Be(null);
    }

    [Fact]
    public void ReadAllReturns5Tasks() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        taskRepo.Create(new Core.TaskCreateDTO("task1", null, null, new List<string> {}));
        taskRepo.Create(new Core.TaskCreateDTO("task2", null, null, new List<string> {}));
        taskRepo.Create(new Core.TaskCreateDTO("task3", null, null, new List<string> {}));
        taskRepo.Create(new Core.TaskCreateDTO("task4", null, null, new List<string> {}));
        taskRepo.Create(new Core.TaskCreateDTO("task5", null, null, new List<string> {}));
        

        var total = taskRepo.ReadAll().Count();

        total.Should().Be(5);
    }

    [Fact]
    public void ReadAllByStateNewReturns3() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        taskRepo.Create(new Core.TaskCreateDTO("task1", null, null, new List<string> {}));
        taskRepo.Create(new Core.TaskCreateDTO("task2", null, null, new List<string> {}));
        taskRepo.Create(new Core.TaskCreateDTO("task3", null, null, new List<string> {}));
        var task4 = taskRepo.Create(new Core.TaskCreateDTO("task4", null, null, new List<string> {}));
        var task5 = taskRepo.Create(new Core.TaskCreateDTO("task5", null, null, new List<string> {}));

        taskRepo.Update(new Core.TaskUpdateDTO(task4.TaskId, "task4", null, null, new List<string> {}, Core.State.Active));
        taskRepo.Update(new Core.TaskUpdateDTO(task5.TaskId, "task5", null, null, new List<string> {}, Core.State.Active));

        var total = taskRepo.ReadAllByState(Core.State.New).Count();

        total.Should().Be(3);
    }

    [Fact]
    public void ReadAllByUserNewReturns2() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        var userRepo = new UserRepository(context);
        var user1 = userRepo.Create(new Core.UserCreateDTO("name", "email1"));
        var user2 = userRepo.Create(new Core.UserCreateDTO("name", "email2"));
        taskRepo.Create(new Core.TaskCreateDTO("task1", user1.UserId, null, new List<string> {}));
        taskRepo.Create(new Core.TaskCreateDTO("task2", null, null, new List<string> {}));
        taskRepo.Create(new Core.TaskCreateDTO("task3", null, null, new List<string> {}));
        taskRepo.Create(new Core.TaskCreateDTO("task4", user1.UserId, null, new List<string> {}));
        taskRepo.Create(new Core.TaskCreateDTO("task5", user2.UserId, null, new List<string> {}));

     
        var total = taskRepo.ReadAllByUser(user1.UserId).Count();

        total.Should().Be(2);
    }

    [Fact]
    public void ReadAllByTagNewReturns3() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        var tagRepo = new TagRepository(context);
        tagRepo.Create(new Core.TagCreateDTO("tag1"));
        tagRepo.Create(new Core.TagCreateDTO("tag2"));
        tagRepo.Create(new Core.TagCreateDTO("tag3"));
        taskRepo.Create(new Core.TaskCreateDTO("task1", null, null, new List<string> {"tag1"}));
        taskRepo.Create(new Core.TaskCreateDTO("task2", null, null, new List<string> {"tag2"}));
        taskRepo.Create(new Core.TaskCreateDTO("task3", null, null, new List<string> {"tag1", "tag3"}));
        taskRepo.Create(new Core.TaskCreateDTO("task4", null, null, new List<string> {}));
        taskRepo.Create(new Core.TaskCreateDTO("task5", null, null, new List<string> {"tag3"}));
        taskRepo.Create(new Core.TaskCreateDTO("task6", null, null, new List<string> {}));
        taskRepo.Create(new Core.TaskCreateDTO("task7", null, null, new List<string> {"tag1", "tag2", "tag3"}));

     
        var total = taskRepo.ReadAllByTag("tag1").Count();

        total.Should().Be(3);
    }

    [Fact]
    public void ReadTaskReturnsCorrectTitle() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        var taskRepo = new TaskRepository(context);
        var task = taskRepo.Create(new Core.TaskCreateDTO("task1", null, null, new List<string> {}));

        var readTask = taskRepo.Read(task.TaskId);
        var taskTitle = readTask.Title;

        taskTitle.Should().Be("task1");

    }

}
