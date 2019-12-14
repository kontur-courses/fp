namespace TagsCloudResult.ApplicationRunning.ConsoleApp.ConsoleCommands
{
    public interface IConsoleCommand
    {
        Result<string[]> ParseArguments(string[] args);
        void Act();
        string Name { get; }
        string Description { get; }
        string Arguments { get; }
    }
}