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
using gx000data;
using Moq;

namespace gx000touchpadUnitTests.gx000data;

[TestFixture]
public class InternalCommBlockTests
{
    private Mock<IDataStore> _dataStore;
    private string _firstMessage;
    private int _firstNumber;
    private byte[] _firstMessageBytes;
    private byte[] _firstNumberBytes;
    private int _blockNumber;
    private int _firstMessageOffset;
    private int _firstMessageLength;
    private int _firstNumberOffset;
    private int _firstNumberLength;
    private byte _defaultFiller;
    private long _firstLong;
    private byte[] _firstLongBytes;
    private int _firstLongOffset;
    private int _firstLongLength;

    [SetUp]
    public void Setup()
    {
        _blockNumber = 1;
        _dataStore = new Mock<IDataStore>();
        _firstMessage = "AbCdEfGhIj";
        _firstMessageBytes = DataConversion.ToBytes(_firstMessage, new StringDataConverter());
        _firstMessageOffset = VariableDefinitions.
            FindVariableAttributes(VariableDefinitions.FirstMessageName).OffsetInBlock;
        _firstMessageLength = VariableDefinitions.FindVariableAttributes(VariableDefinitions.FirstMessageName).Length;
        _firstNumber = 195;
        _firstNumberBytes = DataConversion.ToBytes(_firstNumber, new Int32DataConverter());
        _firstNumberOffset = VariableDefinitions.
            FindVariableAttributes(VariableDefinitions.FirstNumberName).OffsetInBlock;
        _firstNumberLength = VariableDefinitions.FindVariableAttributes(VariableDefinitions.FirstNumberName).Length;
        _firstLong = 1000000000L;
        _firstLongBytes = DataConversion.ToBytes(_firstLong, new Int64DataConverter());
        _firstLongOffset = VariableDefinitions.
            FindVariableAttributes(VariableDefinitions.FirstLongName).OffsetInBlock;
        _firstLongLength = VariableDefinitions.FindVariableAttributes(VariableDefinitions.FirstLongName).Length;
        _defaultFiller = 32;
        

    }
    
    [Test]
    public void MakeBlock_VariablesDefined_ReturnsBlockWithValidData()
    {
        SetupDataStoreForStringVariable();
        SetupDataStoreForInt32Variable();
        SetupDataStoreForInt64Variable();
        
        var result = InternalCommBlock.MakeBlock(_blockNumber, _dataStore.Object);
        
        var resultMessage = result[_firstMessageOffset..(_firstMessageOffset+_firstMessageLength)];
        var resultNumber = result[_firstNumberOffset..(_firstNumberOffset + _firstNumberLength)];
        var resultLong = result[_firstLongOffset..(_firstLongOffset + _firstLongLength)];
        
        Assert.That(_firstNumberBytes, Is.EqualTo(resultNumber), "First number is correct");
        Assert.That(_firstLongBytes, Is.EqualTo(resultLong), "First long is correct");
        Assert.That(_firstMessageBytes, Is.EqualTo(resultMessage), "First message is correct");
    }

    [Test]
    public void MakeBlock_VariablesDefined_ChecksumIsValid()
    {
        SetupDataStoreForStringVariable();
        SetupDataStoreForInt32Variable();
        SetupDataStoreForInt64Variable();
        
        byte[] result = InternalCommBlock.MakeBlock(_blockNumber, _dataStore.Object);
        
        byte[] resultChecksum = result[^VariableDefinitions.ByteSizeOfChecksum..];
        
        ulong calculatedChecksum = FletcherCheckSum.CalculateChecksumFromBytes(
            result[..^VariableDefinitions.ByteSizeOfChecksum],
            FletcherCheckSum.FletcherTypes.Fletcher64);
        ulong foundCheckSum = BitConverter.ToUInt64(resultChecksum);
        
        Assert.That(calculatedChecksum == foundCheckSum, "Checksum is correct");
    }
    
    [Test]
    public void MakeBlock_VariablesNotDefined_ReturnsBlockWithEmptyData()
    {
        SetupDataStoreWithoutVariables();
        
        var result = InternalCommBlock.MakeBlock(_blockNumber, _dataStore.Object);
        
        var resultMessage = result[_firstMessageOffset..(_firstMessageOffset + _firstMessageLength)];
        var resultNumber = result[_firstNumberOffset..(_firstNumberOffset + _firstNumberLength)];
        var resultLong = result[_firstLongOffset..(_firstLongOffset + _firstLongLength)];
        
        var emptyMessage = new byte[_firstMessageLength];
        Array.Fill(emptyMessage, _defaultFiller);
        var emptyNumber = new byte[_firstNumberLength];
        Array.Fill(emptyNumber, _defaultFiller);
        var emptyLong = new byte[_firstLongLength];
        Array.Fill(emptyLong, _defaultFiller);
        
        Assert.That(resultNumber, Is.EqualTo(emptyNumber), "First number is empty");
        Assert.That(resultLong, Is.EqualTo(emptyLong), "First long is empty");
        Assert.That(resultMessage, Is.EqualTo(emptyMessage), "First message is empty");
    }
    
    [Test]
    public void MakeBlock_InvalidBlockNumber_ThrowArgumentOutOfRangeException()
    {
        var invalidBlocknumber = -1;
        
        Assert.That(() => InternalCommBlock.MakeBlock(invalidBlocknumber, It.IsAny<IDataStore>()),
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    [Test]
    public void ExtractBlock_ValidBlock_ReturnsListOfVariables()
    {
        SetupDataStoreForStringVariable();
        SetupDataStoreForInt32Variable();
        SetupDataStoreForInt64Variable();
        
        var testBlock = InternalCommBlock.MakeBlock(1, _dataStore.Object);
        
        var result = InternalCommBlock.ExtractBlock(testBlock);
        
        Assert.That(result.Count == 3, "Number of variables is correct.");
        Assert.That(result.Any(r => r.VariableName == VariableDefinitions.FirstNumberName), Is.True,
            "FirstNumber is found.");
        Assert.That(result.Any(r => r.VariableName == VariableDefinitions.FirstLongName), Is.True,
            "FirstLong is found.");
        Assert.That(result.Any(r => r.VariableName == VariableDefinitions.FirstMessageName), Is.True,
            "FirstMessage is found.");
    }
    
    [Test]
    public void ExtractBlock_InvalidBlockLength_ThrowsArgumentException()
    {
        var testBlock = new byte[VariableDefinitions.BlockSize - 1];
        
        Assert.That(() => InternalCommBlock.ExtractBlock(testBlock), Throws.TypeOf<ArgumentException>());
    }
    
    [Test]
    public void ExtractBlock_InvalidChecksum_ThrowsArgumentException()
    {
        SetupDataStoreForStringVariable();
        SetupDataStoreForInt32Variable();
        SetupDataStoreForInt64Variable();
        
        var testBlock = InternalCommBlock.MakeBlock(1, _dataStore.Object);

        var checksumDistortion = testBlock[VariableDefinitions.BlockSize - 1];
        checksumDistortion -= 1;
        
        testBlock[VariableDefinitions.BlockSize - 1] = checksumDistortion;
        
        Assert.That(() => InternalCommBlock.ExtractBlock(testBlock), Throws.TypeOf<ArgumentException>());
    }
    
    private void SetupDataStoreForStringVariable()
    {
        Variable variable;
        _dataStore.Setup(d => d.TryGetDataFromStore(
                VariableDefinitions.FirstMessageName, out variable))
            .Returns(true)
            .Callback(new TryGetDataFromStoreDelegate((string name, out Variable outVar) =>
            {
                outVar = new StringVariable(
                    VariableDefinitions.FirstMessageName,
                    DataExchange.DataStatus.Synchronized,
                    _firstMessage);
            }));
    }

    private void SetupDataStoreForInt32Variable()
    {
        Variable variable;
        _dataStore.Setup(d => d.TryGetDataFromStore(
                VariableDefinitions.FirstNumberName, out variable))
            .Returns(true)
            .Callback(new TryGetDataFromStoreDelegate((string name, out Variable outVar) =>
            {
                outVar = new Int32Variable(
                    VariableDefinitions.FirstNumberName,
                    DataExchange.DataStatus.Synchronized,
                    _firstNumber);
            }));
    }
    
    private void SetupDataStoreForInt64Variable()
    {
        Variable variable;
        _dataStore.Setup(d => d.TryGetDataFromStore(
                VariableDefinitions.FirstLongName, out variable))
            .Returns(true)
            .Callback(new TryGetDataFromStoreDelegate((string name, out Variable outVar) =>
            {
                outVar = new Int64Variable(
                    VariableDefinitions.FirstLongName,
                    DataExchange.DataStatus.Synchronized,
                    _firstLong);
            }));
    }
    
    private void SetupDataStoreWithoutVariables()
    {
        Variable variable;
        _dataStore.Setup(d => d.TryGetDataFromStore(
                It.IsAny<string>(), out variable))
            .Returns(false)
            .Callback(new TryGetDataFromStoreDelegate((string name, out Variable outVar) =>
            {
                outVar = null;
            }));
    }
    
    private delegate void TryGetDataFromStoreDelegate(string variableName, out Variable variable);
}