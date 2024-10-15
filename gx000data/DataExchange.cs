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

public class DataExchange
{
    public enum DataStatus
    {
        Synchronized = 0,
        FromSimToClient = 1,
        FromClientToSim = 2,
        Test = -1
    }

    /// <summary>
    /// Determines if a change in data status is permissible based on the current and new status values.
    /// </summary>
    /// <param name="existingStatus">The current status of the data.</param>
    /// <param name="newStatus">The new status to which the data is proposed to change.</param>
    /// <returns>True if the status change is allowed, false otherwise.</returns>
    public static bool IsStatusChangeAllowed(DataStatus existingStatus, DataStatus newStatus)
    {
        if (IsNotTestStatus(newStatus))
        {
            bool isSameStatus = existingStatus == newStatus;
            bool isExistingSynchronized = existingStatus == DataStatus.Synchronized;
            return isSameStatus || isExistingSynchronized;
        }
        return false;
    }

    /// <summary>
    /// Determines if storing the new variable to the data store is permissible based on the existing variable
    /// and the new variable's statuses and values.
    /// </summary>
    /// <param name="existingVariable">The current variable in the data store.</param>
    /// <param name="newVariable">The new variable proposed to be stored.</param>
    /// <returns>True if the storage operation is allowed, false otherwise.</returns>
    public static bool StoreToDataStoreIsOk(Variable existingVariable, Variable newVariable)
    {
        ArgumentNullException.ThrowIfNull(existingVariable);
        ArgumentNullException.ThrowIfNull(newVariable);

        if (existingVariable.VariableName != newVariable.VariableName)
        {
            throw new ArgumentException("Variable name mismatch");
        }

        var variableAttributes = VariableDefinitions.FindVariableAttributes(newVariable.VariableName);

        if (AreValuesEqual(existingVariable, newVariable))
        {
            return false;
        }

        if (IsStatusChangeAllowed(existingVariable.Status, newVariable.Status))
        {
            return true;
        }

        return IsSpecialCase(existingVariable, newVariable, variableAttributes);
    }

    private static bool AreValuesEqual(Variable existingVariable, Variable newVariable)
    {
        return existingVariable.GetValueBytes().SequenceEqual(newVariable.GetValueBytes());
    }

    private static bool IsSpecialCase(
        Variable existingVariable, 
        Variable newVariable, 
        IVariableAttributes variableAttributes)
    {
        var statusFromClientToSim = existingVariable.Status == DataStatus.FromClientToSim;
        var statusFromSimToClient = existingVariable.Status == DataStatus.FromSimToClient;

        if ((statusFromClientToSim && existingVariable.Status != newVariable.Status && !variableAttributes.UserIsBoss) ||
            (statusFromSimToClient && existingVariable.Status != newVariable.Status && variableAttributes.UserIsBoss))
        {
            return true;
        }

        return false;
    }
    
    private static bool IsNotTestStatus(DataStatus status)
    {
        return status != DataStatus.Test;
    }

}