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
using System.Runtime.InteropServices.JavaScript;
using gx000data;

namespace gx000touchpadUnitTests.gx000data;
//Note: DataConversion always yields little endian output, whatever the endianness of the system

[TestFixture]
public class DataConversionTests
{
    private const Int32 Input32_1 = 10;
    private const Int32 Input32_2 = -10;
    private const Int32 Input32_3 = 2147483647;
    private const Int32 Input32_4 = -2147483648;
    private const Int64 Input64_1 = 1000000000L;
    private const Int64 Input64_2 = -1000000000L;
    private const Int64 Input64_3 = 9223372036854775807L;
    private const Int64 Input64_4 = -9223372036854775808L;
    private const string InputString_1 = "Hello";
    private const string InputString_2 = "World!";
    private const string InputString_3 = "";
    
    private static readonly byte[] Result32_1= [0x0A, 0x00, 0x00, 0x00];
    private static readonly byte[] Result32_2 = [0xF6, 0xFF, 0xFF, 0xFF];
    private static readonly byte[] Result32_3 = [0xFF, 0xFF, 0xFF, 0x7F];
    private static readonly byte[] Result32_4 = [0x00, 0x00, 0x00, 0x80];
    private static readonly byte[] Result64_1 = [0x00, 0xCA, 0x9A, 0x3B, 0x00, 0x00, 0x00, 0x00];
    private static readonly byte[] Result64_2 = [0x00, 0x36, 0x65, 0xC4, 0xFF, 0xFF, 0xFF, 0xFF];
    private static readonly byte[] Result64_3 = [0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F];
    private static readonly byte[] Result64_4 = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80];
    private static readonly byte[] ResultString_1 = [0x48, 0x65, 0x6C, 0x6C, 0x6F];
    private static readonly byte[] ResultString_2= [0x57, 0x6F, 0x72, 0x6C, 0x64, 0x21];
    private static readonly byte[] ResultString_3 = [];

    [Test]
    [TestCaseSource(typeof(TestCaseDataFactory), nameof(TestCaseDataFactory.ToBytesTestCases))]
    public void ToBytes_WhenCalled_ReturnValue<T>(T inputValue, IDataConverter<T> converter, byte[] expectedResult)
    {
        byte[] result = DataConversion.ToBytes(inputValue, converter);

        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [Test]
    public void ToBytes_ConverterIsNull_ThrowAgrumentNullException()
    {
        Assert.That(() => DataConversion.ToBytes(0, null), Throws.ArgumentNullException);
    }
    
    [Test]
    [TestCaseSource(typeof(TestCaseDataFactory), nameof(TestCaseDataFactory.ToBytesTestCases))]
    public void FromBytes_WhenCalled_ReturnValue<T>(T expectedResult, IDataConverter<T> converter, byte[] inputValue)
    {
        T result = DataConversion.FromBytes(inputValue, converter);

        Assert.That(result, Is.EqualTo(expectedResult));
    }
    
    [Test]
    public void FromBytes_ConverterIsNull_ThrowArgumentNullException()
    {
        Assert.That(() => DataConversion.FromBytes<Int32>(new byte[0], null), Throws.ArgumentNullException);
    }
    private class TestCaseDataFactory
    {
        public static IEnumerable ToBytesTestCases
        {
            get
            {
                yield return new TestCaseData(Input32_1, 
                    new Int32DataConverter(), 
                    Result32_1).SetName("Int32Test1");
                yield return new TestCaseData(Input32_2, 
                    new Int32DataConverter(), 
                    Result32_2).SetName("Int32Test2");
                yield return new TestCaseData(Input32_3, 
                    new Int32DataConverter(), 
                    Result32_3).SetName("Int32Test3");
                yield return new TestCaseData(Input32_4, 
                    new Int32DataConverter(), 
                    Result32_4).SetName("Int32Test4");
                yield return new TestCaseData(Input64_1, 
                    new Int64DataConverter(), 
                    Result64_1).SetName("Int64Test1");
                yield return new TestCaseData(Input64_2, 
                    new Int64DataConverter(), 
                    Result64_2).SetName("Int64Test2");
                yield return new TestCaseData(Input64_3, 
                    new Int64DataConverter(), 
                    Result64_3).SetName("Int64Test3");
                yield return new TestCaseData(Input64_4, 
                    new Int64DataConverter(), 
                    Result64_4).SetName("Int64Test4");
                yield return new TestCaseData(InputString_1, 
                    new StringDataConverter(), 
                    ResultString_1).SetName("StringTest1");
                yield return new TestCaseData(InputString_2, 
                    new StringDataConverter(), 
                    ResultString_2).SetName("StringTest2");
                yield return new TestCaseData(InputString_3, 
                    new StringDataConverter(), 
                    ResultString_3).SetName("StringTest3");
                
                //add more test cases as necessary
            }
        }
    }
    
}