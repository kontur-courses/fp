using System.Drawing;
using TagCloud;
using Result;

namespace ConsoleApp;

public class ApplicationPropertiesSetuper
{
    private readonly ConsoleOptions consoleOptions;

    public ApplicationPropertiesSetuper(ConsoleOptions consoleOptions)
    {
        this.consoleOptions = consoleOptions;
    }

    public Result<ApplicationProperties> Setup(IWordsParser wordsParser)
    {
        var properties = new ApplicationProperties();
        SetupApplySizeOption(properties.SizeProperties, properties.CloudProperties);
        SetupFontOption(properties.FontProperties);
        var path = SetupInputFileOptions();
        if (!path.IsSuccess)
            return new Result<ApplicationProperties>(null, path.Error);
        properties.Path = path;

        var cloudProperties = properties.CloudProperties;
        cloudProperties.Density = consoleOptions.Density;
        if (consoleOptions.ExcludedWords is not null)
        {
            var excludedWords = wordsParser.Parse(consoleOptions.ExcludedWords);
            if (!excludedWords.IsSuccess || excludedWords.Value is null)
                return new Result<ApplicationProperties>(null, excludedWords.Error);
            cloudProperties.ExcludedWords = excludedWords.Value;
        }
        
        var palette = properties.Palette;
        palette.Background = ColorTranslator.FromHtml(consoleOptions.BackgroundColor);
        palette.Foreground = ColorTranslator.FromHtml(consoleOptions.ForegroundColor);

        return new Result<ApplicationProperties>(properties);
    }

    private void SetupApplySizeOption(SizeProperties sizeProperties, CloudProperties cloudProperties)
    {
        sizeProperties.ImageSize = new Size(consoleOptions.Width, consoleOptions.Height);
        cloudProperties.Center = sizeProperties.ImageCenter;
    }

    private void SetupFontOption(FontProperties fontProperties)
    {
        fontProperties.Family = new FontFamily(consoleOptions.FontName);
        fontProperties.MinSize = consoleOptions.MinFont;
        fontProperties.MaxSize = consoleOptions.MaxFont;
    }

    private Result<string> SetupInputFileOptions()
    {
        if (consoleOptions.File is null)
            return new Result<string>(null, "Input file not set");
        
        return File.Exists(consoleOptions.File) 
            ? new Result<string>(consoleOptions.File)
            : new Result<string>(null, "Input file not exists");
    }
}