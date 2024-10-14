// /*
//     * Copyright (c) 2024 - 2024 Marcel Adriani
//     *
//     * This file is part of gx000touchpad.
// 
//      * gx000touchpad is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
//     * gx000touchpad is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
// 
//     * You should have received a copy of the GNU General Public License along with Foobar. If not, see <https://www.gnu.org/licenses/>.
// 
//     */

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
    /// Represents the name of the first number variable.
    /// </summary>
    public const string FirstLongName = "FirstLong";

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
        public string Type { get; }
        public int Length {get; }
        public int BlockNumber { get; }
        public int OffsetInBlock { get; }

        public VariableAttributes(string variableName, string type, int length, int blockNum, int offSet)
        {
            if (!DataTypes.Instance.IsAvailableType(type))
                throw new ArgumentException("Type {0} is not an available type", type);
            VariableName = variableName;
            Type = type;
            Length = length;
            BlockNumber = blockNum;
            OffsetInBlock = offSet;
        }
    }

    /// <summary>
    /// Dictionary with the attributes of all data variables, with the name of the variable as key.
    /// For each variable the attributes are hardcoded here and only here.
    /// </summary>
    private static readonly IReadOnlyDictionary<string, IVariableAttributes> AllVariables =
        new Dictionary<string, IVariableAttributes>()
    {
        {
            FirstMessageName,
            new VariableAttributes(FirstMessageName, 
                DataTypes.StringType, 
                10, 
                1, 
                0x0004)
        },
        { FirstNumberName, 
            new VariableAttributes(FirstNumberName, 
                DataTypes.IntType, 
                4, 
                1, 
                0x000E) 
        },
        { FirstLongName,
            new VariableAttributes(FirstLongName,
                DataTypes.LongType,
                8,
                1,
                0x0012
                )
            
        }
        // add more variables here
    };

    /// <summary>
    /// Gets the total number of defined variables.
    /// </summary>
    public static int NumberOfVariables => AllVariables.Count;
    
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
        return AllVariables;
    }

    /// <summary>
    /// Determines if the size of a variable matters based on its type.
    /// </summary>
    /// <param name="variableType">The type of the variable.</param>
    /// <returns>True if the size matters, false otherwise.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the passed variabletype is unknown</exception>
    public static bool SizeMatters(string variableType)
    {
        if (variableType == DataTypes.StringType)
        {
            return true;
        }
        if (variableType == DataTypes.IntType ||
                 variableType == DataTypes.LongType)
        {
            return false;
        }
        
        throw new InvalidOperationException($"Type definition of {variableType} incomplete");
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

        if (!AllVariables.TryGetValue(variableName, out IVariableAttributes thisVariable))
            throw new KeyNotFoundException($"No variable with the name {variableName}.");

        return thisVariable;
    }
}