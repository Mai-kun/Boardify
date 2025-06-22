using BoardifyApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BoardifyApp.ViewModels;

public partial class TaskCardViewModel(TaskCard task) : ObservableObject
{
    public TaskCard TaskCard { get; } = task;
}