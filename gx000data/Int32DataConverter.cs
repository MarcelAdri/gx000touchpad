namespace gx000data;

/// <summary>
/// Converts <see cref="Int32"/> data to and from bytes.
/// </summary>
public class Int32DataConverter : IDataConverter<int>
{
    /// <summary>
    /// Converts the provided data value to an array of bytes.
    /// </summary>
    /// <param name="dataValue">The data value to convert.</param>
    /// <returns>An array of bytes representing the converted data value.</returns>
    public byte[] ToBytes(int dataValue)
    {
        return GeneralUtilities.GeneralUtilities.StoreLittleEndian(BitConverter.GetBytes(dataValue));
    }

    /// <summary>
    /// Converts a byte array back into the original data value of type int.
    /// </summary>
    /// <param name="bytes">The byte array containing the data value to convert.</param>
    /// <returns>The original data value of type int.</returns>
    public int FromBytes(byte[] bytes)
    {
        return BitConverter.ToInt32(GeneralUtilities.GeneralUtilities.StoreLittleEndian(bytes));
    }
}