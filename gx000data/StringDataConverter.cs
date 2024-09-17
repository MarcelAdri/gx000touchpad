namespace gx000data;

/// <summary>
/// Converts <see cref="string"/> data to and from bytes.
/// </summary>
public class StringDataConverter : IDataConverter<string>
{
    /// <summary>
    /// Converts the provided data value to an array of bytes.
    /// </summary>
    /// <param name="dataValue">The data value to convert.</param>
    /// <returns>An array of bytes representing the converted data value.</returns>
    public byte[] ToBytes(string dataValue)
    {
        return DataConversion.Encoding.GetBytes(dataValue);
    }

    /// <summary>
    /// Converts a byte array back into the original data value of type string.
    /// </summary>
    /// <param name="bytes">The byte array containing the data value to convert.</param>
    /// <returns>The original data value of type int.</returns>
    public string FromBytes(byte[] bytes)
    {
        return DataConversion.Encoding.GetString(bytes);
    }
}