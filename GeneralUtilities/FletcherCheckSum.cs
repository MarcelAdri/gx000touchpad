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

using System.Buffers.Binary;

namespace GeneralUtilities;

/// <summary>
/// Represents a utility class for calculating Fletcher checksums.
/// </summary>
public class FletcherCheckSum
{
    /// <summary>
    /// Represents the sizes of the Fletcher checksum types.
    /// </summary>
    public readonly struct FletcherSizes
    {
        /// <summary>
        /// Represents the type of Fletcher checksum.
        /// </summary>
        private FletcherTypes FletcherType { get; }

        /// <summary>
        /// Represents the available types of Fletcher checksums.
        /// </summary>
        public enum FletcherTypes
        {
            /// <summary>
            /// Represents the Fletcher16 member of the FletcherTypes enum in the GeneralUtilities namespace.
            /// </summary>
            Fletcher16 = 16,

            /// <summary>
            /// Represents the Fletcher32 member of the FletcherTypes enumeration.
            /// </summary>
            Fletcher32 = 32,

            /// <summary>
            /// Represents the Fletcher64 member of the FletcherTypes enum.
            /// </summary>
            Fletcher64 = 64
        }
        public FletcherSizes(FletcherTypes fletcherType)
        {
            FletcherType = fletcherType;
        }
        // Converting method to property
        /// <summary>
        /// Gets the size of each block used for calculating the checksum.
        /// </summary>
        /// <remarks>
        /// The block size is determined by the Fletcher type set in the <see cref="FletcherSizes"/> struct.
        /// </remarks>
        /// <returns>
        /// The size of each block, in bytes.
        /// </returns>
        public int BlockSize => (int)FletcherType / 16;
    }
    
    /// <summary>
    /// Calculates the checksum from the given byte array using the specified number of bits per block.
    /// </summary>
    /// <param name="inputAsBytes">The byte array to calculate the checksum from.</param>
    /// <param name="bitsPerBlock">The number of bits per block used in the Fletcher checksum algorithm.</param>
    /// <returns>The calculated checksum as an unsigned 64-bit integer.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input byte array is null.</exception
    public static UInt64 CalculateChecksumFromBytes(byte[] inputAsBytes, FletcherSizes.FletcherTypes bitsPerBlock)
    {
        if (inputAsBytes == null)
        {
            throw new ArgumentNullException(nameof(inputAsBytes),
                "The input for calculating the checksum may nog be null.");
        }

        var fletcherType = new FletcherSizes(bitsPerBlock);
        
        UInt64 maxBlockValue = (UInt64)(Math.Pow(2, 8 * fletcherType.BlockSize) - 1);
        UInt64 sum1 = 0;
        UInt64 sum2 = 0;
        try
        {
            foreach (UInt64 block in ChunkIntoBlocks(inputAsBytes, fletcherType))
            {
                sum1 = checked((sum1 + block) % maxBlockValue);
                sum2 = checked((sum2 + sum1) % maxBlockValue);
            }
            return checked(sum1 + (sum2 * (maxBlockValue + 1)));
        }
        catch (OverflowException exception)
        {
            throw;
        }
        
        
    }

    /// <summary>
    /// Splits the given byte array into blocks of a specified size.
    /// </summary>
    /// <param name="inputAsBytes">The byte array to be split into blocks.</param>
    /// <param name="fletcherType">The type of Fletcher checksum used to calculate the checksum.</param>
    /// <returns>An IEnumerable collection of UInt64 representing the calculated blocks.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input array is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the fletcherType is invalid.</exception>
    private static IEnumerable<UInt64> ChunkIntoBlocks(byte[] inputAsBytes, FletcherSizes fletcherType)
    {
        if (inputAsBytes == null)
        {
            throw new ArgumentNullException(nameof(inputAsBytes),
                "The input for calculating the checksum may nog be null.");
        }
        
        var blockSize = fletcherType.BlockSize;
        if (blockSize != (int)FletcherSizes.FletcherTypes.Fletcher16 &&
            blockSize != (int)FletcherSizes.FletcherTypes.Fletcher32 &&
            blockSize != (int)FletcherSizes.FletcherTypes.Fletcher64)
        {
            throw new ArgumentOutOfRangeException(nameof(fletcherType));
        }
        
        // Padding byte array to be divisible by blockSize with 0s
        int length = ((inputAsBytes.Length + blockSize - 1) / blockSize) * blockSize;
        byte[] bytes = new byte[length];
        inputAsBytes.CopyTo(bytes, 0);
        Array.Clear(bytes, inputAsBytes.Length, length - inputAsBytes.Length);

        for (int i = 0; i < length; i += blockSize)
        {
            yield return CalculateBlockFromBytes(bytes, i, blockSize);
        }
    }

    /// <summary>
    /// Calculates a block of data from a byte array.
    /// </summary>
    /// <param name="bytes">The byte array containing the data.</param>
    /// <param name="index">The starting index of the block.</param>
    /// <param name="blockSize">The size of the block.</param>
    /// <returns>The calculated block as an unsigned 64-bit integer.</returns>
    private static UInt64 CalculateBlockFromBytes(byte[] bytes, int index, int blockSize)
    {
        if (blockSize == (int)FletcherSizes.FletcherTypes.Fletcher16)
            return BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(index));
        if (blockSize == (int)FletcherSizes.FletcherTypes.Fletcher32)
            return BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(index));
        if (blockSize == (int)FletcherSizes.FletcherTypes.Fletcher64)
            return BinaryPrimitives.ReadUInt64LittleEndian(bytes.AsSpan(index));

        return 0;
    }


}