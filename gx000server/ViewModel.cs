using System.ComponentModel;

namespace gx000server;

public class ViewModel
{
    public GenerateFlightSimContent SimContent { get; } = new();
    public ProcessSimData DataProcess { get; }

    public ViewModel()
    {
        DataProcess = new(SimContent);    
    }
    
}