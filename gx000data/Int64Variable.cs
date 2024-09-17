namespace gx000data;

/// <summary>
/// Represents a variable of type Int64 in the data exchange system.
/// </summary>
public class Int64Variable : TypeVariable<long>, IVariable<long>
{
    /// <summary>
    /// Represents a variable of type Int64.
    /// </summary>
    public Int64Variable(string variableName, DataExchange.DataStatus dataStatus, long dataValue) 
        : base(variableName, dataStatus, dataValue, AvailableTypes.LongType, new Int64DataConverter())
    {
    }
}