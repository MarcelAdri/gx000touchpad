namespace gx000server;

public partial class App : Application
{
    public ViewModel ViewModel { get; set; }
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
        
    }
    
    protected override void OnSleep()
    {
        base.OnSleep();
        
        ViewModel?.SimContent.Stop();
        ViewModel?.DataProcess.Dispose();
    }
    
}