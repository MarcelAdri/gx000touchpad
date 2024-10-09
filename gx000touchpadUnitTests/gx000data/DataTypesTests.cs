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
public class DataTypesTests
{
    [Test]
    public void Instance_ShouldBeSingleton()
    {
        var dataTypesInstance1 = DataTypes.Instance;
        var dataTypesInstance2 = DataTypes.Instance;

        Assert.That(dataTypesInstance2, Is.SameAs(dataTypesInstance1));
    }

    [Test]
    public void ValidTypes_ReflectsAvailableTypes()
    {
        var availableType1 = "StringType";
        var availableType2 = "IntType";
        var availableType3 = "LongType";
        var numberOfTypes = 3;
        
        Assert.That(DataTypes.Instance.ValidTypes.Count, Is.EqualTo(numberOfTypes), "Number of types is correct");
        Assert.IsTrue(DataTypes.Instance.ValidTypes.Contains(availableType1), "StringType is found");
        Assert.IsTrue(DataTypes.Instance.ValidTypes.Contains(availableType2), "IntType is found");
        Assert.IsTrue(DataTypes.Instance.ValidTypes.Contains(availableType3), "LongType is found");
        
    }

    [Test]
    public void IsAvailableType_ValidType_ReturnTrue()
    {
        var validType = "StringType";
        
        Assert.That(DataTypes.Instance.IsAvailableType(validType));
    }
    
    [Test]
    public void IsAvailableType_InvalidType_ReturnFalse()
    {
        var validType = "InvalidType";
        
        Assert.That(!DataTypes.Instance.IsAvailableType(validType));
    }
    
    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void IsAvailableType_TypeIsNullOrEmpty_ThrowArgumentnullException(string? type)
    {
        Assert.That(() => DataTypes.Instance.IsAvailableType(type), Throws.ArgumentNullException);
    }
}