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
/// Converts <see cref="Int64"/> data to and from bytes.
/// </summary>
public class Int64DataConverter : IDataConverter<long>
{
    /// <summary>
    /// Converts the provided data value to an array of bytes.
    /// </summary>
    /// <param name="dataValue">The data value to convert.</param>
    /// <returns>An array of bytes representing the converted data value.</returns>
    public byte[] ToBytes(long dataValue)
    {
        return GeneralUtilities.GeneralUtilities.StoreLittleEndian(BitConverter.GetBytes(dataValue));
    }

    /// <summary>
    /// Converts a byte array back into the original data value of type long.
    /// </summary>
    /// <param name="bytes">The byte array containing the data value to convert.</param>
    /// <returns>The original data value of type int.</returns>
    public long FromBytes(byte[] bytes)
    {
        return BitConverter.ToInt64(GeneralUtilities.GeneralUtilities.StoreLittleEndian(bytes));
    }
}