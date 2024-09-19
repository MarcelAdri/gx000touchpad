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
/// Represents the available types for the data.
/// </summary>
public enum AvailableTypes
{
    /// <summary>
    /// Represents the string data type.
    /// </summary>
    /// <remarks>
    /// This enum member is used to indicate that a variable is of type string.
    /// It is used in the Variables class to validate and assign string values to variables.
    /// </remarks>
    StringType = 0,

    /// <summary>
    /// Represents the integer data type.
    /// </summary>
    /// <remarks>
    /// This enum member is used to indicate that a variable is of type int.
    /// It is used in the Variables class to validate and assign int values to variables.
    /// </remarks>
    IntType = 1,

    /// <summary>
    /// Represents the long data type.
    /// </summary>
    /// <remarks>
    /// This enum member is used to indicate that a variable is of type long.
    /// It is used in the Variables class to validate and assign long values to variables.
    /// </remarks>
    LongType = 2
}