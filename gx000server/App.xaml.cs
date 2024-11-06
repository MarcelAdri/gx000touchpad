namespace gx000server;

public partial class App : Application
{
    public GenerateFlightSimContent Generator { get; set; }
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }

    protected override void OnSleep()
    {
        base.OnSleep();
        
        Generator?.Stop();
    }
    
}