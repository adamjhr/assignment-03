namespace Assignment3.Entities.Tests;


public class TagRepositoryTests
{

    [Fact]
    public void DeleteNonExistantEntityReturnsNotFound() {
        
        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        TagRepository tagRepo = new TagRepository(context);

        var response = tagRepo.Delete(1);

        response.Should().Be(Core.Response.NotFound);

    }

    [Fact]
    public void ReadNonExistantReturnsNull() {
        
        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        TagRepository tagRepo = new TagRepository(context);

        var response = tagRepo.Read(1);

        response.Should().BeNull();

    }

    [Fact]
    public void TagAssignedToTaskReturnsConflict() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        TagRepository tagRepo = new TagRepository(context);
        TaskRepository taskRepo = new TaskRepository(context);
        var tag = tagRepo.Create(new Core.TagCreateDTO("testTag"));
        taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> { "testTag" }));
    
        var response = tagRepo.Delete(tag.TagId, false);

        response.Should().Be(Core.Response.Conflict);
    
    }

    [Fact]
    public void TagAssignedToTaskReturnsDeletedWhenForced() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        TagRepository tagRepo = new TagRepository(context);
        TaskRepository taskRepo = new TaskRepository(context);
        var tag = tagRepo.Create(new Core.TagCreateDTO("testTag"));
        taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> { "testTag" }));
    
        var response = tagRepo.Delete(tag.TagId, true);

        response.Should().Be(Core.Response.Deleted);
    
    }

    [Fact]
    public void CreateSameTagTwiceReturnsConflict() {
        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        context.Database.EnsureDeleted();
        TagRepository tagRepo = new TagRepository(context);
        tagRepo.Create(new Core.TagCreateDTO("testTag"));
        
        var tag = tagRepo.Create(new Core.TagCreateDTO("testTag"));
        var response = tag.Response;

        response.Should().Be(Core.Response.Conflict);
    }

}
