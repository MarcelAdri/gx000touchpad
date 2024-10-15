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
public class DataExchangeTests
{
    private Int32Variable _variable1;
    private int _testValue;
    private Int32Variable _variable2;
    private long _longTestValue;
    private Int64Variable _longVariable1;
    private Int64Variable _longVariable2;

    [SetUp]
    public void SetUp()
    {
        _testValue = 10;
        _variable1 = new Int32Variable(
            VariableDefinitions.FirstNumberName, DataExchange.DataStatus.Synchronized, _testValue);
        
        _variable2 = new Int32Variable(
            VariableDefinitions.FirstNumberName, DataExchange.DataStatus.Synchronized, _testValue);

        _longTestValue = 1000000000L;
        _longVariable1 = new Int64Variable(
            VariableDefinitions.FirstLongName, DataExchange.DataStatus.Synchronized, _longTestValue);
        _longVariable2 = new Int64Variable(
            VariableDefinitions.FirstLongName, DataExchange.DataStatus.Synchronized, _longTestValue);
    }
    [Test]
    [TestCase(DataExchange.DataStatus.FromClientToSim)]
    [TestCase(DataExchange.DataStatus.FromSimToClient)]
    public void IsStatusChangeAllowed_StatusesAreEqual_ReturnsTrue(DataExchange.DataStatus status)
    {
        Assert.That(DataExchange.IsStatusChangeAllowed(status, status));
    }
    
    [Test]
    public void IsStatusChangeAllowed_StatusesAreNotEqualButExistingStatusIsSynchronized_ReturnsTrue()
    {
        Assert.That(DataExchange.IsStatusChangeAllowed(DataExchange.DataStatus.Synchronized, 
            DataExchange.DataStatus.FromClientToSim));
    }
    
    [Test]
    public void IsStatusChangeAllowed_StatusIsTest_ReturnsFalse()
    {
        Assert.That(!DataExchange.IsStatusChangeAllowed(DataExchange.DataStatus.Synchronized, 
            DataExchange.DataStatus.Test));
    }
    
    [Test]
    public void IsStatusChangeAllowed_StatusesAreNotEqual_ReturnsFalse()
    {
        Assert.That(!DataExchange.IsStatusChangeAllowed(DataExchange.DataStatus.FromClientToSim, 
            DataExchange.DataStatus.FromSimToClient));
    }
    
    [Test]
    public void StoreToDataStoreIsOk_ValuesAreEqual_ReturnsFalse()
    {
        _variable1.Status = DataExchange.DataStatus.FromClientToSim;
        _variable2.Status = DataExchange.DataStatus.FromSimToClient;
        
        Assert.That(!DataExchange.StoreToDataStoreIsOk(_variable1, _variable2));
    }
    
    [Test]
    public void StoreToDataStoreIsOk_StatusChangeIsOk_ReturnsTrue()
    {
        _variable1.Value += 1;
        
        Assert.That(DataExchange.StoreToDataStoreIsOk(_variable1, _variable2));
    }
    
    [Test]
    public void StoreToDataStoreIsOk_UserNoPriorityAnOldValueFromClient_ReturnsTrue()
    {
        _variable1.Value += 1;
        
        _variable1.Status = DataExchange.DataStatus.FromClientToSim;
        _variable2.Status = DataExchange.DataStatus.FromSimToClient;
        
        Assert.That(DataExchange.StoreToDataStoreIsOk(_variable1, _variable2));
    }
    
    [Test]
    public void StoreToDataStoreIsOk_UserNoPriorityAnOldValueFromSim_ReturnsFalse()
    {
        _variable1.Value += 1;
        
        _variable1.Status = DataExchange.DataStatus.FromSimToClient;
        _variable2.Status = DataExchange.DataStatus.FromClientToSim;
        
        Assert.That(!DataExchange.StoreToDataStoreIsOk(_variable1, _variable2));
    }
    
    [Test]
    public void StoreToDataStoreIsOk_UserPriorityAnOldValueFromSim_ReturnsTrue()
    {
        _longVariable1.Value += 1L;
        
        _longVariable1.Status = DataExchange.DataStatus.FromSimToClient;
        _longVariable2.Status = DataExchange.DataStatus.FromClientToSim;
        
        Assert.That(DataExchange.StoreToDataStoreIsOk(_longVariable1, _longVariable2));
    }
    
    [Test]
    public void StoreToDataStoreIsOk_UserPriorityAnOldValueFromClient_ReturnsFalse()
    {
        _longVariable1.Value += 1L;
        
        _longVariable1.Status = DataExchange.DataStatus.FromClientToSim;
        _longVariable2.Status = DataExchange.DataStatus.FromSimToClient;
        
        Assert.That(!DataExchange.StoreToDataStoreIsOk(_longVariable1, _longVariable2));
    }
    
    [Test]
    public void StoreToDataStoreIsOk_NullParameter1_ThrowArgumentNullException()
    {
        Assert.That(() => DataExchange.StoreToDataStoreIsOk(null, _variable1), 
            Throws.TypeOf<ArgumentNullException>());
    }
    
    [Test]
    public void StoreToDataStoreIsOk_NullParameter2_ThrowArgumentNullException()
    {
        Assert.That(() => DataExchange.StoreToDataStoreIsOk(_variable1, null), 
            Throws.TypeOf<ArgumentNullException>());
    }
    
    [Test]
    public void StoreToDataStoreIsOk_DifferentVariables_ThrowArgumentException()
    {
        Assert.That(() => DataExchange.StoreToDataStoreIsOk(_variable1, _longVariable1), 
            Throws.TypeOf<ArgumentException>());
    }
}