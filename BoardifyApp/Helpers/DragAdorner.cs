using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace BoardifyApp.Helpers;

public class DragAdorner : Adorner
{
    private readonly UIElement _adornment;
    private double _offsetX;
    private double _offsetY;

    public DragAdorner(UIElement adornedElement, UIElement adornment) : base(adornedElement)
    {
        _adornment = adornment;
        IsHitTestVisible = false;
        AddVisualChild(_adornment);
    }

    public void SetPosition(double x, double y)
    {
        _offsetX = x;
        _offsetY = y;
        InvalidateArrange();
    }

    protected override Size MeasureOverride(Size constraint)
    {
        _adornment.Measure(constraint);
        return _adornment.DesiredSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        _adornment.Arrange(new Rect(new Point(_offsetX, _offsetY), _adornment.DesiredSize));
        return finalSize;
    }

    protected override Visual GetVisualChild(int index)
    {
        return _adornment;
    }

    protected override int VisualChildrenCount => 1;
}