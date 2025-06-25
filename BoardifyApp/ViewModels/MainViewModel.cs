using System.Collections.ObjectModel;
using BoardifyApp.Models;
using BoardifyApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BoardifyApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly NavigationService _navigationService;

    [ObservableProperty]
    private ObservableCollection<BoardViewModel> _boards = [];

    [ObservableProperty]
    private object? _currentViewModel;

    public MainViewModel()
    {
        _navigationService = new NavigationService(vm => CurrentViewModel = vm);
        AppSettings = SettingsManager.Load();

        var defaultBoard = new DataService().LoadBoards();

        Boards.Add(new BoardViewModel(defaultBoard.First(), _navigationService, AppSettings));
        CurrentViewModel = this;
    }

    public AppSettings AppSettings { get; set; }

    [RelayCommand]
    private void CreateBoard()
    {
        var board = new BoardViewModel(new Board { Name = "Новая доска" }, _navigationService);
        Boards.Add(board);
    }

    [RelayCommand]
    private void OpenBoard(BoardViewModel board)
    {
        _navigationService.NavigateTo(board);
    }

    public void SaveSettings()
    {
        foreach (var boardViewModel in Boards)
        {
            foreach (var columnViewModel in boardViewModel.Columns)
            {
                AppSettings.ColumnWidths[columnViewModel.Column.Title] = columnViewModel.Width;
            }
        }

        SettingsManager.Save(AppSettings);
    }
}