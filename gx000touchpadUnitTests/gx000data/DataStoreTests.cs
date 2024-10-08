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
using Moq;

namespace gx000touchpadUnitTests.gx000data;

[TestFixture]
public class DataStoreTests
{
    private DataStore _dataStore;
    private StringVariable _testVariable;

    [SetUp]
    public void SetUp()
    {
        _dataStore = new();
        _testVariable = new StringVariable(
            VariableDefinitions.FirstMessageName,
            DataExchange.DataStatus.Synchronized,
            "TestText");
    }

    [Test]
    public void Store_NullVariable_ThrowArgumentNullException()
    {
        Assert.That(() => _dataStore.Store(null), Throws.ArgumentNullException);
    }

    [Test]
    public void Store_ValidVariable_CallsAddOrUpdateVariable()
    {
        Variable retrievedVariable;
        
        _dataStore.Store(_testVariable);

        var success = _dataStore.TryGetDataFromStore(VariableDefinitions.FirstMessageName, out retrievedVariable);
        
        Assert.That(success, Is.True, "Expected the retrieval of the stored variable to be successful.");
        Assert.That(retrievedVariable, Is.Not.Null, "Expected a retrieved variable but got null.");
        Assert.That(_testVariable.Equals(retrievedVariable), Is.True, "Expected the stored and the retrieved variables to be equal.");

    }

    [Test]
    public void TryGetDataFromStore_ValidData_SuccessfulRetrieval()
    {
        Variable retrievedVariable;
        
        _dataStore.Store(_testVariable);

        var success = _dataStore.TryGetDataFromStore(VariableDefinitions.FirstMessageName, out retrievedVariable);
        
        Assert.That(success, Is.True, "Expected the retrieval of the stored variable to be successful.");
        Assert.That(retrievedVariable, Is.Not.Null, "Expected a retrieved variable but got null.");
        Assert.That(_testVariable.Equals(retrievedVariable), Is.True, "Expected the stored and the retrieved variables to be equal.");
    }
    
    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void TryGetDataFromStore_NullOrEmptyData_TrowArgumentException(string? testString)
    {
        Variable retrievedVariable;
        
        Assert.That(() => _dataStore.TryGetDataFromStore(testString, out retrievedVariable),
            Throws.ArgumentException);
    }
    
    [Test]
    public void TryGetDataFromStore_NoData_FailedRetrieval()
    {
        Variable retrievedVariable;
        
        var success = _dataStore.TryGetDataFromStore(VariableDefinitions.FirstMessageName, out retrievedVariable);
        
        Assert.That(success, Is.False, "Expected the retrieval of the stored variable to be successful.");
        Assert.That(retrievedVariable, Is.Null, "Expected a retrieved variable but got null.");
    }
}