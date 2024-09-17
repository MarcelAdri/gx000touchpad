namespace gx000data;

/// <summary>
/// Class for variable definitions.
/// </summary>
public static class VariableDefinitions
{
    /// <summary>
    /// Represents the name of the first message.
    /// </summary>
    public const string FirstMessageName = "FirstMessage";

    /// <summary>
    /// Represents the name of the first number variable.
    /// </summary>
    public const string FirstNumberName = "FirstNumber";

    /// <summary>
    /// Represents the number of blocks.
    /// </summary>
    public const int NumberOfBlocks = 1;

    /// <summary>
    /// Represents the block size in bytes.
    /// </summary>
    public const int BlockSize = 256;

    /// <summary>
    /// Represents the byte size of the block number in the data block.
    /// </summary>
    public const int ByteSizeOfBlockNumber = 4;

    /// <summary>
    /// Represents the byte size of the checksum.
    /// </summary>
    public const int ByteSizeOfChecksum = 8;

    /// <summary>
    /// Represents the attributes of a variable.
    /// </summary>
    private readonly struct VariableAttributes : IVariableAttributes
    {
        public string VariableName { get; }
        public AvailableTypes Type { get; }
        public int Length {get; }
        public int BlockNumber { get; }
        public int OffsetInBlock { get; }

        public VariableAttributes(string variableName, AvailableTypes type, int length, int blockNum, int offSet)
        {
            VariableName = variableName;
            Type = type;
            Length = length;
            BlockNumber = blockNum;
            OffsetInBlock = offSet;
        }
    }

    /// <summary>
    /// Class for variable definitions.
    /// For each variable the attributes are hardcoded here.
    /// </summary>
    private static readonly IReadOnlyDictionary<string, IVariableAttributes> Variables =
        new Dictionary<string, IVariableAttributes>()
    {
        {
            FirstMessageName,
            new VariableAttributes(FirstMessageName, 
                AvailableTypes.StringType, 
                10, 
                1, 
                0x0004)
        },
        { FirstNumberName, 
            new VariableAttributes(FirstNumberName, 
                AvailableTypes.IntType, 
                4, 
                1, 
                0x000E) 
        }
        // add more variables here
    };

    /// <summary>
    /// Retrieves all the variables defined in the VariableDefinitions class.
    /// </summary>
    /// <returns>A read-only dictionary containing the variable name as the key and its attributes as the value.</returns>
    /// <remarks>
    /// The attributes of each variable, such as its type, length, block number, and offset, are hardcoded in the VariableDefinitions class.
    /// This method returns a dictionary where the variable name is the key and the variable attributes implementing the IVariableAttributes interface are the value.
    /// </remarks>
    public static IReadOnlyDictionary<string, IVariableAttributes> GetAllVariables()
    {
        return Variables;
    }

    /// <summary>
    /// Determines if the size of a variable matters based on its type.
    /// </summary>
    /// <param name="variableType">The type of the variable.</param>
    /// <returns>True if the size matters, false otherwise.</returns>
    public static bool SizeMatters(AvailableTypes variableType)
    {
        switch (variableType)
        {
            case AvailableTypes.StringType:
            {
                return true;
            }
            case AvailableTypes.IntType:
            case AvailableTypes.LongType:
            {
                return false;
            }
            default:
            {
                throw new InvalidOperationException($"Type definition of {variableType} incomplete");
            }
            
        }
    }

    /// <summary>
    /// Finds the variable attributes for a given variable name.
    /// </summary>
    /// <param name="variableName">The name of the variable.</param>
    /// <returns>The variable attributes for the specified variable name.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the variableName parameter is null or whitespace.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when no variable with the specified name is found.</exception>
    /// <remarks>Make sure to pass a valid variable name to this method in order to retrieve the corresponding variable attributes.</remarks>
    public static IVariableAttributes FindVariableAttributes(string variableName)
    {
        if (String.IsNullOrWhiteSpace(variableName))
            throw new ArgumentNullException(nameof(variableName), "variableName may not be null.");

        if (!Variables.TryGetValue(variableName, out IVariableAttributes thisVariable))
            throw new KeyNotFoundException($"No variable with tne name {variableName}.");

        return thisVariable;
    }
}