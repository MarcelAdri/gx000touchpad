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
        
        var cultureInfo = CultureInfo.CurrentCulture;
        var isNumber = false;
        
        var decimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator[0].ToString();
        var groupSeparator = cultureInfo.NumberFormat.NumberGroupSeparator[0].ToString();

        string result = formattedNumber.Replace(groupSeparator, string.Empty);

        result = Regex.Replace(result, $@"[^0-9{Regex.Escape(decimalSeparator)}-]", "");

        if (result.Contains(decimalSeparator))
        {
            isNumber = decimal.TryParse(result, out _);
        }
        else
        {
            isNumber = BigInteger.TryParse(result, out _);
        }

        if (isNumber)
        {
            return result;
        }
        throw new FormatException($"Invalid number format: {result}");   
    }
}