﻿// /*
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

using System.Collections.Concurrent;
using System.ComponentModel;

namespace gx000data;

/// <summary>
/// Represents a data store that stores DataVariables.
/// </summary>
public class DataStore : IDataStore, INotifyPropertyChanged
{
    /// <summary>
    /// Represents a store room for data variables.
    /// </summary>
    private readonly ConcurrentDictionary<string, Variable> _storeRoom = new();
    private readonly ConcurrentDictionary<string, Variable> _queue = new();
    
    private Variable _currentVariable;
    public Variable CurrentVariable
    {
        get => _currentVariable;
        set
        {
            _currentVariable = value;
            OnPropertyChanged();
        }
    }

    private readonly object _lock = new();

    public event PropertyChangedEventHandler? PropertyChanged;
    
    /// <summary>
    /// Adds or updates a data variable in the store.
    /// </summary>
    /// <remarks>
    /// This method adds or updates the given data variable in the store.
    /// If the variable is null, an <see cref="ArgumentNullException"/> is thrown.
    /// </remarks>
    /// <param name="variable">The data variable to be stored.</param>
    public void Store(Variable variable)
    {
        ArgumentNullException.ThrowIfNull(variable);
        
        AddOrUpdateVariable(variable);
    }

    /// <summary>
    /// Tries to retrieve a data variable from the store based on the provided variable name.
    /// </summary>
    /// <param name="variableName">The name of the variable to be retrieved.</param>
    /// <param name="variable">The data variable with the provided name, if found; otherwise, null.</param>
    /// <returns>True if the data variable is found in the store; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="variableName"/> is null or whitespace.</exception>
    public bool TryGetDataFromStore(string variableName, out Variable? variable)
    {
        if (string.IsNullOrWhiteSpace(variableName))
            throw new ArgumentException("Name cannot be null or whitespace.");
    
        if (_storeRoom.TryGetValue(variableName, out Variable? data))
        {
            variable = data;
            CurrentVariable = data;
            return true;
        }

        variable = null;
        return false;
    }

    /// <summary>
    /// Adds or updates a variable in the data store.
    /// </summary>
    /// <param name="variable">The variable to add or update.</param>
    private void AddOrUpdateVariable(Variable variable)
    {
        
        
        
        _storeRoom.AddOrUpdate(variable.Name, variable, (key, oldValue) =>
        {
            if (variable.CanStoreData())
            {
                return variable;
            }
            _queue.TryAdd(key, variable); 
            variable.StatusChanged += OnVariableStatusChanged;

            return oldValue;
        });
    }

    /// <summary>
    /// Event handler for the "StatusChanged" event of the Variable class.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event arguments containing the changed variable.</param>
    private void OnVariableStatusChanged(object sender, StatusChangedEventArgs e)
    {
        Variable variable = e.variable;

        lock (_lock)
        {
            if (_queue.TryRemove(variable.Name, out var foundVariable))
            {
                variable.StatusChanged -= OnVariableStatusChanged;
            }    
        }
        AddOrUpdateVariable(variable);
    }
    
    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">
    /// The name of the property that changed. This is optional and can be supplied automatically
    /// by the CallerMemberName attribute.
    /// </param>
    private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    
}
