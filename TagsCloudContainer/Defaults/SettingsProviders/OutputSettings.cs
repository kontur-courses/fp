using Mono.Options;
using TagsCloudContainer.Abstractions;

namespace TagsCloudContainer.Defaults.SettingsProviders;

public class OutputSettings : ICliSettingsProvider
{
    private const string defaultOutput = "image.png";
    private const ImageFormatType defaultType = ImageFormatType.Png;

    public string OutputPath { get; private set; } = defaultOutput;
    public ImageFormatType ImageFormat { get; private set; } = defaultType;
    public OptionSet GetCliOptions()
    {
        var options = new OptionSet()
            {
                { "output=", $"Name of the output image. Defaults to '{defaultOutput}'", v => OutputPath = v },
                { "image-format=", $"Format of the output image. Defaults to {defaultType}", (ImageFormatType v) => ImageFormat = v }
            };

        return options;
    }
}