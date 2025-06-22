using System.Collections.ObjectModel;
using BoardifyApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BoardifyApp.ViewModels;

public partial class ColumnViewModel : ObservableObject
{
    public Column Column { get; }

    [ObservableProperty]
    private ObservableCollection<TaskCardViewModel> _tasks = [];

    public ColumnViewModel(Column column)
    {
        Column = column;
        foreach (var task in column.Tasks)
        {
            Tasks.Add(new TaskCardViewModel(task));
        }
    }
}