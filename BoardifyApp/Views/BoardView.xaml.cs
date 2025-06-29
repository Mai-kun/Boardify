// ReSharper disable MemberCanBeMadeStatic.Local

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using BoardifyApp.Helpers;
using BoardifyApp.ViewModels;
using MaterialDesignThemes.Wpf;

namespace BoardifyApp.Views;

public partial class BoardView
{
    private AdornerLayer? _adornerLayer;
    private DragAdorner? _dragAdorner;
    private Point _dragStartPoint;

    public BoardView()
    {
        InitializeComponent();
    }

    private void BoardView_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is not BoardViewModel boardViewModel)
        {
            return;
        }

        ColumnGrid.ColumnDefinitions.Clear();
        ColumnGrid.Children.Clear();

        for (var i = 0; i < boardViewModel.Columns.Count; i++)
        {
            var columnViewModel = boardViewModel.Columns[i];

            AddNewColumnDefinition(new GridLength(columnViewModel.Width, GridUnitType.Pixel));
            var border = CreateNewColumnInstance(columnViewModel);
            Grid.SetColumn(border, i * 2);
            ColumnGrid.Children.Add(border);

            var splitter = CreateNewGridSplitterInstance(i * 2, columnViewModel);
            AddNewColumnDefinition(GridLength.Auto, true);
            Grid.SetColumn(splitter, i * 2 + 1);
            ColumnGrid.Children.Add(splitter);
        }
    }

    private void AddNewColumnDefinition(GridLength width, bool isSplitter = false)
    {
        var columnDefinition = new ColumnDefinition
        {
            Width = width,
            MinWidth = isSplitter ? 0 : 200,
        };
        ColumnGrid.ColumnDefinitions.Add(columnDefinition);
    }

    private Border CreateNewColumnInstance(ColumnViewModel columnViewModel)
    {
        var border = new Border
        {
            DataContext = columnViewModel,
            Child = BuildColumnContent(columnViewModel),
        };
        border.SetResourceReference(StyleProperty, "DropHighlightBorder");
        border.AllowDrop = true;
        border.DragEnter += Column_DragEnter;
        border.DragLeave += Column_DragLeave;
        border.DragOver += Column_DragOver;
        border.Drop += Column_Drop;
        return border;
    }

    private StackPanel BuildColumnContent(ColumnViewModel columnViewModel)
    {
        var textBox = new TextBox
        {
            Text = columnViewModel.Column.Title,
            Style = (Style)FindResource("MaterialDesignOutlinedTextBox"),
            Margin = new Thickness(0, 0, 0, 8),
            IsReadOnly = true,
        };
        HintAssist.SetHint(textBox, "Column Title");
        textBox.MouseDoubleClick += (_, _) => textBox.IsReadOnly = false;
        textBox.LostFocus += (_, _) => textBox.IsReadOnly = true;

        var card = new ItemsControl
        {
            ItemsSource = columnViewModel.Tasks,
            ItemTemplate = FindResource("TaskCardTemplate") as DataTemplate,
        };

        return new StackPanel
        {
            Orientation = Orientation.Vertical,
            Children =
            {
                textBox,
                card,
            },
        };
    }

    private GridSplitter CreateNewGridSplitterInstance(int columnCount, ColumnViewModel columnViewModel)
    {
        var splitter = new GridSplitter
        {
            Style = FindResource("GridSplitterStyle") as Style,
        };
        splitter.DragCompleted += (_, _) =>
        {
            columnViewModel.Width = ColumnGrid.ColumnDefinitions[columnCount].Width.Value;
        };
        splitter.MouseDoubleClick += (_, _) =>
        {
            ColumnGrid.ColumnDefinitions[columnCount].Width = new GridLength(250, GridUnitType.Pixel);
        };
        return splitter;
    }

    private void TaskCard_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not Card { DataContext: TaskCardViewModel viewModel } card)
        {
            return;
        }

        _dragStartPoint = e.GetPosition(card);

        card.Visibility = Visibility.Hidden;
        var contentPresenter = new ContentPresenter
        {
            Content = viewModel,
            ContentTemplate = FindResource("TaskCardTemplate") as DataTemplate,
            Width = card.ActualWidth,
            Height = card.ActualHeight,
        };

        _adornerLayer = AdornerLayer.GetAdornerLayer(this);
        if (_adornerLayer is not null)
        {
            _dragAdorner = new DragAdorner(this, contentPresenter);
            _adornerLayer.Add(_dragAdorner);
        }

        GiveFeedback += BoardView_GiveFeedback;

        var data = new DataObject(typeof(TaskCardViewModel), viewModel);
        DragDrop.DoDragDrop(card, data, DragDropEffects.Move);

        if (_dragAdorner is not null && _adornerLayer is not null)
        {
            _adornerLayer.Remove(_dragAdorner);
            _dragAdorner = null;
            card.Visibility = Visibility.Visible;
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