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
public class Int32VariableTests
{
    private Int32DataConverter _converter;
    private TestableInt32Variable _variable;
    private int _testValue;

    [SetUp]
    public void SetUp()
    {
        _testValue = 42;
        _converter = new Int32DataConverter();
        _variable = new TestableInt32Variable(
            VariableDefinitions.FirstNumberName,
            _testValue);
    }
    
    [Test]
    public void TypeVariableNameGetter_WhenCalled_ShouldGiveName()
    {
        var actualName = _variable.VariableName;
        
        Assert.That(actualName, Is.EqualTo(VariableDefinitions.FirstNumberName));
    }
    
    [Test]
    public void TypeVariableValueGetter_WhenCalled_ShouldReturnCorrectValue()
    {
        // Act
        var actualValue = _variable.Value;

        // Assert
        Assert.That(actualValue == _testValue);
    }
    
    [Test]
    public void TypeVariableValueSetter_WhenCalled_ShouldReturnCorrectValue()
    {
        // Act
        _variable.Value = 84;
        var actualValue = _variable.Value;

        // Assert
        Assert.That(actualValue == 84);
    }

    [Test]
    public void StoreToDataIsOk_WhenStorageIsOk_ShouldReturnTrue()
    {
        _variable.SetTrigger(Variable.Triggers.ClientSendsUpdate);
        _variable.ChangeStatus(Variable.Triggers.ClientSendsUpdate);
        
        Assert.That(_variable.StoreToDataIsOk(), Is.True);
    } 
    
    [Test]
    public void StoreToDataIsOk_WhenStorageIsNotOk_ShouldReturnFalse()
    {
        Assert.That(_variable.StoreToDataIsOk(), Is.False);
    } 
    
    [Test]
    [TestCase(Variable.DataStatus.Synchronized, 
        Variable.Triggers.ClientSendsUpdate, 
        Variable.Triggers.SimSendsUpdate, 
        2)]
    [TestCase(Variable.DataStatus.FromSimToClientInProgress, 
        Variable.Triggers.ClientAcknowledged, 
        Variable.Triggers.ClientUpdateFailed, 
        2)]
    [TestCase(Variable.DataStatus.FromClientToSim, 
        Variable.Triggers.SimSendsUpdate, 
        Variable.Triggers.ClientSendsUpdate, 
        2)]
    [TestCase(Variable.DataStatus.FromClientToSimInProgress, 
        Variable.Triggers.SimAcknowledged, 
        Variable.Triggers.SimUpdateFailed, 
        2)]
    [TestCase(Variable.DataStatus.FromSimToClient, 
        Variable.Triggers.SimSendsUpdate, 
        Variable.Triggers.SimSendsUpdate, 
        1)]
    [TestCase(Variable.DataStatus.StatusNotSet, 
        Variable.Triggers.ClientSendsUpdate, 
        Variable.Triggers.SimSendsUpdate, 
        2)]
    public void GetTriggers_WhenCalled_ReturnAListOfPossibleTriggers(
        Variable.DataStatus dataStatus, 
        Variable.Triggers expectedTrigger1,
        Variable.Triggers expectedTrigger2,
        int expectedCount)
    {
        SetDataStatus(dataStatus);

        var status = _variable.GetStatus();
        
        var actualTriggers = _variable.GetTriggers();

        var isBoss = VariableDefinitions.FindVariableAttributes(_variable.VariableName).UserIsBoss;
        
        Assert.That(actualTriggers.Count, Is.EqualTo(expectedCount), 
            "Actual number of triggers was not correct");
        Assert.That(actualTriggers.Contains(expectedTrigger1), Is.True, 
            $"Triggers contains {expectedTrigger1}");
        Assert.That(actualTriggers.Contains(expectedTrigger2), Is.True, 
            $"Triggers contains {expectedTrigger2}");
    }

    [Test]
    public void ChangeStatus_WhenCalledWithValidTrigger_ShouldChangeStatus()
    {
        _variable.SetTrigger(Variable.Triggers.ClientSendsUpdate);
        
        var result =_variable.ChangeStatus(Variable.Triggers.ClientSendsUpdate);
        
        Assert.That(result, Is.EqualTo(Variable.DataStatus.FromClientToSim));
    }
    
    [Test]
    public void ChangeStatus_WhenCalledWithInvalidTrigger_ShouldThrowInvalidOperationException()
    {
        _variable.SetTrigger(Variable.Triggers.ClientSendsUpdate);
        
        Assert.That(() => _variable.ChangeStatus(Variable.Triggers.NoAction), Throws.InvalidOperationException);
    }

    [Test]
    public void GetTrigger_WhenCalled_ShouldReturnCurrentTrigger()
    {
        var result = _variable.GetTrigger();
        
        Assert.That(result, Is.EqualTo(Variable.Triggers.NoAction));
    }

    [Test]
    public void SetTrigger_WhenCalledWithPossibleTrigger_ShouldSetTrigger()
    {
        _variable.SetTrigger(Variable.Triggers.ClientSendsUpdate);
        
        Assert.That(_variable.GetTrigger(), Is.EqualTo(Variable.Triggers.ClientSendsUpdate));
    }
    
    [Test]
    public void SetTrigger_WhenCalledWithInvalidTrigger_ShouldThrowInvalidOperationException()
    {
        Assert.That(() => _variable.SetTrigger(Variable.Triggers.NoAction), Throws.InvalidOperationException);
    }

    [Test]
    public void GetStatus_WhenCalled_ShouldReturnCorrectStatus()
    {
        var result = _variable.GetStatus();
        
        Assert.That(result, Is.EqualTo(Variable.DataStatus.StatusNotSet));
    }
    
    private void SetDataStatus(Variable.DataStatus dataStatusToBeReached)
    {
        Variable.Triggers nextTrigger = default;
        if (dataStatusToBeReached == Variable.DataStatus.FromClientToSim ||
            dataStatusToBeReached == Variable.DataStatus.FromClientToSimInProgress ||
            dataStatusToBeReached == Variable.DataStatus.Synchronized ||
            dataStatusToBeReached == Variable.DataStatus.StatusNotSet)
        {
            nextTrigger = Variable.Triggers.ClientSendsUpdate;
            _variable.SetTrigger(nextTrigger);
        }
        else
        {
            nextTrigger = Variable.Triggers.SimSendsUpdate;
            _variable.SetTrigger(nextTrigger);
        }

        if (dataStatusToBeReached != Variable.DataStatus.StatusNotSet)
        {
            _variable.ChangeStatus(nextTrigger);    
        }

        if (dataStatusToBeReached == Variable.DataStatus.FromClientToSimInProgress ||
            dataStatusToBeReached == Variable.DataStatus.Synchronized)
        {
            _variable.ChangeStatus(Variable.Triggers.SimAcknowledged);
        }
        else if (dataStatusToBeReached == Variable.DataStatus.FromSimToClientInProgress)
        {
            _variable.ChangeStatus(Variable.Triggers.ClientAcknowledged);
        }

        if (dataStatusToBeReached == Variable.DataStatus.Synchronized)
        {
            _variable.ChangeStatus(Variable.Triggers.ClientSendsUpdate);
        }
    }
    
    [Test]
    public void VariableStatusSetter_ChangeIsValid_OnStatusChangedIsCalled()
    {
        _variable.SetTrigger(Variable.Triggers.ClientSendsUpdate);
        _variable.ChangeStatus(Variable.Triggers.ClientSendsUpdate);
        
        Assert.IsTrue(_variable.OnStatusChangedCalled);
    }
    
    
}