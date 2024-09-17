namespace gx000data;

/// <summary>
/// Represents a string variable in the data exchange system.
/// </summary>
/// <remarks>
/// This class is used to store and manipulate string data in the data exchange system.
/// </remarks>
public class StringVariable : TypeVariable<string>, IVariable<string>
{
    /// <summary>
    /// Represents a string variable.
    /// </summary>
    public StringVariable(string variableName, DataExchange.DataStatus dataStatus, string dataValue) 
        : base(variableName, dataStatus, dataValue, AvailableTypes.StringType, new StringDataConverter())
    {
    }
}

