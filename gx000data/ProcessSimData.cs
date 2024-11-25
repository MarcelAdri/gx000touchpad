using System.ComponentModel;
using System.Reactive.Linq;
using GeneralUtilities;
using gx000serverSimComm;

namespace gx000data;

 /// <summary>
/// Handles the processing of simulation data for flight simulation content.
/// </summary>
public class ProcessSimData : INotifyPropertyChanged, IDisposable
{
    /// <summary>
    /// Holds an instance of the GenerateFlightSimContent class, which generates
    /// variable content for simulation/testing purposes.
    /// </summary>
    private readonly GenerateFlightSimContent _content;

    /// <summary>
    /// Represents an active subscription to observable events generated from property changes
    /// in the GenerateFlightSimContent instance.
    /// The subscription listens for PropertyChanged events and processes them accordingly.
    /// </summary>
    protected IDisposable _subscription;

    /// <summary>
    /// Holds the name of a variable being processed for simulation purposes.
    /// </summary>
    private string _variableName;

    /// <summary>
    /// Represents the data type associated with the current variable in the ProcessSimData class.
    /// This field is private and is used internally within the ProcessSimData class to manage
    /// the type of data being processed.
    /// </summary>
    private string _dataType;

    private string _trigger;

    /// <summary>
    /// Holds the data of the currently active variable within the simulation.
    /// </summary>
    private Variable _currentVariable;
    
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// Event that is triggered when a property value changes.
    /// Implements the INotifyPropertyChanged interface's PropertyChanged event.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;
    /// <summary>
    /// The ProcessSimData class is responsible for processing simulated flight data. It listens for property changes
    /// in the GenerateFlightSimContent and processes them accordingly. The class implements the INotifyPropertyChanged
    /// interface to notify the UI of any updates to its properties.
    /// </summary>
    /// <remarks>
    /// The class subscribes to property change events from the GenerateFlightSimContent and updates the CurrentVariable
    /// property based on the new values. It processes different data types such as StringType, IntType, and LongType
    /// to create appropriate Variable objects.
    /// </remarks>
    /// <example>
    /// ProcessSimData is instantiated by passing a GenerateFlightSimContent object to its constructor. The class listens
    /// for changes and processes them automatically.
    /// </example>
    /// <seealso cref="GenerateFLightSimContent.GenerateFlightSimContent"/>
    /// <seealso cref="Variable"/>
    public ProcessSimData(GenerateFlightSimContent content)
    {
        _content = content;
        
        _subscription = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                h => content.PropertyChanged += h,
                h => content.PropertyChanged -= h)
            .Subscribe(e => OnNext(e.EventArgs));
    }
    
    /// <summary>
    /// Gets or sets the current <see cref="Variable"/> instance.
    /// </summary>
    /// <remarks>
    /// This property represents the variable in the simulation data processing context currently being processed.
    /// </remarks>
    public Variable CurrentVariable
    {
        get => _currentVariable;
        protected set
        {
            _currentVariable = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the name of the variable generated from the simulation content.
    /// </summary>
    /// <remarks>
    /// The value of this property is set whenever a property change is detected in the GenerateFlightSimContent instance.
    /// </remarks>
    public string VariableName
    {
        get => _variableName;
        protected set
        {
            if (_variableName == value) return;
            _variableName = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets the type of the data associated with the current variable.
    /// The property value is derived from the variable attributes fetched
    /// from the VariableDefinitions class based on the variable name.
    /// </summary>
    /// <value>
    /// The type of the data, such as "StringType", "IntType", or "LongType".
    /// Changes to this property trigger the PropertyChanged event.
    /// </value>
    public string DataType
    {
        get => _dataType;
        protected set
        {
            if (_dataType == value) return;
            _dataType = value;
            OnPropertyChanged();
        }
    }
    public string Trigger
    {
        get => _trigger;
        protected set
        {
            if (_trigger == value) return;
            _trigger = value;
            OnPropertyChanged();
        }
    }

   
    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">
    /// The name of the property that changed. This is optional and can be supplied automatically
    /// by the CallerMemberName attribute.
    /// </param>
    private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Releases the resources used by the ProcessSimData instance.
    /// </summary>
    /// <remarks>
    /// This method is responsible for cleaning up resources consumed by the ProcessSimData object.
    /// It invokes the Dispose method with disposing set to true to release managed resources and
    /// then calls SuppressFinalize to prevent finalization.
    /// </remarks>
    /// <seealso cref="System.IDisposable"/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases all resources used by the ProcessSimData instance.
    /// </summary>
    /// <param name="disposing">Indicates whether the method call comes from a Dispose method (its value is true)
    /// or from a finalizer (its value is false).</param>
    private void Dispose(bool disposing)
    {
        if (IsDisposed) return;
        if (disposing)
        {
            _subscription.Dispose();
        }
        
        IsDisposed = true;
    }

    /// <summary>
    /// Handles the event when a property value changes in the observed GenerateFlightSimContent instance.
    /// Updates the appropriate fields and processes the new value based on its data type.
    /// </summary>
    /// <param name="e">The event arguments containing the name of the property that changed.</param>
    private void OnNext(PropertyChangedEventArgs e)
    {
        VariableName = e.PropertyName;
        var propertyInfo = _content.GetType().GetProperty(VariableName);
        
        if (propertyInfo == null)
            throw new InvalidOperationException($"{VariableName} is not a valid property name");

        SetDataType(VariableName);
        
        var contentValue = propertyInfo.GetValue(_content) as string
            ?? throw new InvalidOperationException($"{VariableName} does not contain a string value.");

        switch (DataType)
        {
            case "StringType":
                ProcessStringVariable(contentValue);
                break;
            case "IntType":
                ProcessIntVariable(contentValue);
                break;
            case "LongType":
                ProcessLongVariable(contentValue);
                break;
        }
        CurrentVariable.SetCurrentTrigger(Variable.Triggers.SimSendsUpdate);
        Trigger = CurrentVariable.GetCurrentTrigger().ToString();
        
        //gx000serverViewModel.ViewModel.DataStore.Store(CurrentVariable);

    }

    /// <summary>
    /// Retrieves the data type of a variable based on its name and sets the DataType property accordingly.
    /// </summary>
    /// <param name="variableName">The name of the variable whose data type is to be determined.</param>
    /// <exception cref="Exception">Thrown when the variable is not supported.</exception>
    private void SetDataType(string variableName)
    {
        try
        {
            DataType = VariableDefinitions.GetVariableAttributes(variableName).Type;
        }
        catch (KeyNotFoundException e)
        {
            throw new Exception($"Variable {variableName} is not supported", e);
        }
    }

    /// <summary>
    /// Processes a string variable by creating a new instance of the StringVariable class with the given content value.
    /// </summary>
    /// <param name="contentValue">The string content value to be processed into a StringVariable.</param>
    private void ProcessStringVariable(string contentValue)
    {
        CurrentVariable = new StringVariable(VariableName, contentValue);
    }

    /// <summary>
    /// Processes an integer variable by converting the given content value from a string to an integer.
    /// If the conversion is successful, updates the CurrentVariable property with an Int32Variable instance.
    /// </summary>
    /// <param name="contentValue">The string representation of the integer value to be processed.</param>
    /// <exception cref="InvalidOperationException">Thrown if the content value cannot be converted to an integer.</exception>
    private void ProcessIntVariable(string contentValue)
    {
        string numberContentValue = NumberServices.UnformatNumber(contentValue);
        if (int.TryParse(numberContentValue, out var intValue))
        {
            CurrentVariable = new Int32Variable(VariableName, intValue);
        }
        else
        {
            throw new InvalidOperationException($"Cannot convert {numberContentValue} to {typeof(int)}");
        }
    }

    /// <summary>
    /// Processes a long-type variable from the given content value.
    /// </summary>
    /// <param name="contentValue">The string representation of the content value to be processed.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the provided contentValue cannot be converted to a long integer.
    /// </exception>
    private void ProcessLongVariable(string contentValue)
    {
        string numberContentValue = NumberServices.UnformatNumber(contentValue);
        if (long.TryParse(numberContentValue, out var longValue))
        {
            CurrentVariable = new Int64Variable(VariableName, longValue);
        }
        else
        {
            throw new InvalidOperationException($"Cannot convert {numberContentValue} to {typeof(long)}");
        }
    }
}
