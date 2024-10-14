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

using System.Collections;
using gx000data;

namespace gx000touchpadUnitTests.gx000data;

[TestFixture]
public class StringDataConverterTests
{
    private const string InputString_1 = "Hello";
    private const string InputString_2 = "World!";
    private const string InputString_3 = "";
    
    private static readonly byte[] ResultString_1 = [0x48, 0x65, 0x6C, 0x6C, 0x6F];
    private static readonly byte[] ResultString_2= [0x57, 0x6F, 0x72, 0x6C, 0x64, 0x21];
    private static readonly byte[] ResultString_3 = [];
    
    private StringDataConverter _converter;

    [SetUp]
    public void SetUp()
    {
        _converter = new StringDataConverter();
    }
    
    [Test]
    [TestCaseSource(typeof(TestCaseDataFactory), nameof(TestCaseDataFactory.ToBytesTestCases))]
    public void ToBytes_WhenCalled_ReturnValue(string inputValue, byte[] expectedResult)
    {
        byte[] result = _converter.ToBytes(inputValue);

        Assert.That(result, Is.EqualTo(expectedResult));
    }
    
    [Test]
    [TestCaseSource(typeof(TestCaseDataFactory), nameof(TestCaseDataFactory.ToBytesTestCases))]
    public void FromBytes_WhenCalled_ReturnValue(string expectedResult, byte[] inputBytes)
    {
        string result = _converter.FromBytes(inputBytes);

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    private class TestCaseDataFactory
    {
        public static IEnumerable ToBytesTestCases
        {
            get
            {
                yield return new TestCaseData(InputString_1, 
                    ResultString_1).SetName("StringTest1");
                yield return new TestCaseData(InputString_2, 
                    ResultString_2).SetName("StringTest2");
                yield return new TestCaseData(InputString_3, 
                    ResultString_3).SetName("StringTest3");
                
                //add more test cases as necessary
            }
        }
    }
}
