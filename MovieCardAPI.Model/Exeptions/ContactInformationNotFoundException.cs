namespace MovieCardAPI.Model.Exeptions;

public class ContactInformationNotFoundException
    : NotFoundException
{
    private static readonly string _reosurce = "Contact information";

    public ContactInformationNotFoundException(
        int id) : 
        base(GetMsg(_reosurce, id)) { }

    public ContactInformationNotFoundException(
        string id) :
        base(GetMsg(_reosurce, id)) { }

    public ContactInformationNotFoundException(
        Guid id) :
        base(GetMsg(_reosurce, id)) { }
}
