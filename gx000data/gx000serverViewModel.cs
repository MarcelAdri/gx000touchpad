using gx000serverSimComm;

namespace gx000data;

public class gx000serverViewModel
{
    private static readonly Lazy<gx000serverViewModel> _instance = new (() => new gx000serverViewModel());
    
    public static gx000serverViewModel Instance => _instance.Value;
    public GenerateFlightSimContent SimContent { get; } = new();
    public ProcessSimData DataProcess { get; }
    public DataStore Store { get; } = new();

    private  gx000serverViewModel()
    {
        DataProcess = new(SimContent);    
    }
}