using System.IO;
using System.Text.Json;

namespace BoardifyApp.Services;

public static class SettingsManager
{
    private const string FilePath = "appsettings.json";

    public static AppSettings Load()
    {
        if (!File.Exists(FilePath))
        {
            return new AppSettings();
        }

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
    }

    public static void Save(AppSettings settings)
    {
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }
}