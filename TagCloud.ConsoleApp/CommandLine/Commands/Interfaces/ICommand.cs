using TagCloud.Utils.ResultPattern;

namespace TagCloud.ConsoleApp.CommandLine.Commands.Interfaces;

public interface ICommand
{
    public string Trigger { get; }
    
    public Result<bool> Execute(string[] parameters);
    public string GetHelp();
    public string GetShortHelp();
}