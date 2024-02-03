using TagCloud.ConsoleApp.CommandLine.Commands.Interfaces;
using TagCloud.ConsoleApp.CommandLine.Interfaces;
using TagCloud.Utils.ResultPattern;

namespace TagCloud.ConsoleApp.CommandLine;

public class CommandService : ICommandService
{
    private readonly Dictionary<string, ICommand> _commands = new();
    
    public CommandService(IEnumerable<ICommand> commands)
    {
        foreach (var command in commands) 
            _commands[command.Trigger] = command;
    }

    public void Run()
    {
        Console.WriteLine("Введите команду");
        var input = Console.ReadLine();

        while (true)
        {
            var result = Execute(input);
            
            switch (result.IsSuccess)
            {
                case true when result.Value:
                    return;
                case false:
                    Console.WriteLine(result.Error);
                    break;
            }

            Console.WriteLine(Environment.NewLine + "Введите команду");
            input = Console.ReadLine();
        }
    }

    public IEnumerable<ICommand> GetCommands()
    {
        return _commands.Values;
    }

    public bool TryGetCommand(string name, out ICommand command)
    {
        return _commands.TryGetValue(name, out command);
    }

    private Result<bool> Execute(string input)
    {
        var parameters = input.Split(' ');
        
        if (parameters.Length == 0 
            || !_commands.TryGetValue(parameters[0], out var command)
            || parameters.Length == 2 && parameters[1] == "--help")
            return _commands["help"].Execute(parameters);

        return command.Execute(parameters
            .Skip(1)
            .Where(e => e != "")
            .ToArray());
    }
}