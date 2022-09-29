namespace Assignment3.Entities.Tests;


public class TagRepositoryTests
{

    [Fact]
    public void DeleteNonExistantEntityReturnsNotFound() {
        
        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        TagRepository tagRepo = new TagRepository(context);

        var response = tagRepo.Delete(1);

        response.Should().Be(Core.Response.NotFound);

    }

    [Fact]
    public void ReadNonExistantReturnsNull() {
        
        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        TagRepository tagRepo = new TagRepository(context);

        var response = tagRepo.Read(1);

        response.Should().BeNull();

    }

    [Fact]
    public void TagAssignedToTaskReturnsConflict() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        TagRepository tagRepo = new TagRepository(context);
        TaskRepository taskRepo = new TaskRepository(context);
        var tag = tagRepo.Create(new Core.TagCreateDTO("testTag"));
        taskRepo.Create(new Core.TaskCreateDTO("testTask", null, null, new List<string> { "testTag" }));
    
        var response = tagRepo.Delete(tag.TagId, false);

        response.Should().Be(Core.Response.Conflict);
    
    }

    [Fact]
    public void DeleteTagReturnsDeleted() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        TagRepository tagRepo = new TagRepository(context);
        var tag = tagRepo.Create(new Core.TagCreateDTO("testTag"));
    
        var response = tagRepo.Delete(tag.TagId);

        response.Should().Be(Core.Response.Deleted);
    
    }

    [Fact]
    public void ForceDeleteAssignedTaskReturnsDeleted() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
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
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        TagRepository tagRepo = new TagRepository(context);
        tagRepo.Create(new Core.TagCreateDTO("testTag"));
        
        var tag = tagRepo.Create(new Core.TagCreateDTO("testTag"));
        var response = tag.Response;

        response.Should().Be(Core.Response.Conflict);
    }

    [Fact]
    public void TagReadAllReturns5Tags() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        var tagRepo = new TagRepository(context);
        tagRepo.Create(new Core.TagCreateDTO("tag1"));
        tagRepo.Create(new Core.TagCreateDTO("tag2"));
        tagRepo.Create(new Core.TagCreateDTO("tag3"));
        tagRepo.Create(new Core.TagCreateDTO("tag4"));
        tagRepo.Create(new Core.TagCreateDTO("tag5"));

        var tag = tagRepo.Create(new Core.TagCreateDTO("tag6"));
        tagRepo.Delete(tag.TagId);

        var readAll = tagRepo.ReadAll();
        var count = readAll.Count();

        count.Should().Be(5);
    }

    [Fact]
    public void UpdateNonExistingTagReturnsNotFound() {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        var tagRepo = new TagRepository(context);

        var response = tagRepo.Update(new Core.TagUpdateDTO(5, "tag"));

        response.Should().Be(Core.Response.NotFound);
    }

    [Fact]
    public void UpdateTagUpdatesCorrectly () {

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(new string[] {});
        context.Database.EnsureDeleted();
        var tagRepo = new TagRepository(context);
        var tag = tagRepo.Create(new Core.TagCreateDTO("tag"));
        tagRepo.Update(new Core.TagUpdateDTO(tag.TagId, "tagUpdated"));

        var updatedTag = tagRepo.Read(tag.TagId).Name;

        updatedTag.Should().Be("tagUpdated");

    }

}
