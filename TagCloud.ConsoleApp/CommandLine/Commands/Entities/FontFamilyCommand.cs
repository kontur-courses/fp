using Aspose.Drawing;
using TagCloud.ConsoleApp.CommandLine.Commands.Interfaces;
using TagCloud.Domain.Settings;
using TagCloud.Utils.Extensions;
using TagCloud.Utils.ResultPattern;

namespace TagCloud.ConsoleApp.CommandLine.Commands.Entities;

public class FontFamilyCommand : ICommand
{
    private readonly VisualizerSettings _visualizerSettings;

    public FontFamilyCommand(VisualizerSettings visualizerSettings)
    {
        _visualizerSettings = visualizerSettings;
    }
    
    public string Trigger => "fontfamily";
    
    public Result<bool> Execute(string[] parameters)
    {
        return Result
            .OfAction(() =>
            {
                var a = parameters[0];
                Console.WriteLine(a.Length);
            })
            .ReplaceError(_ => GetHelp())
            .Then(_ => string.Join(" ", parameters).ParseFontFamily())
            .Then(ff => _visualizerSettings.Font = new Font(ff, _visualizerSettings.Font.Size))
            .Then(() => false);
    }

    public string GetHelp()
    {
        return GetShortHelp() + Environment.NewLine +
               "Параметры:\n" +
               "string - название шрифта\n" +
               $"Актуальное значение: {_visualizerSettings.Font.FontFamily.Name}\n" +
               "Доступные шрифты в системе: " + string.Join(", ", FontFamily.Families.Select(f => f.Name));
    }

    public string GetShortHelp()
    {
        return Trigger + " позволяет настраивать начертание шрифта";
    }
}