using System.Windows;

namespace BoardifyApp;

public class AppSettings
{
    public double WindowWidth { get; set; }
    public double WindowHeight { get; set; }
    public double WindowTop { get; set; }
    public double WindowLeft { get; set; }
    public WindowState WindowState { get; set; }

    public Dictionary<string, double> ColumnWidths { get; set; } = [];
}