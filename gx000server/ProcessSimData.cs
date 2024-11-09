using System.ComponentModel;
using System.Globalization;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using gx000data;

namespace gx000server;

public class ProcessSimData : INotifyPropertyChanged
{
    private readonly GenerateFlightSimContent _content;
    private readonly IDisposable _subscription;
    private string _variableName;
    private string _dataType;
    private Variable _currentVariable;
    
    public event PropertyChangedEventHandler? PropertyChanged;
    public Variable CurrentVariable
    {
        get => _currentVariable;
        set
        {
            _currentVariable = value;
            OnPropertyChanged();
        }
    }

    public string VariableName
    {
        get => _variableName;
        private set
        {
            if (_variableName == value) return;
            _variableName = value;
            OnPropertyChanged();
        }
    }

    public string DataType
    {
        get => _dataType;
        private set
        {
            if (_dataType == value) return;
            _dataType = value;
            OnPropertyChanged();
        }
    }

    public ProcessSimData(GenerateFlightSimContent content)
    {
        _content = content;
        
        _subscription = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                h => content.PropertyChanged += h,
                h => content.PropertyChanged -= h)
            .Subscribe(e => OnNext(e.EventArgs));
    }

    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
    public void Dispose()
    {
        _subscription.Dispose();
    }
    private void OnNext(PropertyChangedEventArgs e)
    {
        VariableName = e.PropertyName;
        var propertyInfo = _content.GetType().GetProperty(VariableName);
        
        if (propertyInfo == null)
            throw new InvalidOperationException($"{VariableName} is not a valid property name");

        GetType(VariableName);
        var contentValue = (string)propertyInfo.GetValue(_content);
        var numberContentValue = UnformatNumber(contentValue);
        
        
        switch (DataType)
        {
            case "StringType":
                CurrentVariable = new StringVariable(
                    VariableName,
                    contentValue);
                break;
            case "IntType":
                if (int.TryParse(numberContentValue, out var intValue))
                {
                    CurrentVariable = new Int32Variable(
                        VariableName,
                        intValue);
                }
                else
                {
                    throw new InvalidOperationException($"Cannot convert {numberContentValue} to {typeof(int)}");
                }
                break;
            case "LongType":
                if (long.TryParse(numberContentValue, out var longValue))
                {
                    CurrentVariable = new Int64Variable(
                        VariableName,
                        longValue);
                }
                else
                {
                    throw new InvalidOperationException($"Cannot convert {numberContentValue} to {typeof(long)}");
                }
                break;
        }
        

        //Console.WriteLine($"{e.PropertyName} is gewijzigd. Type is: {DataType}.");
    }

    private void CheckType(Type propertyType, Type definedType)
    {
        Console.WriteLine($"PropertyType = {propertyType.Name}, DefinedType = {definedType.Name}.");
        if (propertyType.Name != definedType.Name)
            throw new InvalidOperationException($"{definedType.FullName} is not a valid type");
    }

    private void GetType(string variableName)
    {
        try
        {
            DataType = VariableDefinitions.GetVariableAttributes(variableName).Type;
        }
        catch
        {
            throw new Exception($"Variable {variableName} is not supported");
        }
    }
    private static string UnformatNumber(string formattedNumber)
    {
        var cultureInfo = CultureInfo.CurrentCulture;
        
        char decimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator[0];
        char groupSeparator = cultureInfo.NumberFormat.NumberGroupSeparator[0];

        string result = formattedNumber.Replace(groupSeparator.ToString(), string.Empty);

        result = result.Replace(decimalSeparator, '.');
        
        return Regex.Replace(result, @"[^0-9.-]", "");
    }

}