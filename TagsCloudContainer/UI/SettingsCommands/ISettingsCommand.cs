using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TagsCloudContainer.UI.SettingsCommands
{
    public interface ISettingsCommand
    {
        string Name { get; }
        Result<string[]> GetArguments(string input);
        Result<IInitialSettings> TryChangeSettings(string[] arguments, IInitialSettings settings);
    }
}
