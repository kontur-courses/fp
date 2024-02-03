using TagCloud.ConsoleApp.CommandLine.Commands.Interfaces;
using TagCloud.Domain.Settings;
using TagCloud.Utils.ResultPattern;

namespace TagCloud.ConsoleApp.CommandLine.Commands.Entities;

public class FromCommand : ICommand
{
    private readonly FileSettings _fileSettings;

    public FromCommand(FileSettings fileSettings)
    {
        _fileSettings = fileSettings;
    }
    
    public string Trigger => "from";
    
    public Result<bool> Execute(string[] parameters)
    {
        return Result
            .Of(() => parameters[0])
            .ReplaceError(_ => GetHelp())
            .Then(path => _fileSettings.FileFromWithPath = path)
            .Then(() => false);
    }

    public string GetHelp()
    {
        return GetShortHelp() + Environment.NewLine +
               "Параметры:\n" +
               "string - pathToFIle\n" +
               $"Актуальное значение {_fileSettings.FileFromWithPath}";
    }

    public string GetShortHelp()
    {
        return Trigger + " позволяет указывать файл, из которого брать слова для облака тегов";
    }
}