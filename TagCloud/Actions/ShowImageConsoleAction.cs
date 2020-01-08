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
            Application.Exit();
            return config.Visualization.GetAndDrawRectangles(settings.ImageSettings, settings.PathToRead)
                .Then(image => RunApplication(image, config))
                .OnFail(error => Result.Fail<None>(error));
        }

        private void RunApplication(Bitmap image, ClientConfig config)
        {
            var thread = new Thread(() =>
            {
                Application.EnableVisualStyles();
                var showImageForm = new ShowImageForm(image);
                config.IsRunning = true;
                Application.Run(showImageForm);
            });
            thread.Start();
        }
    }
}