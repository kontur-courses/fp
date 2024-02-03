using Aspose.Drawing;
using TagCloud.ConsoleApp.CommandLine.Commands.Interfaces;
using TagCloud.Domain.Settings;
using TagCloud.Utils.ResultPattern;

namespace TagCloud.ConsoleApp.CommandLine.Commands.Entities;

public class SizeCommand : ICommand
{
    private readonly LayoutSettings _layoutSettings;
    
    public SizeCommand(LayoutSettings layoutSettings)
    {
        _layoutSettings = layoutSettings;
    }
    
    public string Trigger => "size";
    
    public Result<bool> Execute(string[] parameters)
    {
        return Result
            .Of(() => (int.Parse(parameters[0]), int.Parse(parameters[1])))
            .ReplaceError(_ => GetHelp())
            .Then(parsed => _layoutSettings.Dimensions = new Size(parsed.Item1, parsed.Item2))
            .Then(() => false);
    }

    public string GetHelp()
    {
        return GetShortHelp() + Environment.NewLine +
               "Параметры:\n" +
               "int - width\n" +
               "int - height\n" +
               $"Актуальное значение {_layoutSettings.Dimensions}";
    }

    public string GetShortHelp()
    {
        return Trigger + " позволяет настраивать размер выходного изображения";
    }
}