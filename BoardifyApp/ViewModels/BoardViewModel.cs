using System.Collections.ObjectModel;
using BoardifyApp.Models;
using BoardifyApp.Services;
using BoardifyApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BoardifyApp.ViewModels;

public partial class BoardViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ObservableCollection<ColumnViewModel> _columns = [];

    public BoardViewModel(Board board, INavigationService navigationService, AppSettings? settings = null)
    {
        Board = board;
        _navigationService = navigationService;
        foreach (var col in board.Columns)
        {
            var columnViewModel = new ColumnViewModel(col);
            if (settings is not null && settings.ColumnWidths.TryGetValue(col.Title, out var width))
            {
                columnViewModel.Width = width;
            }

            Columns.Add(columnViewModel);
        }
    }

    public Board Board { get; }

    public void MoveTask(TaskCardViewModel task, ColumnViewModel from, ColumnViewModel to)
    {
        if (from == to)
        {
            return;
        }

        from.RemoveTask(task);
        to.AddTask(task);
    }

    [RelayCommand]
    private void Back()
    {
        _navigationService.NavigateBack();
    }

    [RelayCommand]
    private void OpenAddTaskDialog()
    {
        var dialog = new AddTaskDialog(Columns);
        if (dialog.ShowDialog() != true || dialog.SelectedColumn == null)
        {
            return;
        }

        var newTask = new TaskCard
        {
            Title = dialog.TaskTitle,
            Description = dialog.Description,
        };

        dialog.SelectedColumn.Tasks.Add(new TaskCardViewModel(newTask));
    }
}