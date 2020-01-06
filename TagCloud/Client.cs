using System;
using System.Collections.Generic;
using System.Linq;
using ResultOf;
using TagCloud.IServices;
using TagCloud.Models;

namespace TagCloud
{
    public class Client : IClient
    {
        private readonly IConsoleAction[] actions;
        private readonly ICloudVisualization visualization;
        private readonly HashSet<string> availableFontNames;
        private HashSet<string> availablePaletteNames;
        private Dictionary<string, IConsoleAction> actionsDictionary;
        private ClientConfig config;

        public Client(IConsoleAction[] actions, IPaletteNamesFactory paletteNamesFactory, ICloudVisualization visualization)
        {
            availableFontNames = new HashSet<string>
            {
                "Arial",
                "Comic Sans MS"
            };
            this.actions = actions;
            this.visualization = visualization;
            availablePaletteNames = paletteNamesFactory.GetPaletteNames(visualization);
            actionsDictionary = actions.ToDictionary(a => a.CommandName, a => a);
        }

        public void Start()
        {
            config = new ClientConfig(availableFontNames, availablePaletteNames,visualization);
            var userSettings = UserSettings.GetDefaultUserSettings();
            actionsDictionary["-help"].Perform(config,userSettings);
            while (!config.ToExit)
            {
                Console.WriteLine("Введите команду");
                    Console.Write(">>>");
                    var command = Console.ReadLine().ToLower();
                    var commandResult = GetCommandResult(command, actionsDictionary);
                    var perform = commandResult.Value.Perform(config, userSettings);
                    if(!perform.IsSuccess)
                        Console.WriteLine(perform.Error);
            }
        }

        private Result<IConsoleAction> GetCommandResult(string commandName, Dictionary<string, IConsoleAction> availableCommands)
        {
            return availableCommands.ContainsKey(commandName)
                ? Result.Ok(availableCommands[commandName])
                : Result.Fail<IConsoleAction>("Unknown command");
        }
    }
}