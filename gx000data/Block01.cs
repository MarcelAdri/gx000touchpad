using System.Collections;
using System.ComponentModel;

namespace gx000data;

public struct Block01LayOut
{
    private const string FirstMessageName = "FirstMessage";
    private const int FirstMessageOffset = 0x0004;
    private const int FirstMessageLength = 10;
    private Type _firstMessageType = typeof(string);

    private const string FirstNumberName = "FirstNumber"; 
    private const int FirstNumberOffset = 0x000E;
    private const int FirstNumberLength = 4;
    private Type _firstNumberType = typeof(int);
    
    
    public Dictionary<string, int> DataOffset { get; private set; }
    public Dictionary<string, int > DataLength { get; private set; }
    public Dictionary<string, Type > DataType { get; private set; }

    public Block01LayOut()
    {
        DataOffset[FirstMessageName] = FirstMessageOffset;
        DataLength[FirstMessageName] = FirstMessageLength;
        DataType[FirstMessageName] = _firstMessageType;
        
        DataOffset[FirstNumberName] = FirstNumberOffset;
        DataLength[FirstNumberName] = FirstNumberLength;
        DataType[FirstNumberName] = _firstNumberType;
    }

} 
public class Block01 : DataBlock
{
    private ArrayList _dataBlock;
    private Block01LayOut _blockLayout;
    public Block01()
    {
        _dataBlock = new ArrayList();
    }
    public override void StoreData(string variable, string data)
    {
        if (!_blockLayout.DataLength.TryGetValue(variable, out int value))
        {
            throw new ArgumentException($"{variable} is not part of Block01 LayOut.");
        }
        if (data.Length != _blockLayout.DataLength[variable])
        {
            throw new ArgumentException($"data is not valid for {variable}");
        }
        _dataBlock.Insert(_blockLayout.DataOffset[variable], data);
    }

    public override void StoreData(string variable, int data)
    {
        if (!_blockLayout.DataLength.TryGetValue(variable, out int value))
        {
            throw new ArgumentException($"{variable} is not part of Block01 LayOut.");
        }
        _dataBlock.Insert(_blockLayout.DataOffset[variable], data);
    }

    public override string GetStringData(string variable)
    {
        
    }

    public override int GetIntData(string variable)
    {
        throw new NotImplementedException();
    }
    
    
    
}