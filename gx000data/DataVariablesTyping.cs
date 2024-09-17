namespace gx000data;

/// <summary>
/// Type-specific abstract base class for data variables.
/// Class is derived from DataVariables
/// </summary>
/// <typeparam name="T">The type of data value.</typeparam>
public abstract class DataVariablesTyping<T> : DataVariables
{
    /// <summary>
    /// Provides an abstract base class for data variables.
    /// The DataType property needs to be set in derived classes
    /// </summary>
    protected DataVariablesTyping(string variableName, DataExchange.DataStatus dataStatus)
        : base(variableName, dataStatus)
    {
    }

    /// <summary>
    /// Sets the data value for the data variable.
    /// </summary>
    /// <param name="dataToBeConverted">The data to be converted to the appropriate type and assigned as the data value.</param>
    public virtual void SetDataValue(T dataToBeConverted)
    {
        //assigning null is a placeholder, and needs to be changed in overriden instances of this method.
        this.DataValue = null;
    }

    /// <summary>
    /// Gets the data value of the data variable.
    /// </summary>
    /// <typeparam name="T">The type of the data variable.</typeparam>
    /// <returns>The data value of the data variable.</returns>
    public virtual T GetDataValue()
    {
        return default(T);
    }

}