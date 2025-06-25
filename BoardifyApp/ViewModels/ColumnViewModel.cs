using System.Collections.ObjectModel;
using BoardifyApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BoardifyApp.ViewModels;

public partial class ColumnViewModel : ObservableObject
{
    public Column Column { get; }

    [ObservableProperty]
    private double _width = 250;
    
    [ObservableProperty]
    private ObservableCollection<TaskCardViewModel> _tasks = [];

    public ColumnViewModel(Column column)
    {
        Column = column;
        Width = _width;
        foreach (var task in column.Tasks)
        {
            Tasks.Add(new TaskCardViewModel(task));
        }
    }

    public void RemoveTask(TaskCardViewModel task)
    {
        Tasks.Remove(task);
    }

    public void AddTask(TaskCardViewModel task)
    {
        Tasks.Add(task);
    }
}