namespace gx000data;

/// <summary>
/// Represents the attributes of a variable.
/// </summary>
public interface IVariableAttributes
{
    /// <summary>
    /// Represents the name of a variable.
    /// </summary>
    /// <remarks>
    /// The name of a variable is a string that uniquely identifies a variable.
    /// </remarks>
    public string VariableName { get; }

    /// <summary>
    /// Gets the type of the variable.
    /// </summary>
    public AvailableTypes Type { get; }

    /// <summary>
    /// Represents the Length property of a variable.
    /// </summary>
    /// <remarks>
    /// The Length property determines the size of the variable.
    /// </remarks>
    public int Length {get; }
    public int BlockNumber { get; }

    /// <summary>
    /// Represents the offset value of a variable within a data block.
    /// </summary>
    public int OffsetInBlock { get; }
}