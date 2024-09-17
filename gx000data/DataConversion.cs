using System.Text;

namespace gx000data;

/// The DataConversion class provides static methods for converting data between byte arrays and specific types using
/// IDataConverter implementations.
/// /
public static class DataConversion
{
    /// <summary>
    /// Represents the character encoding for converting between strings and byte arrays.
    /// </summary>
    public static readonly Encoding Encoding = Encoding.UTF8;


    /// <summary>
    /// Converts the specified data value to an array of bytes using the provided converter.
    /// </summary>
    /// <typeparam name="T">The type of the data value.</typeparam>
    /// <param name="dataValue">The data value to convert.</param>
    /// <param name="converter">The converter that performs the conversion.</param>
    /// <returns>An array of bytes representing the converted data value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when converter is null</exception>
    public static byte[] ToBytes<T>(T dataValue, IDataConverter<T> converter)
    {
        if (converter == null)
        {
            throw new ArgumentNullException(nameof(converter));
        }

        return converter.ToBytes(dataValue);
    }

    /// <summary>
    /// Converts a byte array back into the original data value of type T.
    /// </summary>
    /// <typeparam name="T">The type of the original data value.</typeparam>
    /// <param name="bytes">The byte array containing the data value to convert.</param>
    /// <param name="converter">The converter used to convert the byte array to the original data value.</param>
    /// <returns>The original data value of type T.</returns>
    /// <exception cref="ArgumentNullException">Thrown when converter is null</exception>
    public static T FromBytes<T>(byte[] bytes, IDataConverter<T> converter)
    {
        if (converter == null)
        {
            throw new ArgumentNullException(nameof(converter));
        }
        return converter.FromBytes(bytes);
    }
    
}