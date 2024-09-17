namespace gx000data;

/// <summary>
/// Abstract base class for data variables.
/// </summary>
public abstract class DataVariables
{
    /// <summary>
    /// Represents the available data types for data variables.
    /// </summary>
    public enum DataTypes
    {
        /// <summary>
        /// Represents a data variable of type string.
        /// </summary>
        String = 0,

        /// <summary>
        /// Represents a 32-bit signed integer data type.
        /// </summary>
        Int32 = 1,

        /// <summary>
        /// Represents the Int64 data type.
        /// </summary>
        Int64 = 2,

        /// <summary>
        /// Represents the None member of the DataTypes enumeration.
        /// </summary>
        None = 9
    }
    /// <summary>
    /// Gets the name of the variable.
    /// </summary>
    public string VariableName { get; private set; }

    /// <summary>
    /// Gets the data type of the data variable.
    /// </summary>
    public DataTypes DataType { get; protected set; }

    /// <summary>
    /// Represents a data variable with a byte array value.
    /// </summary>
    protected byte[] DataValue { get; set; }

    /// <summary>
    /// Represents the data status of a data variable.
    /// </summary>
    private DataExchange.DataStatus DataStatus { get; set; }

    /// <summary>
    /// Provides an abstract base class for data variables.
    /// The DataType property needs to be set in derived classes
    /// </summary>
    protected DataVariables(string variableName, DataExchange.DataStatus dataStatus)
    {
        AssignVariableName(variableName, dataStatus);
    }

    protected DataVariables(string variableName, DataExchange.DataStatus dataStatus, byte[] dataValue) : 
        this(variableName, dataStatus)
    {
        SetValueBytes(dataValue);
    }

    /// <summary>
    /// Represents assignment of variable name for a data variable.
    /// </summary>
    /// <remarks>
    /// This method is internal and should not be called directly. It is used internally in the implementation of
    /// derived classes of DataVariables.
    /// </remarks>
    /// <param name="variableName">The name of the variable.</param>
    /// <param name="dataStatus">The status of the data for the variable.</param>
    private void AssignVariableName(string variableName, DataExchange.DataStatus dataStatus)
    {
        this.VariableName = variableName;
        SetStatus(dataStatus);
    }

    /// <summary>
    /// Gets the data status of the data variable.
    /// </summary>
    /// <returns>The data status of the data variable.</returns>
    public DataExchange.DataStatus GetStatus()
    {
        return DataStatus;
    }
    /// <summary>
    /// Sets the status of the data variable.
    /// </summary>
    /// <param name="dataStatus">The status to be set.</param>
    public void SetStatus(DataExchange.DataStatus dataStatus)
    {
        this.DataStatus = dataStatus;
    }

    /// <summary>
    /// Gets the value of the data variable.
    /// </summary>
    /// <returns>The value of the data variable as a byte array.</returns>
    public byte[] GetValueBytes()
    {
        return DataValue;
    }

    /// <summary>
    /// Sets the value of the data variable.
    /// </summary>
    /// <param name="value">The value to be set.</param>
    /// <exception cref="ArgumentException">Thrown when the length of value is not right for this variable.</exception>
    public void SetValueBytes(byte[] value)
    {
        if (value.Length != VariableDefinitions.GetLength(this.VariableName))
        {
            throw new ArgumentException("Invalid length of data.", nameof(value));
        }
        DataValue = value;
    }

    
}