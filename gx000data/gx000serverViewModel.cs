using gx000serverSimComm;

namespace gx000data;

public class gx000serverViewModel
{
    public GenerateFlightSimContent SimContent { get; } = new();
    public ProcessSimData DataProcess { get; }
    public DataStore Store { get; } = new();

    public gx000serverViewModel()
    {
        DataProcess = new(SimContent);    
    }
}