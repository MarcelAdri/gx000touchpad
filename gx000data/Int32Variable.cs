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
/// Represents a variable of type Int32 in the data exchange system.
/// </summary>
public class Int32Variable : TypeVariable<int>, IVariable<int>
{
    /// <summary>
    /// Represents a variable of type int.
    /// </summary>
    public Int32Variable(string variableName, DataExchange.DataStatus dataStatus, int dataValue) 
        : base(variableName, dataStatus, dataValue, "IntType", new Int32DataConverter())
    {
    }
}