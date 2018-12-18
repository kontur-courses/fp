using System.Windows.Forms;

namespace TagsCloudVisualization
{
    public class ConsoleApplicationRunner : IApplicationRunner
    {
        private ConsoleApplication app;

        public ConsoleApplicationRunner(ConsoleApplication application)
        {
            app = application;
        }

        public void Run(string[] args)
        {
            var result = app.GenerateImage(args);
            MessageBox.Show(result.IsSuccess ? "OK" : "Error: " + result.Error);
        }
    }
}
