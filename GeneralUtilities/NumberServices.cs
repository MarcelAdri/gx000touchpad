using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

namespace GeneralUtilities;

public static class NumberServices
{
    /// <summary>
    /// Converts a formatted number string into an unformatted numeric string.
    /// Removes group separators and replaces the decimal separator with '.'.
    /// Checks if the result is a valid number
    /// </summary>
    /// <param name="formattedNumber">The formatted number string to unformat.</param>
    /// <returns>A valid numeric string with group separators removed and decimal separator replaced.</returns>
    /// <exception cref="ArgumentNullException">Thrown when formattedNumber is null.</exception>
    /// <exception cref="ArgumentException">Thrown when formattedNumber is empty.</exception>
    /// <exception cref="FormatException">Thrown when the result string does not represent an number.</exception>
    public static string UnformatNumber(string formattedNumber)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(formattedNumber);
    
        var (decimalSeparator, groupSeparator) = GetSeparators(CultureInfo.CurrentCulture);
        const string patternTemplate = @"[^0-9{0}-]";
        var pattern = string.Format(patternTemplate, Regex.Escape(decimalSeparator));
        var result = Regex.Replace(formattedNumber.Replace(groupSeparator, string.Empty), pattern, "");
    
        bool isValidNumber = result.Contains(decimalSeparator)
            ? decimal.TryParse(result, out _)
            : BigInteger.TryParse(result, out _);

        if (isValidNumber)
        {
            return result;
        }

        throw new FormatException($"Invalid number format: {result}");
    }

    private static (string decimalSeparator, string groupSeparator) GetSeparators(CultureInfo cultureInfo)
    {
        var decimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator[0].ToString();
        var groupSeparator = cultureInfo.NumberFormat.NumberGroupSeparator[0].ToString();
        return (decimalSeparator, groupSeparator);
    }
}