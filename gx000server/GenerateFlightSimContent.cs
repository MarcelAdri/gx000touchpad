using GeneralUtilities;

namespace gx000server;

/// <summary>
/// Generates variable content in a continuous loop for testing purposes.
/// TODO: will be replaced by getting content from FlightSim.
/// </summary>
public class GenerateFlightSimContent
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

    public event EventHandler MessageValueChanged;
    public event EventHandler IntegerValueChanged;
    public event EventHandler LongValueChanged;
    public string MessageValue
    {
        get => _messageValue;
        private set
        {
            if (_messageValue != value)
            {
                _messageValue = value;
                OnMessageChanged(EventArgs.Empty);
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
                OnIntegerChanged(EventArgs.Empty);
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
                OnLongChanged(EventArgs.Empty);
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
    }

    public void Stop()
    {
        _running = false;
        if (_thread != null && _thread.IsAlive)
        {
            _thread.Join();
        }
    }
    protected virtual void OnMessageChanged(EventArgs e)
    {
        MessageValueChanged?.Invoke(this, e);
    }
    protected virtual void OnIntegerChanged(EventArgs e)
    {
        IntegerValueChanged?.Invoke(this, e);
    }
    protected virtual void OnLongChanged(EventArgs e)
    {
        LongValueChanged?.Invoke(this, e);
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
        
        //Console.WriteLine(result);

        return result;
    }

    
}