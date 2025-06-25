using System.ComponentModel;
using System.Windows;
using BoardifyApp.Services;
using BoardifyApp.ViewModels;

namespace BoardifyApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        var settings = SettingsManager.Load();

        if (settings.WindowWidth > 0)
        {
            Width = settings.WindowWidth;
        }

        if (settings.WindowHeight > 0)
        {
            Height = settings.WindowHeight;
        }

        Top = settings.WindowTop;
        Left = settings.WindowLeft;
        WindowState = settings.WindowState;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (DataContext is MainViewModel mainViewModel)
        {
            mainViewModel.AppSettings.WindowWidth = Width;
            mainViewModel.AppSettings.WindowHeight = Height;
            mainViewModel.AppSettings.WindowTop = Top;
            mainViewModel.AppSettings.WindowLeft = Left;
            mainViewModel.AppSettings.WindowState = WindowState;

            mainViewModel.SaveSettings();
        }

        base.OnClosing(e);
    }
}