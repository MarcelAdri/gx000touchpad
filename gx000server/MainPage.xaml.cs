namespace gx000server;

public partial class MainPage : ContentPage
{
    private GenerateFlightSimContent _generator;

    public MainPage()
    {
        InitializeComponent();
        _generator = new GenerateFlightSimContent();
        
        var app = Application.Current as App;
        if (app != null)
        {
            app.Generator = _generator;
        }

        _generator.MessageValueChanged += Generator_MessageValueChanged;
        _generator.IntegerValueChanged += Generator_IntegerValueChanged;
        _generator.LongValueChanged += Generator_LongValueChanged;

        FirstMessageLabel.Text = _generator.MessageValue;
        FirstIntegerLabel.Text = _generator.IntValue;
        FirstLongLabel.Text = _generator.LongValue;
    }

    private void Generator_IntegerValueChanged(object? sender, EventArgs e)
    {
        var text = _generator.IntValue;
        
        MainThread.BeginInvokeOnMainThread(() =>
        {
            FirstIntegerLabel.Text = text;
        });
    }
    private void Generator_LongValueChanged(object? sender, EventArgs e)
    {
        var text = _generator.LongValue;
        
        MainThread.BeginInvokeOnMainThread(() =>
        {
            FirstLongLabel.Text = text;
        });
    }
    private void Generator_MessageValueChanged(object? sender, EventArgs e)
    {
        var text = _generator.MessageValue;
        
        MainThread.BeginInvokeOnMainThread(() =>
        {
            FirstMessageLabel.Text = text;
        });
    }

    /*private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
    */
    
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _generator.Stop();
    }
}