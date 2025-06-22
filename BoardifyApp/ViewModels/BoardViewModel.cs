using System.Collections.ObjectModel;
using BoardifyApp.Models;
using BoardifyApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BoardifyApp.ViewModels;

public partial class BoardViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    
    public Board Board { get; }
    
    [ObservableProperty] 
    private ObservableCollection<ColumnViewModel> _columns = [];
    
    public BoardViewModel(Board board, INavigationService navigationService)
    {
        Board = board;
        _navigationService = navigationService;
        foreach (var col in board.Columns)
        {
            Columns.Add(new ColumnViewModel(col));
        }
    }

    [RelayCommand]
    private void Back()
    {
        _navigationService.NavigateBack();
    }
}