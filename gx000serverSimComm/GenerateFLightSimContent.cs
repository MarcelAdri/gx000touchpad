using System.ComponentModel;
using System.Reactive.Linq;
using GeneralUtilities;

namespace gx000serverSimComm;

/// <summary>
/// Generates variable content in a continuous loop for testing purposes.
/// TODO: will be replaced by getting content from FlightSim.
/// </summary>
public class GenerateFlightSimContent : INotifyPropertyChanged
{
    private readonly RandomGenerator _randomGenerator = new ();
    private string _messageValue;
    private string _intValue;
    private string _longValue;
    private IDisposable? _messageSubscription;
    private IDisposable? _intSubscription;
    private IDisposable? _longSubscription;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string FirstMessage
    {
        get => _messageValue;
        private set
        {
            if (_messageValue != value)
            {
                _messageValue = value;
                OnPropertyChanged();
            }
        }
    }
    public string FirstNumber { 
        get => _intValue;
        private set
        {
            if (_intValue != value)
            {
                _intValue = value;
                OnPropertyChanged();
            }   
        } 
    }

    public string FirstLong
    {
        get => _longValue;
        private set
        {
            if (_longValue != value)
            {
                _longValue = value;
                OnPropertyChanged();
            }   
        }
    }
    
    
    public GenerateFlightSimContent()
    {
        FirstMessage = "FirstMessa";
        FirstNumber = "0";
        FirstLong = "0";
        
        _messageSubscription = Observable.Interval(TimeSpan.FromSeconds(5))
            .Subscribe(_ => FirstMessage = SetNewMessage());

        _intSubscription = Observable.Interval(TimeSpan.FromSeconds(6))
            .Subscribe(_ => FirstNumber = SetNewInt());

        _longSubscription = Observable.Interval(TimeSpan.FromSeconds(7))
            .Subscribe(_ => FirstLong = SetNewLong());
    }
    
    public void Stop()
    {
        _messageSubscription?.Dispose();
        _intSubscription?.Dispose();
        _longSubscription?.Dispose();
    }
    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
    private string SetNewLong()
    {
        return _randomGenerator.RandomLongNumber(0, Int64.MaxValue).ToString("#,##0");
    }

    private string SetNewInt()
    {
        return _randomGenerator.RandomIntNumber(0, Int32.MaxValue).ToString("#,##0");
    }

    private string SetNewMessage()
    {
        char offset = 'A';
        
        return _randomGenerator.GenerateRandomString(10, offset);
    }


}
