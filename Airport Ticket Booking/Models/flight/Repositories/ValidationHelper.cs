namespace Airport_Ticket_Booking.Models.flight.Repositories
{
    public static class ValidationHelper
{
    public static DateTime ValidateDate(string value, int line, List<string> errors)
    {
        if (DateTime.TryParse(value, out var date))
        {
            if (date < DateTime.Now)
            {
                AddError(errors, line, $"Departure date '{value}' cannot be in the past.");
            }
            return date;
        }

        AddError(errors, line, $"Invalid date format '{value}'.");
        return DateTime.MinValue;
    }

    public static decimal ValidateDecimal(string value, string fieldName, int line, List<string> errors)
    {
        if (decimal.TryParse(value, out var result) && result >= 0)
        {
            return result;
        }

        AddError(errors, line, $"Invalid {fieldName} '{value}', must be a positive number.");
        return 0;
    }

    public static string ValidateString(string value, string fieldName, int line, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            AddError(errors, line, $"{fieldName} cannot be empty.");
        }

        return value.Trim();
    }

    public static int ValidateInt(string value, string fieldName, int line, List<string> errors)
    {
        if (int.TryParse(value, out var result))
        {
            return result;
        }

        AddError(errors, line, $"Invalid {fieldName} '{value}', must be an integer.");
        return 0;
    }

    public static int? ValidateNullableInt(string value, string fieldName, int line, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return ValidateInt(value, fieldName, line, errors);
    }

    public static T ValidateEnum<T>(string value, string fieldName, int line, List<string> errors)
        where T : struct
    {
        if (Enum.TryParse(value, true, out T result))
        {
            return result;
        }

        AddError(errors, line, $"Invalid {fieldName} '{value}', must be a valid {typeof(T).Name}.");
        return default!;
    }

    public static bool ValidateBool(string value, string fieldName, int line, List<string> errors)
    {
        if (bool.TryParse(value, out var result))
        {
            return result;
        }

        AddError(errors, line, $"Invalid {fieldName} '{value}', must be 'true' or 'false'.");
        return false;
    }

    private static void AddError(List<string> errors, int line, string message)
    {
        errors.Add($"Line {line}: {message}");
    }
}

}