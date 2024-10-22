/*
 * Copyright (c) 2024 - 2024 Marcel Adriani
 *
 * This file is part of gx000touchpad.
 *
 * gx000touchpad is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
 *
 * gx000touchpad is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with Foobar. If not, see <https://www.gnu.org/licenses/>.
 */
using Stateless;
namespace gx000data
{
    public abstract class Variable
    {
        public enum DataStatus
        {
            Synchronized = 0,
            FromSimToClient = 1,
            FromSimToClientInProgress = 3,
            FromClientToSim = 2,
            FromClientToSimInProgress = 4,
            FailedCommunication = 5,
            StatusNotSet = 99,
            Test = -1
        }

        public enum Triggers
        {
            SimSendsUpdate = 0,
            ClientSendsUpdate = 1,
            ClientAcknowledged = 2,
            SimAcknowledged = 3,
            SimUpdateFailed = 4,
            ClientUpdateFailed = 5,
            NoAction = 9
        }

        /// <summary>
        /// Represents a variable with a specific data type.
        /// </summary>
        public string VariableName { get; private set; }

        private DataStatusStateMachine _stateMachine;
        public Triggers Trigger { get; set; }
        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        protected Variable(string variableName)
        {
            VariableName = variableName;
            _stateMachine = new DataStatusStateMachine(VariableName);
            Trigger = Triggers.NoAction;
        }

        public virtual byte[] GetValueBytes()
        {
            return new byte[0];
        }

        public bool StoreToDataIsOk()
        {
            return _stateMachine.CanFire(Trigger);
        }

        public DataStatus ChangeStatus()
        {
            _stateMachine.Fire(Trigger);
            return _stateMachine.State;
        }

        protected virtual void OnStatusChanged()
        {
            StatusChanged?.Invoke(this, new StatusChangedEventArgs { variable = this });
        }

        /// <summary>
        /// Represents a state machine for handling data status transitions.
        /// </summary>
        private class DataStatusStateMachine : StateMachine<DataStatus, Triggers>
        {
            /// <summary>
            /// Stores the valid triggers for each DataStatus in the DataStatusStateMachine.
            /// </summary>
            private readonly Dictionary<DataStatus, List<Triggers>> _triggers;

            /// <summary>
            /// Represents a state machine for managing the data status transitions in the system.
            /// </summary>
            /// <remarks>
            /// This state machine handles various status transitions such as synchronization,
            /// data updates from simulation to client, and data updates from client to simulation,
            /// along with their progress and failure states.
            /// </remarks>
            public DataStatusStateMachine(string variableName) : base(DataStatus.StatusNotSet)
            {
                Configure(DataStatus.Synchronized)
                    .Permit(Triggers.SimSendsUpdate, DataStatus.FromSimToClientInProgress)
                    .Permit(Triggers.ClientSendsUpdate, DataStatus.FromClientToSimInProgress);
                Configure(DataStatus.FromSimToClientInProgress)
                    .Permit(Triggers.ClientAcknowledged, DataStatus.Synchronized)
                    .Permit(Triggers.ClientUpdateFailed, DataStatus.FailedCommunication);
                Configure(DataStatus.FromClientToSim)
                    .PermitIf(Triggers.SimSendsUpdate, DataStatus.FromSimToClientInProgress,
                        () => !VariableDefinitions.FindVariableAttributes(variableName).UserIsBoss)
                    .Permit(Triggers.ClientSendsUpdate, DataStatus.FromClientToSimInProgress);
                Configure(DataStatus.FromClientToSimInProgress)
                    .Permit(Triggers.SimAcknowledged, DataStatus.Synchronized)
                    .Permit(Triggers.SimUpdateFailed, DataStatus.FailedCommunication);
                Configure(DataStatus.FromSimToClient)
                    .Permit(Triggers.SimSendsUpdate, DataStatus.FromSimToClientInProgress)
                    .PermitIf(Triggers.ClientSendsUpdate, DataStatus.FromSimToClientInProgress,
                        () => VariableDefinitions.FindVariableAttributes(variableName).UserIsBoss);
                Configure(DataStatus.StatusNotSet)
                    .Permit(Triggers.SimSendsUpdate, DataStatus.FromSimToClient)
                    .Permit(Triggers.ClientSendsUpdate, DataStatus.FromClientToSim);

                _triggers = new Dictionary<DataStatus, List<Triggers>>();
                AddTrigger(DataStatus.Synchronized, Triggers.SimSendsUpdate, Triggers.ClientSendsUpdate);
                AddTrigger(DataStatus.FromClientToSim, Triggers.SimSendsUpdate, Triggers.ClientSendsUpdate);
                AddTrigger(DataStatus.FromSimToClient, Triggers.SimSendsUpdate, Triggers.ClientSendsUpdate);
                AddTrigger(DataStatus.StatusNotSet, Triggers.SimSendsUpdate, Triggers.ClientSendsUpdate);
                AddTrigger(DataStatus.FromSimToClientInProgress, Triggers.ClientAcknowledged, Triggers.ClientUpdateFailed);
                AddTrigger(DataStatus.FromClientToSimInProgress, Triggers.SimAcknowledged, Triggers.SimUpdateFailed);
            }

            /// <summary>
            /// Adds the provided triggers to the specified data status.
            /// </summary>
            /// <param name="status">The data status to which the triggers are to be added.</param>
            /// <param name="triggers">The triggers to be added to the specified data status.</param>
            private void AddTrigger(DataStatus status, params Triggers[] triggers)
            {
                if (!_triggers.ContainsKey(status))
                {
                    _triggers[status] = new List<Triggers>();
                }
                foreach (var trigger in triggers)
                {
                    _triggers[status].Add(trigger);
                }
            }

            /// <summary>
            /// Retrieves the list of triggers available for the current state of the DataStatusStateMachine.
            /// </summary>
            /// <returns>A read-only collection of triggers that can be applied in the current state,
            /// or null if no triggers are available for the current state.</returns>
            public IReadOnlyCollection<Triggers> GetTriggers()
            {
                if (_triggers.ContainsKey(State))
                {
                    return _triggers[State];
                }
                return null;
            }
        }
    }
}