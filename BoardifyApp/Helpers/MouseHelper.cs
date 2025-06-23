using System.Runtime.InteropServices;

namespace BoardifyApp.Helpers;

public static class MouseHelper
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int X;
        public int Y;

        public static implicit operator System.Windows.Point(Point point)
        {
            return new System.Windows.Point(point.X, point.Y);
        }
    }

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out Point lpPoint);

    public static System.Windows.Point GetCursorPosition()
    {
        return GetCursorPos(out var lpPoint) ? lpPoint : new System.Windows.Point(0, 0);
    }
}