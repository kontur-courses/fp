using Aspose.Drawing.Imaging;
using TagCloud.ConsoleApp.CommandLine.Commands.Interfaces;
using TagCloud.Domain.Settings;
using TagCloud.Utils.Extensions;
using TagCloud.Utils.ResultPattern;

namespace TagCloud.ConsoleApp.CommandLine.Commands.Entities;

public class ImageFormatCommand : ICommand
{
    private readonly FileSettings _fileSettings;

    public ImageFormatCommand(FileSettings fileSettings)
    {
        _fileSettings = fileSettings;
    }
    
    public string Trigger => "format";
    public Result<bool> Execute(string[] parameters)
    {
        return Result
            .Of(() => parameters[0])
            .ReplaceError(_ => "Данный формат недоступен\n" + GetHelp())
            .Then(strFormat => strFormat.ConvertToImageFormat())
            .Then(format => _fileSettings.ImageFormat = format)
            .Then(() => false);
    }

    public string GetHelp()
    {
        return GetShortHelp() + Environment.NewLine +
               "Параметры:\n" +
               "string - название шрифта\n" +
               $"Актуальное значение: {_fileSettings.ImageFormat}";
    }

    public string GetShortHelp()
    {
        return Trigger + " позволяет настраивать начертание шрифта";
    }
}