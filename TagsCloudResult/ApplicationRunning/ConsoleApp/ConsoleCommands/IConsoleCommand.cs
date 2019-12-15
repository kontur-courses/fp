namespace TagsCloudResult.ApplicationRunning.ConsoleApp.ConsoleCommands
{
    public interface IConsoleCommand
    {
        string Name { get; }
        string Description { get; }
        string Arguments { get; }
        Result<string[]> ParseArguments(string[] args);
        Result<None> Act();
    }
}