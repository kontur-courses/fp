using TagCloud.Templates;

namespace TagCloudApp.Configurations;

public static class ConfigurationExtensions
{
    public static TemplateConfiguration ToTemplateConfiguration(this Configuration configuration)
    {
        return new TemplateConfiguration(configuration.FontFamily, configuration.BackgroundColor,
            configuration.ImageSize);
    }
}