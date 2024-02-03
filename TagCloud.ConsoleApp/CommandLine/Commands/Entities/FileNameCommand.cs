using TagCloud.ConsoleApp.CommandLine.Commands.Interfaces;
using TagCloud.Domain.Settings;
using TagCloud.Utils.ResultPattern;

namespace TagCloud.ConsoleApp.CommandLine.Commands.Entities;

public class FileNameCommand : ICommand
{
    private readonly FileSettings fileSettings;

    public FileNameCommand(FileSettings fileSettings)
    {
        this.fileSettings = fileSettings;
    }
    
    public string Trigger => "filename";
    
    public Result<bool> Execute(string[] parameters)
    {
        return Result
            .Of(() => parameters[0])
            .ReplaceError(_ => GetHelp())
            .Then(name => fileSettings.OutFileName = name)
            .Then(() => false);
    }

    public string GetHelp()
    {
        return GetShortHelp() + Environment.NewLine +
               "Параметры:\n" +
               "stirng - filename\n" +
               $"Актуальное значение {fileSettings.OutFileName}";
    }
    
    public string GetShortHelp()
    {
        return Trigger + " позволяет настраивать имя файла при сохранении облака тегов";
    }
}