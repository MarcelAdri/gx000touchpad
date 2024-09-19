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

using System.Collections.Concurrent;

namespace gx000data;

/// <summary>
/// <summary>
///Makes datablocks for communication between gx000 devices over the internet.
///The parameters (size, composition etc.) of the block derive from VariableDefinitions
/// </summary>
/// </summary>
public static class InternalCommBlock
{
    private const byte DefaultFiller = 32;
    
    private static readonly ConcurrentDictionary<int, ConcurrentBag<string>> BlockContents;

    static InternalCommBlock()
    {
        BlockContents = FillBlockContents();
    }
    
    /// <summary>
    /// Creates a data block for the specified block number,
    /// from the variables in the data store associated with the given block number.
    /// </summary>
    /// <param name="blockNumber">The number of the block.</param>
    /// <param name="dataStore">The data store object.</param>
    /// <returns>The byte array representing the data block.</returns>
    public static byte[] MakeBlock(int blockNumber, DataStore dataStore)
    {
        ValidateBlockNumber(blockNumber);

        var blockContents = GetBlockContents(blockNumber);
        var blockBytes = CreateBlockWithVariableContents(dataStore, blockContents);
        blockBytes = AddBlockProperties(blockBytes, blockNumber);

        return blockBytes;
    }
    /// <summary>
    /// Validates if the specified block number is valid within the range of blocks.
    /// </summary>
    /// <param name="blockNumber">The block number to validate.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the block number is out of range.</exception>
    private static void ValidateBlockNumber(int blockNumber)
    {
        if (blockNumber is < 1 or > VariableDefinitions.NumberOfBlocks)
        {
            throw new ArgumentOutOfRangeException(nameof(blockNumber), "No such block");
        }
    }

    /// <summary>
    /// Get the contents of a specific block.
    /// </summary>
    /// <param name="blockNumber">The number of the block to get the contents of.</param>
    /// <returns>A ConcurrentBag of strings representing the contents of the block.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when an unknown blocknumber was passed.</exception>
    private static ConcurrentBag<string> GetBlockContents(int blockNumber)
    {
        if (BlockContents.TryGetValue(blockNumber, out var blockContents))
            return blockContents;

        // handle error condition when blockNumber not found
        throw new KeyNotFoundException($"Block with number '{blockNumber}' was not found.");
    }

    /// <summary>
    /// Fills the contents of the internal communication block with variables from the VariableDefinitions class.
    /// </summary>
    /// <returns>A concurrent dictionary containing the block number as the key and a concurrent bag of variable names as the value.</returns>
    /// <remarks>
    /// This method retrieves all the variables defined in the VariableDefinitions class and adds them to the internal communication block.
    /// The block number and variable names are stored in a concurrent dictionary where the block number is the key and the variable names are stored in a concurrent bag.
    /// </remarks>
    private static ConcurrentDictionary<int, ConcurrentBag<string>> FillBlockContents()
    {
        var outputBlockContents = new ConcurrentDictionary<int, ConcurrentBag<string>>();
        
        foreach (var variable in VariableDefinitions.GetAllVariables())
        {
            var bag = outputBlockContents.GetOrAdd(variable.Value.BlockNumber, _ => new ConcurrentBag<string>());
            bag.Add(variable.Value.VariableName);
        }

        return outputBlockContents;
    }

    /// <summary>
    /// Creates a data block with variable contents based on the given DataStore object and blockContents.
    /// </summary>
    /// <param name="dataStore">The DataStore object used to retrieve variable contents.</param>
    /// <param name="blockContents">A ConcurrentBag of variable names to include in the block.</param>
    /// <returns>A byte array representing the block with variable contents.</returns>
    private static byte[] CreateBlockWithVariableContents(DataStore dataStore, ConcurrentBag<string> blockContents)
    {
        var blockBytes = new byte[VariableDefinitions.BlockSize];
        var totalLength = 0;
        foreach (var variableName in blockContents)
        {
            var variableAttributes = VariableDefinitions.FindVariableAttributes(variableName);

            byte[] contentsValue = GetVariableBytesOrDefault(dataStore, variableName, variableAttributes.Length);

            totalLength += variableAttributes.Length;
            Array.Copy(contentsValue,
                0,
                blockBytes,
                variableAttributes.OffsetInBlock,
                variableAttributes.Length);
        }
        blockBytes = AddFiller(blockBytes, totalLength);
        return blockBytes;
    }

    /// <summary>
    /// Gets the bytes of a variable from the data store, or returns a default value if the variable is not found.
    /// </summary>
    /// <param name="dataStore">The data store object.</param>
    /// <param name="variableName">The name of the variable.</param>
    /// <param name="length">The length of the variable in bytes.</param>
    /// <returns>The byte array representing the variable's value, or a default value if the variable is not found.</returns>
    private static byte[] GetVariableBytesOrDefault(DataStore dataStore, string variableName, int length)
    {
        if (dataStore.TryGetDataFromStore(variableName, out Variable variableContents))
        {
            return variableContents.GetValueBytes();
        }
    
        var contentsValue = new byte[length];
        Array.Fill(contentsValue, DefaultFiller);
        return contentsValue;
    }
    /// <summary>
    /// Adds block properties to the given block bytes.
    /// </summary>
    /// <param name="blockBytes">The block bytes to which block properties are added.</param>
    /// <param name="blockNumber">The block number to be added as a property.</param>
    /// <returns>The block bytes with block properties added.</returns>
    private static byte[] AddBlockProperties(byte[] blockBytes, int blockNumber)
    {
        blockBytes = AddBlockNumber(blockBytes, blockNumber);
        blockBytes = AddChecksum(blockBytes);
        return blockBytes;
    }

    /// <summary>
    /// Extracts data variables from a given data block.
    /// </summary>
    /// <param name="dataBlock">The data block to extract variables from.</param>
    /// <returns>A list of extracted data variables.</returns>
    /// <exception cref="ArgumentException">Thrown when block size or checksum are not OK.</exception>
    public static IReadOnlyList<Variable> ExtractBlock(byte[] dataBlock)
    {
        if (dataBlock.Length != VariableDefinitions.BlockSize || !ChecksumChecksOut(dataBlock))
        {
            throw new ArgumentException("Invalid block passed into method.", nameof(dataBlock));
        }

        var outputList = new List<Variable>();
        var blockNumber = Convert.ToInt32(GeneralUtilities.GeneralUtilities.StoreLittleEndian(
            dataBlock[..VariableDefinitions.ByteSizeOfBlockNumber]));
        foreach (var variableName in BlockContents[blockNumber])
        {
            var attributes = VariableDefinitions.FindVariableAttributes(variableName);
            var dataValue = dataBlock[attributes.OffsetInBlock..(attributes.OffsetInBlock + attributes.Length)];
        
            if (IsDataNotEmpty(dataValue, attributes.Length))
            {
                switch (attributes.Type)
                {
                    case AvailableTypes.StringType:
                        outputList.Add(ExtractStringVariable(attributes, dataBlock));
                        break;
                    case AvailableTypes.IntType:
                        outputList.Add(ExtractInt32Variable(attributes, dataBlock));
                        break;
                    case AvailableTypes.LongType:
                        outputList.Add(ExtractInt64Variable(attributes, dataBlock));
                        break;
                    default:
                        throw new ArgumentException($"Variable type {attributes.Type.ToString()} not implemented.");
                }
            }
        }
        return outputList;
    }

    /// <summary>
    /// Checks whether the given data array is not empty.
    /// </summary>
    /// <param name="data">The data array to be checked.</param>
    /// <param name="length">The length of the data array.</param>
    /// <returns>True if the data array is not empty, otherwise false.</returns>
    private static bool IsDataNotEmpty(byte[] data, int length)
    {
        var emptyDataValue = new byte[length];
        Array.Fill(emptyDataValue, DefaultFiller);
        return Enumerable.SequenceEqual(data, emptyDataValue);
    }
    /// <summary>
    /// Extracts the checksum value from a data block.
    /// </summary>
    /// <param name="dataBlock">The data block from which to extract the checksum.</param>
    /// <returns>The checksum value extracted from the data block.</returns>
    private static ulong ExtractChecksumFromBlock(byte[] dataBlock)
    {
        return Convert.ToUInt64(GeneralUtilities.GeneralUtilities.StoreLittleEndian(
            dataBlock[(VariableDefinitions.BlockSize - VariableDefinitions.ByteSizeOfChecksum)..]));
    }

    /// <summary>
    /// Calculates the checksum for a given data block using the Fletcher-64 checksum algorithm.
    /// </summary>
    /// <param name="dataBlock">The data block for which the checksum needs to be calculated.</param>
    /// <returns>The calculated checksum as an unsigned 64-bit integer.</returns>
    private static ulong CalculateChecksum(byte[] dataBlock)
    {
        return GeneralUtilities.FletcherCheckSum.CalculateChecksumFromBytes(
            dataBlock[..^VariableDefinitions.ByteSizeOfChecksum],
            GeneralUtilities.FletcherCheckSum.FletcherSizes.FletcherTypes.Fletcher64);
    }
    /// <summary>
    /// Checks if the checksum of the data block is valid.
    /// </summary>
    /// <param name="dataBlock">The data block to check.</param>
    /// <returns>
    /// <c>true</c> if the checksum is valid; otherwise, <c>false</c>.
    /// </returns>
    private static bool ChecksumChecksOut(byte[] dataBlock)
    {
        ValidateDataBlockSize(dataBlock);
    
        ulong checksumFromBlock = ExtractChecksumFromBlock(dataBlock);
        ulong checksumCalculated = CalculateChecksum(dataBlock);

        return checksumFromBlock == checksumCalculated;
    }

    /// <summary>
    /// Validates the data block size.
    /// </summary>
    /// <param name="dataBlock">The data block to validate.</param>
    private static void ValidateDataBlockSize(byte[] dataBlock)
    {
        if (dataBlock == null)
        {
            throw new ArgumentNullException(nameof(dataBlock));
        }
        if (dataBlock.Length != VariableDefinitions.BlockSize)
        {
            throw new ArgumentException("dataBlock is not of a valid length");
        }
    }
    /// <summary>
    /// Extracts a string variable from a data block.
    /// </summary>
    /// <param name="variable">The variable attributes.</param>
    /// <param name="dataBlock">The data block.</param>
    /// <returns>The extracted string variable.</returns>
    private static StringVariable ExtractStringVariable(IVariableAttributes variable, byte[] dataBlock)
    {
        return new StringVariable(
            variable.VariableName,
            DataExchange.DataStatus.FromClientToSim,
           DataConversion.FromBytes(dataBlock[variable.OffsetInBlock..(variable.OffsetInBlock + variable.Length)], 
               new StringDataConverter()));
    }

    /// <summary>
    /// Extracts an Int32 variable from a data block.
    /// </summary>
    /// <param name="variable">The variable attributes.</param>
    /// <param name="dataBlock">The data block.</param>
    /// <returns>The extracted Int32 variable.</returns>
    private static Int32Variable ExtractInt32Variable(IVariableAttributes variable, byte[] dataBlock)
    {
        return new Int32Variable(
            variable.VariableName,
            DataExchange.DataStatus.FromClientToSim,
            DataConversion.FromBytes(dataBlock[variable.OffsetInBlock..(variable.OffsetInBlock + variable.Length)],
                new Int32DataConverter()));
    }

    /// <summary>
    /// Extracts an Int64 variable from a data block.
    /// </summary>
    /// <param name="variable">The variable attributes.</param>
    /// <param name="dataBlock">The data block to extract the variable from.</param>
    /// <returns>The extracted Int64 variable.</returns>
    private static Int64Variable ExtractInt64Variable(IVariableAttributes variable, byte[] dataBlock)
    {
        return new Int64Variable(
            variable.VariableName,
            DataExchange.DataStatus.FromClientToSim,
            DataConversion.FromBytes(dataBlock[variable.OffsetInBlock..(variable.OffsetInBlock + variable.Length)],
                new Int64DataConverter()));
    }

    /// <summary>
    /// Adds the block number to the given byte array.
    /// </summary>
    /// <param name="blockBytes">The byte array to add the block number to.</param>
    /// <param name="blockNumber">The block number to add.</param>
    /// <returns>The byte array with the block number added.</returns>
    private static byte[] AddBlockNumber(byte[] blockBytes, int blockNumber)
    {
        Array.Copy(GeneralUtilities.GeneralUtilities.StoreLittleEndian(BitConverter.GetBytes(blockNumber)),
            0,
            blockBytes,
            0,
            VariableDefinitions.ByteSizeOfBlockNumber);
        return blockBytes;
    }

    /// <summary>
    /// Adds filler bytes to the given block of bytes.
    /// </summary>
    /// <param name="blockBytes">The block of bytes to which filler bytes are added.</param>
    /// <param name="totalLength">The total length of the data in the block, excluding filler bytes.</param>
    /// <returns>The block of bytes with filler bytes added.</returns>
    private static byte[] AddFiller(byte[] blockBytes, int totalLength)
    {
        var lengthOfFiller = VariableDefinitions.BlockSize - (
            VariableDefinitions.ByteSizeOfBlockNumber + 
            VariableDefinitions.ByteSizeOfChecksum + totalLength);
        if (lengthOfFiller < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lengthOfFiller), "Invalid block length.");
        }

        var startOfFiller = totalLength + VariableDefinitions.ByteSizeOfBlockNumber;
        Array.Fill<byte>(blockBytes, 
            0xFF, 
            startOfFiller, 
            lengthOfFiller);

        return blockBytes;
    }

    /// <summary>
    /// Adds a checksum to the given block bytes.
    /// </summary>
    /// <param name="blockBytes">The block bytes to add the checksum to.</param>
    /// <returns>The block bytes with the checksum added.</returns>
    private static byte[] AddChecksum(byte[] blockBytes)
    {
        var checkSum = GeneralUtilities.FletcherCheckSum.CalculateChecksumFromBytes(
            blockBytes[..^VariableDefinitions.ByteSizeOfChecksum],
            GeneralUtilities.FletcherCheckSum.FletcherSizes.FletcherTypes.Fletcher64);
        var checkSumOffset = VariableDefinitions.BlockSize - VariableDefinitions.ByteSizeOfChecksum;
        
        Array.Copy(GeneralUtilities.GeneralUtilities.StoreLittleEndian(BitConverter.GetBytes(checkSum)),
            0,
            blockBytes,
            checkSumOffset,
            VariableDefinitions.ByteSizeOfChecksum);

        return blockBytes;
    }

}