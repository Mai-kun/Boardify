using System.Globalization;
using System.Resources;
using System.Windows.Markup;

namespace BoardifyApp.Helpers;

public class LocExtension(string key) : MarkupExtension
{
    private static readonly ResourceManager ResourceManager = Resources.en_lang.ResourceManager;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var culture = CultureInfo.CurrentUICulture;
        return ResourceManager.GetString(key, culture) ?? $"!{key}!";
    }
}