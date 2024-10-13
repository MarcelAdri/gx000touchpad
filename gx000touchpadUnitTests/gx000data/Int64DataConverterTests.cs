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
public class Int64DataConverterTests
{
    private const Int32 Input64_0 = 0;
    private const Int64 Input64_1 = 1000000000L;
    private const Int64 Input64_2 = -1000000000L;
    private const Int64 Input64_3 = 9223372036854775807L;
    private const Int64 Input64_4 = -9223372036854775808L;
    
    private static readonly byte[] Result64_0 = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];
    private static readonly byte[] Result64_1 = [0x00, 0xCA, 0x9A, 0x3B, 0x00, 0x00, 0x00, 0x00];
    private static readonly byte[] Result64_2 = [0x00, 0x36, 0x65, 0xC4, 0xFF, 0xFF, 0xFF, 0xFF];
    private static readonly byte[] Result64_3 = [0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F];
    private static readonly byte[] Result64_4 = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80];
    
    private Int64DataConverter _converter;

    [SetUp]
    public void SetUp()
    {
        _converter = new Int64DataConverter();
    }
    
    [Test]
    [TestCaseSource(typeof(TestCaseDataFactory), nameof(TestCaseDataFactory.ToBytesTestCases))]
    public void ToBytes_WhenCalled_ReturnValue(long inputValue, byte[] expectedResult)
    {
        byte[] result = _converter.ToBytes(inputValue);

        Assert.That(result, Is.EqualTo(expectedResult));
    }
    
    [Test]
    [TestCaseSource(typeof(TestCaseDataFactory), nameof(TestCaseDataFactory.ToBytesTestCases))]
    public void FromBytes_WhenCalled_ReturnValue(long expectedResult, byte[] inputBytes)
    {
        long result = _converter.FromBytes(inputBytes);

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void FromBytes_InputHasInvalidLength_ThrowArgumentOutOfRangeException()
    {
        var inputBytes = new byte[9];
        
        Assert.Throws<ArgumentOutOfRangeException>(() => _converter.FromBytes(inputBytes));
    }
   
    private class TestCaseDataFactory
    {
        public static IEnumerable ToBytesTestCases
        {
            get
            {
                yield return new TestCaseData(Input64_0, 
                    Result64_0).SetName("Int64Test0");
                yield return new TestCaseData(Input64_1, 
                    Result64_1).SetName("Int64Test1");
                yield return new TestCaseData(Input64_2, 
                    Result64_2).SetName("Int64Test2");
                yield return new TestCaseData(Input64_3, 
                    Result64_3).SetName("Int64Test3");
                yield return new TestCaseData(Input64_4, 
                    Result64_4).SetName("Int64Test4");
                
                //add more test cases as necessary
            }
        }
    }
}