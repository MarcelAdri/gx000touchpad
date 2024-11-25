using gx000data;
using gx000serverSimComm;

namespace gx000serverViewModel;

public class ViewModel
{
    public GenerateFlightSimContent SimContent { get; } = new();
    public ProcessSimData DataProcess { get; }
    public DataStore Store { get; } = new();

    public ViewModel()
    {
        DataProcess = new(SimContent);    
    }
}
