using System.ComponentModel.DataAnnotations;

namespace MovieCardAPI.Model.Validation;

public class UNIXTimestampValidationAttribute : ValidationAttribute
{
    public override bool IsValid(
        object? value)
    {
        if (value is not string input)
        {
            return false;
        }

        return IsUnixTimeStamp(input);
    }

    private bool IsUnixTimeStamp(string input)
    {
        if (!long.TryParse(input, out long timestamp))
        {
            return false;
        }
        long minUnixTimestamp = 0;
        long maxUnixTimestamp = DateTimeOffset.MaxValue.ToUnixTimeSeconds();
        return timestamp >= minUnixTimestamp &&
            timestamp <= maxUnixTimestamp;
    }
}
