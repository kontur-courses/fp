using TagCloud.ConsoleApp.CommandLine.Commands.Interfaces;
using TagCloud.Domain.Settings;
using TagCloud.Utils.ResultPattern;

namespace TagCloud.ConsoleApp.CommandLine.Commands.Entities;

public class PathToFileCommand : ICommand
{
    private readonly FileSettings _fileSettings;

    public PathToFileCommand(FileSettings fileSettings)
    {
        _fileSettings = fileSettings;
    }
    
    public string Trigger => "path";
    
    public Result<bool> Execute(string[] parameters)
    {
        return Result
            .Of(() => parameters[0])
            .ReplaceError(_ => GetHelp())
            .Then(path => _fileSettings.OutPathToFile = path)
            .Then(() => false);
    }

    public string GetHelp()
    {
        return GetShortHelp() + Environment.NewLine +
               "Параметры:\n" +
               "string - pathToFile\n" +
               $"Актуальное значение {_fileSettings.OutPathToFile}";
    }

    public string GetShortHelp()
    {
        return Trigger + " позволяет настраивать путь для сохранения файла";
    }
}