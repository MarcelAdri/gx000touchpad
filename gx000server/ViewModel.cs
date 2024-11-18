using System.ComponentModel;
using gx000data;
using gx000serverSimComm;

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