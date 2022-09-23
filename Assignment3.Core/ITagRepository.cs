namespace Assignment3.Core;

public interface ITagRepository
{
    (Response Response, int TagId) Create(TagCreateDTO tag);
    IReadOnlyCollection<TagDTO> ReadAll();
    TagDTO Read(int tagId);
    Response Update(TagUpdateDTO tag) {
        return Response.NotFound;
    }
    Response Delete(int tagId, bool force = false) {
        return Response.NotFound;
    }
}