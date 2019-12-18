using System;
using System.Windows.Forms;
using ErrorHandler;
using TagsCloudVisualization.Services;

namespace TagsCloudVisualization.UI.Actions
{
    public class CreateCloudAction : IUiAction
    {
        private readonly IVisualizer visualizer;
        private readonly IImageHolder imageHolder;
        private readonly IDocumentPathProvider pathProvider;
        private readonly IUiErrorHandler errorHandler;
        public string Name { get; }

        public CreateCloudAction(
            IVisualizer visualizer,
            IDocumentPathProvider pathProvider,
            IImageHolder imageHolder,
            IUiErrorHandler errorHandler)
        {
            this.errorHandler = errorHandler;
            this.imageHolder = imageHolder;
            this.visualizer = visualizer;
            this.pathProvider = pathProvider;
            Name = "Create";
        }

        public void Perform(object sender, EventArgs e)
        {
            var button = sender as Button;
            button.Hide();
            pathProvider
                .GetPath()
                .Then(path => visualizer.VisualizeTextFromFile(path))
                .Then(image => imageHolder.SetImage(image))
                .RefineError("Couldn't create tag cloud")
                .OnFail(errorHandler.PostError);
            button.Show();
        }
    }
}