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
    public string Type { get; }

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