using System.Drawing;
using TagsCloudVisualization.ResultOf;
using TagsCloudVisualization.TextHandlers;
using TagsCloudVisualization.Visualization;

namespace TagsCloudVisualization
{
    public class TagCloudCreator : ITagCloudCreator
    {
        private readonly ITextHandler handler;
        private readonly IVisualizer visualizer;

        public TagCloudCreator(IVisualizer visualizer, ITextHandler handler)
        {
            this.visualizer = visualizer;
            this.handler = handler;
        }
        
        public Result<Bitmap> CreateFromFile(string filepath)
        {
            return handler.Handle(filepath)
                .Then(visualizer.Visualize);
        }
    }
}