namespace MovieCardAPI.Model.ValueObjects;

public record DirectorVO(
    string Name,
    long DateOfBirth
);


//public record DirectorVO
//{
//    [JsonConstructor]
//    public DirectorVO(
//        string name,
//        long dateOfBirth,
//        ContactInformationVO contactInformation)
//    {
//        Name = name;
//        DateOfBirth = dateOfBirth;
//        ContactInformation = contactInformation;
//    }

//    public string Name { get; }
//    public long DateOfBirth { get; }
//    public ContactInformationVO ContactInformation { get; }
//}
