using TagCloud.ConsoleApp.CommandLine.Commands.Interfaces;
using TagCloud.Domain.Settings;
using TagCloud.Utils.Extensions;
using TagCloud.Utils.ResultPattern;

namespace TagCloud.ConsoleApp.CommandLine.Commands.Entities;

public class ColorCommand : ICommand
{
    private readonly VisualizerSettings _visualizerSettings;

    public ColorCommand(VisualizerSettings visualizerSettings)
    {
        _visualizerSettings = visualizerSettings;
    }
    
    public string Trigger => "color";
    
    public Result<bool> Execute(string[] parameters)
    {
        return Result
            .Of(() => (int.Parse(parameters[0]), int.Parse(parameters[1]), int.Parse(parameters[2])))
            .ReplaceError(_ => GetHelp())
            .Then(parsed => parsed.ParseColor())
            .Then(color => _visualizerSettings.Color = color)
            .Then(() => false);
    }

    public string GetHelp()
    {
        return GetShortHelp() + Environment.NewLine +
               "Параметры:\n" +
               "int - red channel\n" +
               "int - green channel\n" +
               "int - blue channel\n" +
               $"Актуальное значение {_visualizerSettings.Color}";
    }
    
    public string GetShortHelp()
    {
        return Trigger + " позволяет настраивать цвет шрифта";
    }
}