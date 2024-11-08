using System.ComponentModel;
using System.Runtime.CompilerServices;
using GeneralUtilities;

namespace gx000server;

/// <summary>
/// Generates variable content in a continuous loop for testing purposes.
/// TODO: will be replaced by getting content from FlightSim.
/// </summary>
public class GenerateFlightSimContent : INotifyPropertyChanged
{
    private Thread _thread;
    private bool _running;
    private DateTime _startTimeMessage = DateTime.Now;
    private DateTime _startTimeInt = DateTime.Now;
    private DateTime _startTimeLong = DateTime.Now;
    private readonly RandomGenerator _randomGenerator = new ();
    private string _messageValue;
    private string _intValue;
    private string _longValue;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string MessageValue
    {
        get => _messageValue;
        private set
        {
            if (_messageValue != value)
            {
                _messageValue = value;
                OnPropertyChanged(nameof(MessageValue));
            }
        }
    }
    public string IntValue { 
        get => _intValue;
        private set
        {
            if (_intValue != value)
            {
                _intValue = value;
                OnPropertyChanged(nameof(IntValue));
            }   
        } 
    }

    public string LongValue
    {
        get => _longValue;
        private set
        {
            if (_longValue != value)
            {
                _longValue = value;
                OnPropertyChanged(nameof(LongValue));
            }   
        }
    }
    
    
    public GenerateFlightSimContent()
    {
        _running = true;
        _thread = new Thread(new ThreadStart(Run));
        _thread.Start();

        MessageValue = "FirstMessa";
        IntValue = "0";
        LongValue = "0";
    }
    
    private void Run()
    {
        while (_running)
        {
            Console.WriteLine("Running...");
            var now = DateTime.Now;
            if (now - _startTimeMessage > TimeSpan.FromSeconds(10))
            {
                MessageValue = SetNewMessage();
            }
            if (now - _startTimeInt > TimeSpan.FromSeconds(15))
            {
                IntValue = SetNewInt();
            }
            if (now - _startTimeLong > TimeSpan.FromSeconds(20))
            {
                LongValue = SetNewLong();
            }
            Thread.Sleep(1000);
        }
        Console.WriteLine("Thread beëindigd");
    }

    public void Stop()
    {
        Console.WriteLine("Stopping");
        _running = false;
        if (_thread != null && _thread.IsAlive)
        {
            _thread.Join();
        }
        Console.WriteLine("Thread beëindigd succesvol");
    }
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
    private string SetNewLong()
    {
        _startTimeLong = DateTime.Now;
        
        var result = _randomGenerator.RandomLongNumber(0, Int64.MaxValue).ToString("#,##0");

        return result;
    }

    private string SetNewInt()
    {
        _startTimeInt = DateTime.Now;
        
        var result = _randomGenerator.RandomIntNumber(0, Int32.MaxValue).ToString("#,##0");

        return result;
    }

    private string SetNewMessage()
    {
        _startTimeMessage = DateTime.Now;
        char offset = 'A';
        
        var result = _randomGenerator.GenerateRandomString(10, offset);
        
        return result;
    }


}