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
/// Represents a variable in the data exchange system.
/// </summary>
/// <typeparam name="T">The type of the variable.</typeparam>
public interface IVariable<T>
{
    /// <summary>
    /// Represents an interface for variables in the gx000data namespace.
    /// </summary>
    /// <typeparam name="T">The type of the variable.</typeparam>
    string VariableName { get; }

    /// <summary>
    /// Represents the value of a variable.
    /// </summary>
    /// <typeparam name="T">The type of the variable value.</typeparam>
    /// <returns>The current value of the variable.</returns>
    /// <seealso cref="IVariable{T}"/>
    /// <seealso cref="DataExchange.DataStatus"/>
    /// <example>
    /// The following example demonstrates how to use the Value property.
    /// <code>
    /// IVariable<see cref="int"/> variable = new MyVariable();
    /// variable.Value = 10;
    /// Console.WriteLine(variable.Value); // Output: 10
    /// </code>
    /// </example>
    T Value { get; set; }
}