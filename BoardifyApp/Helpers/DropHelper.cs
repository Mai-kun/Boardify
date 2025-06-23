using System.Windows;

namespace BoardifyApp.Helpers;

public static class DropHelper
{
    public static readonly DependencyProperty IsDropTargetProperty =
        DependencyProperty.RegisterAttached(
            "IsDropTarget",
            typeof(bool),
            typeof(DropHelper),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender)
        );

    public static void SetIsDropTarget(UIElement element, bool value)
    {
        element.SetValue(IsDropTargetProperty, value);
    }

    public static bool GetIsDropTarget(UIElement element)
    {
        return (bool)element.GetValue(IsDropTargetProperty);
    }
}