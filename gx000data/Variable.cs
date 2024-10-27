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
        /// <summary>
        /// Represents the various statuses that a data variable can have during synchronization
        /// between a simulator and a client.
        /// </summary>
        public enum DataStatus
        {
            /// <summary>
            /// Indicates that the data is synchronized between the client and the simulation.
            /// </summary>
            Synchronized = 0,

            /// <summary>
            /// Indicates that data is being transmitted from the simulation to the client.
            /// </summary>
            FromSimToClient = 1,

            /// <summary>
            /// Indicates that data is currently in the process of being transferred
            /// from the simulation to the client.
            /// </summary>
            FromSimToClientInProgress = 3,

            /// <summary>
            /// Indicates that the data is being sent from the client to the simulation.
            /// </summary>
            FromClientToSim = 2,

            /// <summary>
            /// Indicates that the data transfer from the client to the simulation is currently in progress.
            /// </summary>
            FromClientToSimInProgress = 4,

            /// <summary>
            /// Indicates that there was a communication failure between the client and the simulator.
            /// </summary>
            FailedCommunication = 5,

            /// <summary>
            /// Indicates that the status has not been set.
            /// </summary>
            StatusNotSet = 99,

            /// <summary>
            /// Represents a test status used for debugging or experimental purposes.
            /// </summary>
            Test = -1
        }

        /// <summary>
        /// Defines the possible trigger actions that can occur within the Variable state machine.
        /// </summary>
        public enum Triggers
        {
            /// <summary>
            /// Represents a trigger generated when an update is sent from the simulator to the client.
            /// </summary>
            SimSendsUpdate = 0,

            /// <summary>
            /// Indicates that the client sends an update to the server or simulation.
            /// </summary>
            ClientSendsUpdate = 1,

            /// <summary>
            /// Represents the trigger when the client has acknowledged the update sent by the simulator.
            /// </summary>
            ClientAcknowledged = 2,

            /// <summary>
            /// Indicates that the variable has been acknowledged by the simulation.
            /// </summary>
            SimAcknowledged = 3,

            /// <summary>
            /// Trigger indicating that an update from the simulator to the client has failed.
            /// </summary>
            SimUpdateFailed = 4,

            /// <summary>
            /// Represents the trigger that indicates a failed update attempt from the client side.
            /// </summary>
            ClientUpdateFailed = 5,

            /// <summary>
            /// Represents a state where no action is required or taken.
            /// This can be used as a default trigger when no specific
            /// action needs to be performed by the variable.
            /// </summary>
            NoAction = 9,
            
            Error = 99
        }

        /// <summary>
        /// Represents a variable with a specific data type.
        /// </summary>
        private DataStatusStateMachine _stateMachine;

        private Triggers _trigger;

        public string VariableName { get; }

        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        protected Variable(string variableName)
        {
            VariableName = variableName;
            _stateMachine = new DataStatusStateMachine(VariableName);
            _trigger = Triggers.NoAction;
        }

        /// <summary>
        /// Retrieves the value of the variable as a byte array.
        /// </summary>
        /// <returns>
        /// A byte array representing the value of the variable.
        /// </returns>
        public virtual byte[] GetValueBytes()
        {
            return new byte[0];
        }

        /// <summary>
        /// Determines whether storing the current variable's data is allowed based on the state machine.
        /// </summary>
        /// <returns>
        /// A boolean value indicating if the data can be stored.
        /// </returns>
        public bool StoreToDataIsOk()
        {
            return _stateMachine.CanFire(_trigger);
        }

        /// <summary>
        /// Changes the status of the variable based on the current trigger and sets the next trigger.
        /// </summary>
        /// <returns>
        /// The new status of the variable as a DataStatus enum value.
        /// </returns>
        public DataStatus ChangeStatus(Triggers nextTrigger)
        {
            _stateMachine.Fire(_trigger);
            SetTrigger(nextTrigger);
            OnStatusChanged();
            return _stateMachine.State;
        }

        /// <summary>
        /// Retrieves the collection of valid triggers for the current variable state.
        /// </summary>
        /// <returns>
        /// An IReadOnlyCollection of Triggers representing the valid state transitions for the variable.
        /// </returns>
        public IReadOnlyCollection<Triggers> GetTriggers()
        {
            return _stateMachine.GetTriggers();
        }

        /// <summary>
        /// Retrieves the current trigger action for the variable instance.
        /// </summary>
        /// <returns>
        /// The current trigger action represented as a value of the <c>Triggers</c> enum.
        /// </returns>
        public Triggers GetTrigger()
        {
            return _trigger;
        }

        /// <summary>
        /// Sets the specified trigger for the variable.
        /// </summary>
        /// <param name="trigger">
        /// The trigger to be set. Must be one of the allowed triggers for the current state.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the specified trigger is not allowed in the current state.
        /// </exception>
        public void SetTrigger(Triggers trigger)
        {
            if (GetTriggers().Contains(trigger))
            {
                _trigger = trigger;
            }
            else
            {
                throw new InvalidOperationException($"Trigger {trigger} not allowed with status {_stateMachine.State}");
            }
        }

        public DataStatus GetStatus()
        {
            return _stateMachine.State;
        }

        /// <summary>
        /// Invokes the StatusChanged event.
        /// </summary>
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

            private readonly string _variableName;

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
                _variableName = variableName;
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
                var isBoss = VariableDefinitions.FindVariableAttributes(_variableName).UserIsBoss;
                if (!_triggers.ContainsKey(status))
                {
                    _triggers[status] = new List<Triggers>();
                }
                foreach (var trigger in triggers)
                {
                    _triggers[status].Add(trigger);
                    if (status == DataStatus.FromClientToSim)
                        if (isBoss && trigger == Triggers.SimSendsUpdate)
                            _triggers[status].Remove(trigger);
                    if (status == DataStatus.FromSimToClient)
                        if (!isBoss && trigger == Triggers.ClientSendsUpdate)
                            _triggers[status].Remove(trigger);
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