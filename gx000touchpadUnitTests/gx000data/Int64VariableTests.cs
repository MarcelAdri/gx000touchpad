﻿// //     * Copyright (c) 2024 - 2024 Marcel Adriani
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
        _variable = new TestableInt64Variable(
            VariableDefinitions.FirstLongName,
            DataExchange.DataStatus.Synchronized,
            _testValue);
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
        _variable.Value = 84L;
        var actualValue = _variable.Value;

        // Assert
        Assert.That(actualValue == 84L);
    }
    
    [Test]
    public void VariableStatusSetter_StatusChangeIsOkIsFalse_ShouldThrowInvalidOperationException()
    {
        Assert.That(() => _variable.Status = DataExchange.DataStatus.Test, 
            Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void VariableStatusSetter_ChangeIsValid_NewStatusIsSet()
    {
        _variable.Status = DataExchange.DataStatus.FromClientToSim;
        
        Assert.That(_variable.Status == DataExchange.DataStatus.FromClientToSim);
    }
    
    [Test]
    public void VariableStatusSetter_ChangeIsValid_OnStatusChangedIsCalled()
    {
        _variable.Status = DataExchange.DataStatus.FromClientToSim;
        
        Assert.IsTrue(_variable.OnStatusChangedCalled);
    }


}