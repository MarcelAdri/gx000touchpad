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

namespace GeneralUtilities;

/// <summary>
/// Represents a utility class for calculating Fletcher checksums.
/// </summary>
public static class FletcherCheckSum
{
    /// <summary>
    /// Represents the types of Fletcher checksums that can be calculated.
    /// </summary>
    public enum FletcherTypes
    {
        Fletcher16 = 16,
        Fletcher32 = 32,
        Fletcher64 = 64
    }

    /// <summary>
    /// Calculates the Fletcher checksum from an array of bytes.
    /// </summary>
    /// <param name="input">The array of bytes for which the checksum will be calculated.</param>
    /// <param name="bitsPerBlock">A member of enum FletcherTypes, representing the type of Fletcher calculation</param>
    /// <returns>The calculated Fletcher checksum as an unsigned 64-bit integer.</returns>
    /// <exception cref="ArgumentNullException">Thrown when input is null</exception>
    public static UInt64 CalculateChecksumFromBytes(IReadOnlyList<byte> input, FletcherTypes bitsPerBlock)
    {
        if (input == null)
            throw new ArgumentNullException(nameof(input), "Array for calculating checksum may not be null,");
            
        var bytesPerCycle = (int) bitsPerBlock / 16;
        var modValue = (ulong) (Math.Pow(2, 8 * bytesPerCycle) - 1);

        ulong sum1 = 0;
        ulong sum2 = 0;

        foreach (var block in Blockify(input, bytesPerCycle))
        {
            sum1 = (sum1 + block) % modValue;
            sum2 = (sum2 + sum1) % modValue;
        }

        return sum1 + sum2 * (modValue + 1);
    }

    /// <summary>
    /// Splits the given input byte array into blocks of the specified size and calculates the Fletcher checksum.
    /// </summary>
    /// <param name="inputAsBytes">The input byte array.</param>
    /// <param name="blockSize">The number of blocks (1, 2 or 4) in a group</param>
    /// <returns>An IEnumerable&lt;ulong&gt; of the carved out blocks</returns>
    private static IEnumerable<ulong> Blockify(IReadOnlyList<byte> inputAsBytes, int blockSize)
    {
        var i = 0;
        ulong block = 0;

        while (i < inputAsBytes.Count)
        {
            block = (block << 8) | inputAsBytes[i];
            i++;

            if (i % blockSize != 0 && i != inputAsBytes.Count) continue;

            yield return block;
            block = 0;
        }
    }

}