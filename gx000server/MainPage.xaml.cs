namespace gx000server;

public partial class MainPage : ContentPage
{
    private ViewModel _viewModel;

    public MainPage()
    {
        InitializeComponent();
        
        if (_viewModel == null)
        {
            _viewModel = new ViewModel();    
        }
        
        this.BindingContext = _viewModel;
        
        var app = Application.Current as App;
        if (app != null)
        {
            app.ViewModel = _viewModel;
        }

       

    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.SimContent.Stop();
        _viewModel.DataProcess.Dispose();
    }
}