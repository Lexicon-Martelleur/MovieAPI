
namespace MovieCardAPI.Model.ValueObjects;

public record ContactInformationVO(string Email, string PhoneNumber);

//public record ContactInformationVO
//{
//    [JsonConstructor]
//    public ContactInformationVO(string email, string phoneNumber)
//    {
//        Email = email;
//        PhoneNumber = phoneNumber;
//    }

//    public string Email { get; }
//    public string PhoneNumber { get; }
//}
