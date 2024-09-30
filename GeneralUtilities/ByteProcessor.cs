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
/// Provides general utility methods.
/// </summary>
public class ByteProcessor
{
    private readonly IEndianessChecker _checker;

    public ByteProcessor(IEndianessChecker checker)
    {
        _checker = checker;
    }
    /// <summary>
    /// Stores the given byte array in little-endian order, if necessary.
    /// </summary>
    /// <param name="arr">The byte array to be stored.</param>
    /// <returns>A new byte array stored in little-endian order, if necessary.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input array is null.</exception>
    /// /// <exception cref="ArgumentOutOfRangeException">Thrown when the input array is empty.</exception>
    public byte[] StoreLittleEndian(IReadOnlyCollection<byte> arr)
    {
        if (arr == null)
            throw new ArgumentNullException(nameof(arr), "The array to be stored may not be null.");
        
        if (arr.Count == 0)
            throw new ArgumentOutOfRangeException(nameof(arr), "The array to be stored may not be empty.");
        

        byte[] bytesToChange = arr.ToArray();

        if (!_checker.IsLittleEndian())
            Array.Reverse(bytesToChange);
        
        return bytesToChange;
    }
}