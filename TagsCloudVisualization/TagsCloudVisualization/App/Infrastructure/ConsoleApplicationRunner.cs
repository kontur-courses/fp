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

        public Result<None> Run(string[] args)
        {
            return app.GenerateImage(args);
        }
    }
}
