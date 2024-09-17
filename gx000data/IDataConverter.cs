namespace gx000data;

/// <summary>
/// Represents an interface for converting data to and from bytes.
/// </summary>
/// <typeparam name="T">The type of data to convert.</typeparam>
public interface IDataConverter<T>
{
    /// <summary>
    /// Converts the provided data value to an array of bytes.
    /// </summary>
    /// <typeparam name="T">The type of data to convert.</typeparam>
    /// <param name="dataValue">The data value to convert.</param>
    /// <returns>An array of bytes representing the converted data value.</returns>
    byte[] ToBytes(T dataValue);

    /// <summary>
    /// Converts a byte array back into the original data value of type T. </summary> <param name="bytes">The byte array containing the data value to convert.</param> <typeparam name="T">The type of the original data value.</typeparam> <returns>The original data value of type T.</returns>
    /// /
    T FromBytes(byte[] bytes);
}