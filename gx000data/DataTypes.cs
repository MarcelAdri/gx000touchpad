namespace gx000data;

/// <summary>
/// Provides a collection of available data types.
/// </summary>
public sealed class DataTypes
{
    /// <summary>
    /// ThisInstance represents the singleton instance of the DataTypes class.
    /// </summary>
    /// <remarks>
    /// ThisInstance is used to access the singleton instance of the DataTypes class,
    /// which provides access to the available types for the data.
    /// </remarks>
    private static readonly DataTypes ThisInstance = new ();
    private readonly List<AvailableTypes> _validTypes = new()
    {
        AvailableTypes.StringType,
        AvailableTypes.IntType,
        AvailableTypes.LongType
    };

    /// <summary>
    /// Represents a class that provides information about available data types.
    /// </summary>
    /// <remarks>
    /// This class is meant to be used as a singleton. Use the <see cref="Instance"/> property to get the singleton instance.
    /// </remarks>
    private DataTypes()
    {
    }

    /// <summary>
    /// Represents the singleton instance of the DataTypes class.
    /// </summary>
    /// <remarks>
    /// The Instance property of the DataTypes class provides access to the singleton instance of the class.
    /// </remarks>
    public static DataTypes Instance
    {
        get
        {
            return ThisInstance;
        }
    }

    /// <summary>
    /// Represents the valid types for the data.
    /// </summary>
    /// <remarks>
    /// This property allows you to access the list of valid types for the data.
    /// </remarks>
    /// <example>
    /// The following code demonstrates how to use the ValidTypes property:
    /// <code>
    /// DataTypes dataTypes = DataTypes.Instance;
    /// IReadOnlyList&lt;AvailableTypes&gt; validTypes = dataTypes.ValidTypes;
    /// foreach (AvailableTypes type in validTypes)
    /// {
    /// Console.WriteLine(type);
    /// }
    /// </code>
    /// </example>
    public IReadOnlyList<AvailableTypes> ValidTypes
    {
        get
        {
            return _validTypes.AsReadOnly();
        }
    }

    /// <summary>
    /// Determines whether the specified type is an available data type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is available; otherwise, false.</returns>
    public bool IsAvailableType(AvailableTypes type)
    {
        return _validTypes.Contains(type);
    }
}