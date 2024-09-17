namespace gx000data;

public interface IVariableDefinitions
{
    string FirstMessageName { get; }
    string FirstNumberName { get; }
    int NumberOfBlocks { get; }
    int BlockSize { get; }
    int ByteSizeOfBlockNumber { get; }
    int ByteSizeOfChecksum { get; }
    
    bool SizeMatters(AvailableTypes variableType);
    IVariableAttributes FindVariableAttributes(string variableName);
}