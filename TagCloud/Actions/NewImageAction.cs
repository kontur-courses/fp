using System.Windows.Forms;
using ResultOf;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class NewImageAction : IAction
    {
        public string CommandName { get; } = "-newimage";
        public string Description { get; } = "обновить пользовательские настройки";

        public Result<None> Perform(ClientConfig config,UserSettings settings)
        {
            Application.Exit();
            settings.MakeDefault();
            return Result.Ok();
        }
    }
}