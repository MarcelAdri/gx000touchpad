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

using System.ComponentModel.DataAnnotations;
using gx000data;

namespace gx000touchpadUnitTests.gx000data;

[TestFixture]
public class VariableDefinitionsTests
{
    [Test]
    public void GetAllVariables_WhenCalled_ContainTheRightNumberOfVariables()
    {
        var allVariables = VariableDefinitions.GetAllVariables();
        
        Assert.That(allVariables.Count, Is.EqualTo(VariableDefinitions.NumberOfVariables));
    }
    
    [Test]
    public void GetAllVariables_WhenCalled_ContainFirstMessage()
    {
        var allVariables = VariableDefinitions.GetAllVariables();
        
        Assert.That(allVariables.ContainsKey(VariableDefinitions.FirstMessageName));
    }

    [Test]
    public void SizeMatters_WhenCalledWithStringType_ReturnsTrue()
    {
        Assert.That(VariableDefinitions.SizeMatters(DataTypes.StringType));
    }
    
    [Test]
    public void SizeMatters_WhenCalledWithNumericType_ReturnsFalse()
    {
        Assert.That(!VariableDefinitions.SizeMatters(DataTypes.IntType));
    }
    
    [Test]
    public void SizeMatters_WhenCalledWithUnknownType_ThrowsInvalidOperationException()
    {
        Assert.That(() => VariableDefinitions.SizeMatters("AnyType"), Throws.InvalidOperationException);
    }
    
    [Test]
    public void FindVariableAttributes_WhenCalledWithValidName_ReturnAttributes()
    {
        var result = VariableDefinitions.FindVariableAttributes(VariableDefinitions.FirstMessageName);
        
        Assert.That(result.VariableName, Is.EqualTo(VariableDefinitions.FirstMessageName));
    }
    
    [Test]
    public void FindVariableAttributes_WhenCalledWithInvalidName_ThrowKeyNotFoundException()
    {
        Assert.That(() => VariableDefinitions.FindVariableAttributes("AnyName"), 
            Throws.TypeOf<KeyNotFoundException>());
    }
    
    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void FindVariableAttributes_WhenCalledEmptyName_ThrowArgumentNullException(string? name)
    {
        Assert.That(() => VariableDefinitions.FindVariableAttributes(name), 
            Throws.ArgumentNullException);
    }
}