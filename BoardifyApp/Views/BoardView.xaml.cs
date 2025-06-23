using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using BoardifyApp.Helpers;
using BoardifyApp.ViewModels;

namespace BoardifyApp.Views;

public partial class BoardView : UserControl
{
    private DragAdorner? _dragAdorner;
    private AdornerLayer? _adornerLayer;
    private Point _dragStartPoint;

    public BoardView()
    {
        InitializeComponent();
    }

    private void TaskCard_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not Border { DataContext: TaskCardViewModel viewModel } border)
        {
            return;
        }

        _dragStartPoint = e.GetPosition(border);
        
        border.Visibility = Visibility.Hidden;
        
        var visual = new Border
        {
            CornerRadius = new CornerRadius(5),
            Width = border.ActualWidth,
            Height = border.ActualHeight,
            Background = border.Background,
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1),
            Child = new TextBlock
            {
                Margin = new Thickness(3),
                Text = viewModel.TaskCard.Title,
                FontSize = 12,
                Padding = new Thickness(5),
                TextWrapping = TextWrapping.Wrap
            },
            Opacity = 1
        };
        
        
        _adornerLayer = AdornerLayer.GetAdornerLayer(this);
        if (_adornerLayer is not null)
        {
            _dragAdorner = new DragAdorner(this, visual);
            _adornerLayer.Add(_dragAdorner);
        }

        GiveFeedback += BoardView_GiveFeedback;

        var data = new DataObject(typeof(TaskCardViewModel), viewModel);
        DragDrop.DoDragDrop(border, data, DragDropEffects.Move);

        if (_dragAdorner is not null && _adornerLayer is not null)
        {
            _adornerLayer.Remove(_dragAdorner);
            _dragAdorner = null;
            border.Visibility = Visibility.Visible;
        }
        
        GiveFeedback -= BoardView_GiveFeedback;
    }
    
    private void BoardView_GiveFeedback(object sender, GiveFeedbackEventArgs e)
    {
        if (_dragAdorner != null && _adornerLayer != null)
        {
            var screenPosition = MouseHelper.GetCursorPosition();
            var currentPosition = PointFromScreen(screenPosition);

            var offsetX = currentPosition.X - _dragStartPoint.X;
            var offsetY = currentPosition.Y - _dragStartPoint.Y;

            _dragAdorner.SetPosition(offsetX, offsetY);
        }

        e.UseDefaultCursors = false;
        e.Handled = true;
    }

    private void Column_DragOver(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(typeof(TaskCardViewModel)))
        {
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }
        else
        {
            e.Effects = DragDropEffects.None;
        }
    }

    private void Column_Drop(object sender, DragEventArgs e)
    {
        if (sender is Border border)
        {
            DropHelper.SetIsDropTarget(border, false);
        }

        if (e.Data.GetData(typeof(TaskCardViewModel)) is not TaskCardViewModel draggedTask ||
            DataContext is not BoardViewModel boardVm)
        {
            return;
        }

        var targetColumn = (sender as FrameworkElement)?.DataContext as ColumnViewModel;
        var sourceColumn = boardVm.Columns.FirstOrDefault(c => c.Tasks.Contains(draggedTask));
        if (targetColumn != null && sourceColumn != null && targetColumn != sourceColumn)
        {
            boardVm.MoveTask(draggedTask, sourceColumn, targetColumn);
        }
    }

    private void Column_DragEnter(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(typeof(TaskCardViewModel)) &&
            sender is Border border)
        {
            DropHelper.SetIsDropTarget(border, true);
        }
    }

    private void Column_DragLeave(object sender, DragEventArgs e)
    {
        if (sender is Border border)
        {
            DropHelper.SetIsDropTarget(border, false);
        }
    }
}