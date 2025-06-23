using System.Windows;
using BoardifyApp.ViewModels;

namespace BoardifyApp.Views;

public partial class AddTaskDialog : Window
{
    public string TaskTitle => TitleBox.Text;
    public string Description => DescriptionBox.Text;
    public ColumnViewModel? SelectedColumn => ColumnSelector.SelectedItem as ColumnViewModel;
    
    public AddTaskDialog(IEnumerable<ColumnViewModel> columns)
    {
        InitializeComponent();
        ColumnSelector.ItemsSource = columns;
        ColumnSelector.SelectedIndex = 0;
    }
    
    private void Add_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(TaskTitle) && SelectedColumn != null)
            DialogResult = true;
    }
}