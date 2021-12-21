using Mono.Options;
using ResultExtensions;
using ResultOf;
using TagsCloudContainer.Abstractions;

namespace TagsCloudContainer.Defaults.SettingsProviders;

public class OutputSettings : ICliSettingsProvider
{
    private const string defaultOutput = "image.png";
    private const ImageFormatType defaultType = ImageFormatType.Png;

    public string OutputPath { get; private set; } = defaultOutput;
    public ImageFormatType ImageFormat { get; private set; } = defaultType;

    public Result State { get; private set; }
    public OptionSet GetCliOptions()
    {
        var options = new OptionSet()
            {
                { "output=", $"Name of the output image. Defaults to '{defaultOutput}'", v => OutputPath = v },
                { "image-format=", $"Format of the output image. Defaults to {defaultType}",
                    v => State = Parse(v).Then(type => ImageFormat = type) }
            };

        return options;
    }

    private static Result<ImageFormatType> Parse(string v)
    {
        return Enum.TryParse<ImageFormatType>(v, true, out var type) ? type : Result.Fail<ImageFormatType>($"Could not parse {v} as {nameof(ImageFormatType)}");
    }
}