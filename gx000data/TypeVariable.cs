namespace gx000data;

/// <summary>
/// Represents a variable in the data exchange system.
/// </summary>
/// <typeparam name="T">The type of the variable.</typeparam>
public abstract class TypeVariable<T> : Variable
{
    /// <summary>
    /// Represents a data value converted into bytes[].
    /// </summary>
    private byte[] _dataValue;

    /// <summary>
    /// Represents a variable with a specified type and data converter.
    /// </summary>
    /// <typeparam name="T">The type of the variable.</typeparam>
    private readonly IDataConverter<T> _converter;
    private readonly object _lock = new();

    /// <summary>
    /// Represents the value of a variable.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the value.</typeparam>
    public T Value
    {
        get
        {
            lock (_lock)
            {
                return DataConversion.FromBytes(_dataValue, _converter);    
            }
        }
        set
        {
            lock(_lock)
            {
                byte[] valueInBytes = DataConversion.ToBytes(value, _converter);
                _dataValue = SetDataLength(valueInBytes, VariableName);
            }
        }
    }

    public override byte[] GetValueBytes()
    {
        return _dataValue;
    }

    /// <summary>
    /// Gets or sets the name of the variable.
    /// </summary>
    protected TypeVariable(
        string variableName,
        DataExchange.DataStatus dataStatus,
        T dataValue,
        AvailableTypes expectedType,
        IDataConverter<T> converter) 
            : base(variableName, dataStatus)
    {
        AssertVariableType(variableName, expectedType);
        _converter = converter;
        Value = dataValue;
    }

    /// <summary>
    /// Returns the provided data adjusted to be consistent with the data definition for the variable.
    /// If the length of the provided data is too long, the superfluous bytes at the end are omitted.
    /// If the length of the provided data is too short, the data is padded at the end with spaces
    /// </summary>
    /// <param name="dataValue">The data value to be adjusted for length.</param>
    /// <param name="variableName">The name of the variable.</param>
    /// <returns>The adjusted data value.</returns>
    private byte[] SetDataLength(byte[] dataValue, string variableName)
    {
        IVariableAttributes thisVariable = VariableDefinitions.FindVariableAttributes(variableName);
        var requiredLength = thisVariable.Length;
        if (VariableDefinitions.SizeMatters(thisVariable.Type))
        {
            if (dataValue.Length > requiredLength)
            {
                return DecreaseDataLength(dataValue, requiredLength);
            }

            if (dataValue.Length < requiredLength)
            {
                return IncreaseDataLength(dataValue, requiredLength);
            }
        }
        return dataValue;
    }

    /// <summary>
    /// Increases the length of the given byte array to the specified required length.
    /// </summary>
    /// <param name="dataValue">The byte array to be increased.</param>
    /// <param name="requiredLength">The required length of the byte array.</param>
    /// <returns>The byte array with increased length.</returns>
    private byte[] IncreaseDataLength(byte[] dataValue, int requiredLength)
    {
        var result = new byte[requiredLength];
        Array.Copy(dataValue, result, dataValue.Length);
        for (var i = dataValue.Length; i< requiredLength; i++)
        {
            result[i] = 32;
        }

        return result;
    }

    /// <summary>
    /// Decreases the length of the given data if it exceeds the required length.
    /// </summary>
    /// <param name="dataValue">The data to decrease the length of.</param>
    /// <param name="requiredLength">The required length of the data.</param>
    /// <returns>The data with the decreased length.</returns>
    private byte[] DecreaseDataLength(byte[] dataValue, int requiredLength)
    {
        var result = new byte[requiredLength];
        Array.Copy(
            dataValue, 
            result, 
            requiredLength);
        return result;
    }

    /// <summary>
    /// Asserts the type of a variable.
    /// </summary>
    /// <param name="variableName">The name of the variable.</param>
    /// <param name="expectedType">The expected type of the variable.</param>
    /// <exception cref="ArgumentException">Thrown when variableName is empty
    /// or expectedType is not consistent with the variable definition.</exception>
    private void AssertVariableType(string variableName, AvailableTypes expectedType)
    {
        if (String.IsNullOrWhiteSpace(variableName))
        {
            throw new ArgumentException("Variable name cannot be empty.");
        }

        IVariableAttributes thisVariable = VariableDefinitions.FindVariableAttributes(variableName);
        if (thisVariable.Type != expectedType)
        {
            throw new ArgumentException("Variable type mismatch.");
        }
    }
}