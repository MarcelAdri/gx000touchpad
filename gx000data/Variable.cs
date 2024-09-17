namespace gx000data;

public abstract class Variable
{
    /// <summary>
    /// Represents the data status of a variable.
    /// </summary>
    private DataExchange.DataStatus _dataStatus;
    
    /// <summary>
    /// Represents a variable with a specific data type.
    /// </summary>
    public string VariableName { get; private set; }

    public event EventHandler<StatusChangedEventArgs> StatusChanged;

    /// <summary>
    /// Represents a variable with a status and value of type T.
    /// </summary>
    public DataExchange.DataStatus Status
    {
        get => _dataStatus;
        set
        {
            if (!DataExchange.StatusChangeIsOK(_dataStatus, value))
            {
                throw new InvalidOperationException("Change of data status refused.");
            }
            _dataStatus = value;
            OnStatusChanged();
        }
    }

    protected Variable(string variableName,
        DataExchange.DataStatus dataStatus)
    {
        VariableName = variableName;
        Status = dataStatus;
    }

    public virtual byte[] GetValueBytes()
    {
        return [];
    }

    protected virtual void OnStatusChanged()
    {
        StatusChanged?.Invoke(this, new StatusChangedEventArgs { variable = this });
    }
}