using System.Windows.Forms;
using ResultOf;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class NewImageAction : IAction
    {
        public string CommandName { get; } = "-newimage";
        public string Description { get; } = "set parameters for a new image";

        public Result<None> Perform(ClientConfig config, UserSettings settings)
        {
            Application.Exit();
            settings = new UserSettings();
            return Result.Ok();
        }
    }
}