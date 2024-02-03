using TagCloud.ConsoleApp.CommandLine.Commands.Interfaces;
using TagCloud.Domain.Settings;
using TagCloud.Utils.ResultPattern;

namespace TagCloud.ConsoleApp.CommandLine.Commands.Entities;

public class BigToCenterCommand : ICommand
{
    private readonly LayoutSettings _layoutSettings;
    
    public BigToCenterCommand(LayoutSettings layoutSettings)
    {
        _layoutSettings = layoutSettings;
    }
    
    public string Trigger => "bigcenter";
    
    public Result<bool> Execute(string[] parameters)
    {
        return Result
            .Of(() => int.Parse(parameters[0]))
            .Then(parsed => parsed == 0 || parsed == 1 ? Result.Fail<int>("") : parsed.AsResult())
            .ReplaceError(_ => GetHelp())
            .Then(parsed => _layoutSettings.BigToCenter = parsed == 1)
            .Then(() => false);
    }

    public string GetHelp()
    {
        return GetShortHelp() + Environment.NewLine +
               "Параметры:\n" +
               "int - 1(ближе к центру) или 0(в случайном порядке)\n" +
               $"Актуальное значение {_layoutSettings.BigToCenter}";
    }
    
    public string GetShortHelp()
    {
        return Trigger + " позволяет настраивать положение более частых слов";
    }
}