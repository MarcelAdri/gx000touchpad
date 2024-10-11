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
public class Int32DataConverterTests
{
    private const Int32 Input32_0 = 0;
    private const Int32 Input32_1 = 10;
    private const Int32 Input32_2 = -10;
    private const Int32 Input32_3 = 2147483647;
    private const Int32 Input32_4 = -2147483648;
    
    private static readonly byte[] Result32_0= [0x00, 0x00, 0x00, 0x00];
    private static readonly byte[] Result32_1= [0x0A, 0x00, 0x00, 0x00];
    private static readonly byte[] Result32_2 = [0xF6, 0xFF, 0xFF, 0xFF];
    private static readonly byte[] Result32_3 = [0xFF, 0xFF, 0xFF, 0x7F];
    private static readonly byte[] Result32_4 = [0x00, 0x00, 0x00, 0x80];
    
    private Int32DataConverter _converter;

    [SetUp]
    public void SetUp()
    {
        _converter = new Int32DataConverter();
    }
    
    [Test]
    [TestCaseSource(typeof(TestCaseDataFactory), nameof(TestCaseDataFactory.ToBytesTestCases))]
    public void ToBytes_WhenCalled_ReturnValue(int inputValue, byte[] expectedResult)
    {
        byte[] result = _converter.ToBytes(inputValue);

        Assert.That(result, Is.EqualTo(expectedResult));
    }
    
    [Test]
    [TestCaseSource(typeof(TestCaseDataFactory), nameof(TestCaseDataFactory.ToBytesTestCases))]
    public void FromBytes_WhenCalled_ReturnValue(int expectedResult, byte[] inputBytes)
    {
        int result = _converter.FromBytes(inputBytes);

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void FromBytes_InputHasInvalidLength_ThrowArgumentOutOfRangeException()
    {
        var inputBytes = new byte[5];
        
        Assert.Throws<ArgumentOutOfRangeException>(() => _converter.FromBytes(inputBytes));
    }
   
    private class TestCaseDataFactory
    {
        public static IEnumerable ToBytesTestCases
        {
            get
            {
                yield return new TestCaseData(Input32_0, 
                    Result32_0).SetName("Int32Test0");
                yield return new TestCaseData(Input32_1, 
                    Result32_1).SetName("Int32Test1");
                yield return new TestCaseData(Input32_2, 
                    Result32_2).SetName("Int32Test2");
                yield return new TestCaseData(Input32_3, 
                    Result32_3).SetName("Int32Test3");
                yield return new TestCaseData(Input32_4, 
                    Result32_4).SetName("Int32Test4");
                
                //add more test cases as necessary
            }
        }
    }
}