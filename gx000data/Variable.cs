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

public abstract class Variable
{
    /// <summary>
    /// Represents the data status of a variable.
    /// </summary>
    private DataExchange.DataStatus _dataStatus;
    
    /// <summary>
    /// Represents a variable with a specific data type.
    /// </summary>
    public string VariableName { get; private set; }

    public event EventHandler<StatusChangedEventArgs> StatusChanged;

    /// <summary>
    /// Represents a variable with a status and value of type T.
    /// </summary>
    public DataExchange.DataStatus Status
    {
        get => _dataStatus;
        set
        {
            if (!DataExchange.IsStatusChangeAllowed(_dataStatus, value))
            {
                throw new InvalidOperationException("Change of data status refused.");
            }
            _dataStatus = value;
            OnStatusChanged();
        }
    }

    protected Variable(string variableName,
        DataExchange.DataStatus dataStatus)
    {
        VariableName = variableName;
        Status = dataStatus;
    }

    public virtual byte[] GetValueBytes()
    {
        return [];
    }

    protected virtual void OnStatusChanged()
    {
        StatusChanged?.Invoke(this, new StatusChangedEventArgs { variable = this });
    }
}