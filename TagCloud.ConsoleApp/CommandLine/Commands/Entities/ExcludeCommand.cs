using TagCloud.ConsoleApp.CommandLine.Commands.Interfaces;
using TagCloud.Domain.Settings;
using TagCloud.Utils.ResultPattern;

namespace TagCloud.ConsoleApp.CommandLine.Commands.Entities;

public class ExcludeCommand : ICommand
{
    private readonly WordSettings _wordSettings;
    
    public ExcludeCommand(WordSettings wordSettings)
    {
        _wordSettings = wordSettings;
    }
    
    public string Trigger => "exclude";
    
    public Result<bool> Execute(string[] parameters)
    {
        return parameters.Length < 1
            ? Result.Fail<bool>(GetHelp())
            : Result
                .OfAction(() => _wordSettings.Excluded.AddRange(parameters))
                .Then(() => false);
    }

    public string GetHelp()
    {
        return GetShortHelp() + Environment.NewLine +
               "Параметры:\n" +
               "string[] - список слов, которые надо исключить через пробел\n" +
               "Сейчас исключено " + string.Join(", ", _wordSettings.Excluded);
    }
    
    public string GetShortHelp()
    {
        return Trigger + " позволяет исключить слова из облака тегов";
    }
}