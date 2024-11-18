using gx000data;
using gx000serverSimComm;

namespace gx000touchpadUnitTests.gx000data;

public class TestableProcessSimData(GenerateFlightSimContent content) : ProcessSimData(content)
{
    public void SetSubscription(IDisposable obj)
    {
        _subscription = obj;
    }

    public void SetVariable(Variable variable)
    {
        CurrentVariable = variable;
    }

    public void SetVariableName(string variableName)
    {
        VariableName = variableName;
    }
    
    public void SetDataType(string dataType)
    {
        DataType = dataType;
    }
    
}