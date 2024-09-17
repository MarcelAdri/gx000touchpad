namespace gx000data;

/// <summary>
/// Represents a variable of type Int32 in the data exchange system.
/// </summary>
public class Int32Variable : TypeVariable<int>, IVariable<int>
{
    /// <summary>
    /// Represents a variable of type int.
    /// </summary>
    public Int32Variable(string variableName, DataExchange.DataStatus dataStatus, int dataValue) 
        : base(variableName, dataStatus, dataValue, AvailableTypes.IntType, new Int32DataConverter())
    {
    }
}