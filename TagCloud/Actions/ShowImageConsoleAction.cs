using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ResultOf;
using TagCloud.Models;

namespace TagCloud.Actions
{
    public class ShowImageConsoleAction : IConsoleAction
    {
        private UserSettings lastSettings;
        public string CommandName { get; } = "-showimage";
        
        public string Description { get; } = "display image";

        public Result<None> Perform(ClientConfig config, UserSettings settings)
        {
            if (config.ImageToSave is null || !settings.Equals(lastSettings) )
            {
                if (!(config.ImageToSave is null))
                    config.ImageToSave.Dispose();
                var createImageResult =
                    config.Visualization.GetAndDrawRectangles(settings.ImageSettings, settings.PathToRead);
                if (!createImageResult.IsSuccess)
                    return Result.Fail<None>(createImageResult.Error);
                config.ImageToSave = createImageResult.GetValueOrThrow();
            }

            Application.Exit();
            var thread = new Thread(() =>
            {
                Application.EnableVisualStyles();
                var showImageForm = new ShowImageForm(config.ImageToSave);
                config.IsRunning = true;
                Application.Run(showImageForm);
            });
            thread.Start();
            lastSettings = settings.Clone() as UserSettings;
            return Result.Ok();
        }
    }
}