using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ResultOf;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class ShowImageConsoleAction : IConsoleAction
    {
        public string CommandName { get; } = "-showimage";
        
        public string Description { get; } = "display image";

        public Result<None> Perform(ClientConfig config, UserSettings settings)
        {
            var createImageResult =
                    config.Visualization.GetAndDrawRectangles(settings.ImageSettings, settings.PathToRead);
                if (!createImageResult.IsSuccess)
                    return Result.Fail<None>(createImageResult.Error);

                Application.Exit();
            var thread = new Thread(() =>
            {
                Application.EnableVisualStyles();
                var showImageForm = new ShowImageForm(createImageResult.Value);
                config.IsRunning = true;
                Application.Run(showImageForm);
            });
            thread.Start();
            return Result.Ok();
        }
    }
}