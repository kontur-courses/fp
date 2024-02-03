using TagCloud.ConsoleApp.CommandLine.Commands.Interfaces;
using TagCloud.Utils.ResultPattern;

namespace TagCloud.ConsoleApp.CommandLine.Commands.Entities;

public class ExitCommand : ICommand
{
    public string Trigger => "exit";
    public Result<bool> Execute(string[] parameters)
    {
        return true;
    }

    public string GetHelp()
    {
        return GetShortHelp();
    }
    
    public string GetShortHelp()
    {
        return Trigger + " завершить выполнение программы";
    }
}