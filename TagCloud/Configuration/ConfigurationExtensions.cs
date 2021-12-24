using TagCloud.Templates;

namespace TagCloud.Configuration;

internal static class ConfigurationExtensions
{
    public static TemplateConfiguration ToTemplateConfiguration(this Configuration configuration)
    {
        return new TemplateConfiguration(configuration.FontFamily, configuration.BackgroundColor,
            configuration.ImageSize);
    }
}