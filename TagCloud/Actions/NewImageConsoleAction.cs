using System.Windows.Forms;
using ResultOf;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class NewImageConsoleAction : IConsoleAction
    {
        public string CommandName { get; } = "-newimage";
        public string Description { get; } = "обновить пользовательские настройки";

        public Result<None> Perform(ClientConfig config, UserSettings settings)
        {
            return Result.OfAction(Application.Exit)
                .Then(result =>settings.MakeDefault());
        }
    }
}