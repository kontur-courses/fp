using System;
using ErrorHandler;
using TagsCloudVisualization.Services;

namespace TagsCloudVisualization.UI.Actions
{
    public class CreateCloudAction : IUiAction
    {
        private readonly IVisualizer visualizer;
        private readonly IImageHolder imageHolder;
        private readonly IDocumentPathProvider pathProvider;
        public string Name { get; }

        public CreateCloudAction(
            IVisualizer visualizer,
            IDocumentPathProvider pathProvider,
            IImageHolder imageHolder)
        {
            this.imageHolder = imageHolder;
            this.visualizer = visualizer;
            this.pathProvider = pathProvider;
            Name = "Create";
        }

        public void Perform(object sender, EventArgs e)
        {
            pathProvider
                .GetPath()
                .Then(path => visualizer.VisualizeTextFromFile(path))
                .Then(image => imageHolder.SetImage(image));
        }
    }
}