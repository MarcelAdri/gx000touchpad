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
/// Represents a string variable in the data exchange system.
/// </summary>
/// <remarks>
/// This class is used to store and manipulate string data in the data exchange system.
/// </remarks>
public class StringVariable : TypeVariable<string>, IVariable<string>
{
    /// <summary>
    /// Represents a string variable.
    /// </summary>
    public StringVariable(string variableName, DataExchange.DataStatus dataStatus, string dataValue) 
        : base(variableName, dataStatus, dataValue, AvailableTypes.StringType, new StringDataConverter())
    {
    }
}

