namespace gx000data;

public class TypedVariable<T> 
{
    
    private readonly AvailableTypes _dataType;

    public TypedVariable(string variableName, DataExchange.DataStatus dataStatus, byte[] dataValue) : 
        base(variableName, dataStatus, dataValue)
    {
        
        if (!DataTypes.IsAvailableType(typeof(T)))
        {
            throw new ArgumentException($"Invalid type: {typeof(T)}");
        }
        
        //_dataType = DataTypes.GetAllPossibleTypes().FirstOrDefault(
            x => x.Value == typeof(T)).Key;
    }
    
    public TypedVariable(string variableName, DataExchange.DataStatus dataStatus, T dataValue) :
        this(variableName, dataStatus, DataConversion.ToBytes(dataValue, variableName))
    {
    }
    
   
}