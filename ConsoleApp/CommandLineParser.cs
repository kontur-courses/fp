using CommandLine;
using ConsoleApp.Handlers;
using ConsoleApp.Options;

namespace ConsoleApp;

public class CommandLineParser: ICommandLineParser
{
    private readonly IOptionsHandler[] handlers;
    private readonly IOptions[] options;

    public CommandLineParser(IOptionsHandler[] handlers, IOptions[] options)
    {
        this.handlers = handlers;
        this.options = options;
    }

    public void ParseFromConsole()
    {
        var types = options
            .Select(opt => opt.GetType())
            .ToArray();
        
        Console.WriteLine("Доступные команды '--help'");
        while (true)
        {
            var input = Console.ReadLine();
            var args = input.Split();
            Parser.Default.ParseArguments(args, types)
                .WithParsed<IOptions>(WithParsed);
        }
    }

    private void WithParsed(IOptions option)
    {
        string message;
        var handler = handlers.FirstOrDefault(h => h.CanParse(option));
        if (handler is null)
        {
            message = "Обработчик не найден";
        }
        else
        {
            var result = handler.WithParsed(option);
            message = result.IsSuccess
                ? result.Value
                : result.Error;
        }

        Console.WriteLine(message);
    }
}