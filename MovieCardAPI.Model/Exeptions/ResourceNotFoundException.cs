namespace MovieCardAPI.Model.Exeptions;

public class ResourceNotFoundException : NotFoundException
{
    public ResourceNotFoundException(
        string resource,
        int id) : 
        base(GetMsg(resource, id)) { }

    public ResourceNotFoundException(
        string resource,
        string id) :
        base(GetMsg(resource, id)) { }

    public ResourceNotFoundException(
        string resource,
        Guid id) :
        base(GetMsg(resource, id)) { }

    public ResourceNotFoundException(
        string resource) :
        base(GetMsg(resource))
    { }
}
