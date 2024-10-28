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

using gx000data;
namespace gx000touchpadUnitTests.gx000data;

[TestFixture]
public class Int64VariableTests
{
    private Int64DataConverter _converter;
    private TestableInt64Variable _variable;
    private long _testValue;

    [SetUp]
    public void SetUp()
    {
        _testValue = 1000000000L;
        _converter = new Int64DataConverter();
        _variable = new TestableInt64Variable(VariableDefinitions.FirstLongName, _testValue);
    }

    [Test]
    public void VariableNameGetter_ShouldReturnCorrectName()
    {
        // Act
        var actualName = _variable.Name;
        
        // Assert
        Assert.That(actualName, Is.EqualTo(VariableDefinitions.FirstLongName));
    }

    [Test]
    public void VariableValueGetter_ShouldReturnCorrectValue()
    {
        // Act
        var actualValue = _variable.Value;

        // Assert
        Assert.That(actualValue, Is.EqualTo(_testValue));
    }

    [Test]
    public void VariableValueSetter_ShouldReturnUpdatedValue()
    {
        // Act
        _variable.Value = 123456789L;
        var actualValue = _variable.Value;

        // Assert
        Assert.That(actualValue, Is.EqualTo(123456789L));
    }

    [Test]
    public void StoreToDataIsOk_WhenStorageIsOk_ShouldReturnTrue()
    {
        // Arrange
        _variable.SetCurrentTrigger(Variable.Triggers.ClientSendsUpdate);
        _variable.ChangeStateWithTrigger(Variable.Triggers.ClientSendsUpdate);

        // Assert
        Assert.That(_variable.CanStoreData(), Is.True);
    }

    [Test]
    public void StoreToDataIsOk_WhenStorageIsNotOk_ShouldReturnFalse()
    {
        // Act & Assert
        Assert.That(_variable.CanStoreData(), Is.False);
    }

    [Test]
    [TestCase(Variable.DataStatus.Synchronized, Variable.Triggers.ClientSendsUpdate, Variable.Triggers.SimSendsUpdate, 2)]
    [TestCase(Variable.DataStatus.FromSimToClientInProgress, Variable.Triggers.ClientAcknowledged, Variable.Triggers.ClientUpdateFailed, 2)]
    [TestCase(Variable.DataStatus.FromClientToSim, Variable.Triggers.ClientSendsUpdate, Variable.Triggers.ClientSendsUpdate, 1)]
    [TestCase(Variable.DataStatus.FromClientToSimInProgress, Variable.Triggers.SimAcknowledged, Variable.Triggers.SimUpdateFailed, 2)]
    [TestCase(Variable.DataStatus.FromSimToClient, Variable.Triggers.SimSendsUpdate, Variable.Triggers.ClientSendsUpdate, 2)]
    [TestCase(Variable.DataStatus.StatusNotSet, Variable.Triggers.ClientSendsUpdate, Variable.Triggers.SimSendsUpdate, 2)]
    public void GetTriggers_ShouldReturnCorrectListOfPossibleTriggers(
        Variable.DataStatus dataStatus, 
        Variable.Triggers expectedTrigger1,
        Variable.Triggers expectedTrigger2,
        int expectedCount)
    {
        // Arrange
        SetDataStatus(dataStatus);

        // Act
        var actualTriggers = _variable.GetAvailableTriggers();
        var isBoss = VariableDefinitions.FindVariableAttributes(_variable.Name).UserIsBoss;

        // Assert
        Assert.That(actualTriggers.Count, Is.EqualTo(expectedCount), "Actual number of triggers was not correct");
        Assert.That(actualTriggers.Contains(expectedTrigger1), Is.True, $"Triggers do not contain {expectedTrigger1}");
        Assert.That(actualTriggers.Contains(expectedTrigger2), Is.True, $"Triggers do not contain {expectedTrigger2}");
    }

    [Test]
    public void ChangeStatus_WithValidTrigger_ShouldChangeStatus()
    {
        // Arrange
        _variable.SetCurrentTrigger(Variable.Triggers.ClientSendsUpdate);

        // Act
        var result = _variable.ChangeStateWithTrigger(Variable.Triggers.ClientSendsUpdate);

        // Assert
        Assert.That(result, Is.EqualTo(Variable.DataStatus.FromClientToSim));
    }

    [Test]
    public void ChangeStatus_WithInvalidTrigger_ShouldThrowInvalidOperationException()
    {
        // Arrange
        _variable.SetCurrentTrigger(Variable.Triggers.ClientSendsUpdate);

        // Act & Assert
        Assert.That(() => _variable.ChangeStateWithTrigger(Variable.Triggers.NoAction), Throws.InvalidOperationException);
    }

    [Test]
    public void GetTrigger_ShouldReturnCurrentTrigger()
    {
        // Act
        var result = _variable.GetCurrentTrigger();

        // Assert
        Assert.That(result, Is.EqualTo(Variable.Triggers.NoAction));
    }

    [Test]
    public void SetTrigger_WithValidTrigger_ShouldSetTrigger()
    {
        // Act
        _variable.SetCurrentTrigger(Variable.Triggers.ClientSendsUpdate);

        // Assert
        Assert.That(_variable.GetCurrentTrigger(), Is.EqualTo(Variable.Triggers.ClientSendsUpdate));
    }

    [Test]
    public void SetTrigger_WithInvalidTrigger_ShouldThrowInvalidOperationException()
    {
        // Act & Assert
        Assert.That(() => _variable.SetCurrentTrigger(Variable.Triggers.NoAction), Throws.InvalidOperationException);
    }

    [Test]
    public void GetStatus_ShouldReturnCorrectStatus()
    {
        // Act
        var result = _variable.GetCurrentStatus();

        // Assert
        Assert.That(result, Is.EqualTo(Variable.DataStatus.StatusNotSet));
    }

    [Test]
    public void VariableStatusSetter_WithValidChange_ShouldInvokeOnStatusChanged()
    {
        // Act
        _variable.SetCurrentTrigger(Variable.Triggers.ClientSendsUpdate);
        _variable.ChangeStateWithTrigger(Variable.Triggers.ClientSendsUpdate);

        // Assert
        Assert.IsTrue(_variable.OnStatusChangedCalled);
    }

    private void SetDataStatus(Variable.DataStatus dataStatusToBeReached)
    {
        Variable.Triggers nextTrigger;

        if (dataStatusToBeReached == Variable.DataStatus.FromClientToSim ||
            dataStatusToBeReached == Variable.DataStatus.FromClientToSimInProgress ||
            dataStatusToBeReached == Variable.DataStatus.Synchronized ||
            dataStatusToBeReached == Variable.DataStatus.StatusNotSet)
        {
            nextTrigger = Variable.Triggers.ClientSendsUpdate;
        }
        else
        {
            nextTrigger = Variable.Triggers.SimSendsUpdate;
        }

        _variable.SetCurrentTrigger(nextTrigger);

        if (dataStatusToBeReached != Variable.DataStatus.StatusNotSet)
        {
            _variable.ChangeStateWithTrigger(nextTrigger);
        }

        if (dataStatusToBeReached == Variable.DataStatus.FromClientToSimInProgress || 
            dataStatusToBeReached == Variable.DataStatus.Synchronized)
        {
            _variable.ChangeStateWithTrigger(Variable.Triggers.SimAcknowledged);
        } 
        else if (dataStatusToBeReached == Variable.DataStatus.FromSimToClientInProgress) 
        {
            _variable.ChangeStateWithTrigger(Variable.Triggers.ClientAcknowledged);
        }

        if (dataStatusToBeReached == Variable.DataStatus.Synchronized)
        {
            _variable.ChangeStateWithTrigger(Variable.Triggers.ClientSendsUpdate);
        }
    }
}