using Aspose.Drawing;
using TagCloud.ConsoleApp.CommandLine.Commands.Interfaces;
using TagCloud.Domain.Settings;
using TagCloud.Utils.ResultPattern;

namespace TagCloud.ConsoleApp.CommandLine.Commands.Entities;

public class FontSizeCommand : ICommand
{
    private readonly VisualizerSettings _visualizerSettings;

    public FontSizeCommand(VisualizerSettings visualizerSettings)
    {
        _visualizerSettings = visualizerSettings;
    }

    public string Trigger => "fontsize";
    
    public Result<bool> Execute(string[] parameters)
    {
        return Result.Of(() => float.Parse(parameters[0]))
            .ReplaceError(_ => GetHelp())
            .Then(fz => _visualizerSettings.Font = new Font(_visualizerSettings.Font.FontFamily, fz))
            .Then(() => false);
    }

    public string GetHelp()
    {
        return GetShortHelp() + Environment.NewLine +
               "Параметры:\n" +
               "float - размер шрифта\n" +
               $"Актуальное значение {_visualizerSettings.Font.Size}";
    }

    public string GetShortHelp()
    {
        return Trigger + " позволяет настраивать размер шрифта";
    }
}