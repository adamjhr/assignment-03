namespace Assignment3.Entities.Tests;

public class TaskRepositoryTests
{

    [Fact]
    public void DeleteWithStateNewReturnsDeleted() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        var taskRepo = new TaskRepository(context);
        var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> {}));
        
        var response = taskRepo.Delete(task.TaskId);

        response.Should().Be(Core.Response.Deleted);
    }

    [Fact]
    public void StateIsNewUponCreation() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        var taskRepo = new TaskRepository(context);
        var task = taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> {}));
        
        var response = taskRepo.Read(task.TaskId).State;

        response.Should().Be(Core.State.New);
    }

}
