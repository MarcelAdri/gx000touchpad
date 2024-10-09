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
/// Provides a collection of available data types.
/// </summary>
public sealed class DataTypes
{
    private enum AvailableTypes
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
    /// <summary>
    /// ThisInstance represents the singleton instance of the DataTypes class.
    /// </summary>
    /// <remarks>
    /// ThisInstance is used to access the singleton instance of the DataTypes class,
    /// which provides access to the available types for the data.
    /// </remarks>
    private static readonly DataTypes ThisInstance = new ();
    private readonly List<string> _validTypes = Enum.GetNames(typeof(AvailableTypes)).ToList();

    /// <summary>
    /// Represents a class that provides information about available data types.
    /// </summary>
    /// <remarks>
    /// This class is meant to be used as a singleton. Use the <see cref="Instance"/> property to get the singleton instance.
    /// </remarks>
    private DataTypes()
    {
    }

    /// <summary>
    /// Represents the singleton instance of the DataTypes class.
    /// </summary>
    /// <remarks>
    /// The Instance property of the DataTypes class provides access to the singleton instance of the class.
    /// </remarks>
    public static DataTypes Instance
    {
        get
        {
            return ThisInstance;
        }
    }

    /// <summary>
    /// Represents the valid types for the data.
    /// </summary>
    /// <remarks>
    /// This property allows you to access the list of valid types for the data.
    /// </remarks>
    /// <example>
    /// The following code demonstrates how to use the ValidTypes property:
    /// <code>
    /// DataTypes dataTypes = DataTypes.Instance;
    /// IReadOnlyList&lt;string&gt; validTypes = dataTypes.ValidTypes;
    /// foreach (AvailableTypes type in validTypes)
    /// {
    /// Console.WriteLine(type);
    /// }
    /// </code>
    /// </example>
    public IReadOnlyList<string> ValidTypes
    {
        get
        {
            return _validTypes;
        }
    }

    /// <summary>
    /// Determines whether the specified type is an available data type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is available; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when type is null or empty.</exception>
    public bool IsAvailableType(string type)
    {
        if (String.IsNullOrWhiteSpace(type))
        {
            throw new ArgumentNullException("Argument 'type' may not be null.", new Exception());
        }
        
        return _validTypes.Contains(type);
    }
}