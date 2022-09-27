namespace Assignment3.Entities.Tests;


public class TagRepositoryTests
{

    [Fact]
    public void DeleteNonExistantEntityReturnsNotFound() {
        
        var factory = new KanbanContextFactory();
        var context = factory.CreateDbTestContext(null);
        TagRepository tagRepo = new TagRepository(context);

        var response = tagRepo.Delete(1);

        response.Should().Be(Core.Response.NotFound);

    }

}
