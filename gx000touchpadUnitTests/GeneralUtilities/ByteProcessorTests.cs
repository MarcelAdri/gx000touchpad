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

using GeneralUtilities;
using Moq;

namespace gx000touchpadUnitTests.GeneralUtilities;

[TestFixture]
public class ByteProcessorTests
{
    private ByteProcessor _byteProcessor;
    private Mock<IEndianessChecker> _checker;
    private byte[] _littleEndianArray;
    private byte[] _bigEndianArray;

    [SetUp]
    public void SetUp()
    {
        _checker = new Mock<IEndianessChecker>();
        _byteProcessor = new ByteProcessor(_checker.Object);
        _littleEndianArray = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        _bigEndianArray = new byte[] { 0x04, 0x03, 0x02, 0x01 };
    }

    [Test]
    public void StoreLittleEndian_SystemIsLittleEndian_ArrayUnchanged()
    {
        _checker.Setup(x => x.IsLittleEndian()).Returns(true);
        
        byte[] result = _byteProcessor.StoreLittleEndian(_littleEndianArray);
        
        Assert.That(result, Is.EqualTo(_littleEndianArray));
    }
    
    [Test]
    public void StoreLittleEndian_SystemIsBigEndian_ArrayReversed()
    {
        _checker.Setup(x => x.IsLittleEndian()).Returns(false);
        
        byte[] result = _byteProcessor.StoreLittleEndian(_littleEndianArray);
        
        Assert.That(result, Is.EqualTo(_bigEndianArray));
    }
    
    [Test]
    public void StoreLittleEndian_ArrayIsNull_ThrowsArgumentNullException()
    {
        byte[] arr = null;
        
        Assert.That(() =>_byteProcessor.StoreLittleEndian(arr), Throws.InstanceOf<ArgumentNullException>());
    }
    
    [Test]
    public void StoreLittleEndian_ArrayIsEmpty_ThrowsArgumentOutOfRangeException()
    {
        byte[] arr = new byte[]  {};
        
        Assert.That(() =>_byteProcessor.StoreLittleEndian(arr), Throws.InstanceOf<ArgumentOutOfRangeException>());
    }
}