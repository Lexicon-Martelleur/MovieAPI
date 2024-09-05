using System.ComponentModel.DataAnnotations;

namespace MovieCardAPI.Model.Validation;

public class UNIXTimestampValidationAttribute : ValidationAttribute
{
    public override bool IsValid(
        object? value)
    {
        if (value is not long input)
        {
            return false;
        }

        return IsUnixTimeStamp(input);
    }

    private bool IsUnixTimeStamp(long input)
    {
        long minUnixTimestamp = 0;
        long maxUnixTimestamp = DateTimeOffset.MaxValue.ToUnixTimeSeconds();
        
        return input >= minUnixTimestamp && input <= maxUnixTimestamp;
    }
}
