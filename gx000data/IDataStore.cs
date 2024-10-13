// //     * Copyright (c) 2024 - 2024 Marcel Adriani
// //     *
// //     * This file is part of gx000touchpad.
// //
// //      * gx000touchpad is free software: you can redistribute it and/or modify it under the terms of the
// //          GNU General Public License as published by the Free Software Foundation, either version 3 of the License,
// //          or (at your option) any later version.
// //
// //     * gx000touchpad is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
// //          without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// //          See the GNU General Public License for more details.
// //
// //     * You should have received a copy of the GNU General Public License along with Foobar.
// //          If not, see <https://www.gnu.org/licenses/>.

namespace gx000data;

public interface IDataStore
{
    /// <summary>
    /// Adds or updates a data variable in the store.
    /// </summary>
    /// <remarks>
    /// This method adds or updates the given data variable in the store.
    /// If the variable is null, an <see cref="ArgumentNullException"/> is thrown.
    /// </remarks>
    /// <param name="variable">The data variable to be stored.</param>
    void Store(Variable variable);

    /// <summary>
    /// Tries to retrieve a data variable from the store based on the provided variable name.
    /// </summary>
    /// <param name="variableName">The name of the variable to be retrieved.</param>
    /// <param name="variable">The data variable with the provided name, if found; otherwise, null.</param>
    /// <returns>True if the data variable is found in the store; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="variableName"/> is null or whitespace.</exception>
    bool TryGetDataFromStore(string variableName, out Variable? variable);
}