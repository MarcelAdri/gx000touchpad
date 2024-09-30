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

using System.Text;
using GeneralUtilities;

namespace gx000touchpadUnitTests.GeneralUtilities;

[TestFixture]
public class FletcherCheckSumTests
{
    //TestVectors
    private const string Test1 = "abcde";
    private const string Test2 = "abcdef";
    private const string Test3 = "abcdefgh";

    private const UInt64 Result16Bit1 = 51440;
    private const UInt64 Result16Bit2 = 8279;
    private const UInt64 Result16Bit3 = 1575;
    
    private const UInt64 Result32Bit1 = 3948201259;
    private const UInt64 Result32Bit2 = 1347824173;
    private const UInt64 Result32Bit3 = 3790311829;
    
    private const UInt64 Result64Bit1 = 14034561336514601929;
    private const UInt64 Result64Bit2 = 14034672391484000458;
    private const UInt64 Result64Bit3 = 2894457976838998732;

    private const FletcherCheckSum.FletcherTypes Fletcher16 = FletcherCheckSum.FletcherTypes.Fletcher16;
    private const FletcherCheckSum.FletcherTypes Fletcher32 = FletcherCheckSum.FletcherTypes.Fletcher32;
    private const FletcherCheckSum.FletcherTypes Fletcher64 = FletcherCheckSum.FletcherTypes.Fletcher64;
    
    [Test]
    [TestCase(Test1,Fletcher16,Result16Bit1)]
    [TestCase(Test2, Fletcher16, Result16Bit2)]
    [TestCase(Test3, Fletcher16, Result16Bit3)]
    [TestCase(Test1, Fletcher32, Result32Bit1)]
    [TestCase(Test2, Fletcher32, Result32Bit2)]
    [TestCase(Test3, Fletcher32, Result32Bit3)]
    [TestCase(Test1, Fletcher64, Result64Bit1)]
    [TestCase(Test2, Fletcher64, Result64Bit2)]
    [TestCase(Test3, Fletcher64, Result64Bit3)]
    public void CalculateChecksumFromBytes_TestVectors_ReturnResult(
        string inputString,FletcherCheckSum.FletcherTypes fletcherType, UInt64 expectedResult)
    {
        byte[] inputArray = Encoding.UTF8.GetBytes(inputString);

        var result = FletcherCheckSum.CalculateChecksumFromBytes(inputArray, fletcherType);

        Assert.That(result == expectedResult);
    }
    
    [Test]
    public void CalculateChecksumFromBytes_ArrayIsNull_ThrowException()
    {
        byte[] inputArray = null;

        Assert.That(() => FletcherCheckSum.CalculateChecksumFromBytes(inputArray, Fletcher64),
            Throws.InstanceOf<ArgumentNullException>());
       
    }
    
}