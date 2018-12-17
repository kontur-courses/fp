using ResultOf;
using TagsCloudVisualization.Layouter;
using TagsCloudVisualization.Visualizer;

namespace TagsCloudVisualization
{
    public class CloudSaver : ISaver<IVisualizer<IWordsCloudBuilder>>
    {
        private readonly IVisualizer<IWordsCloudBuilder> visualizer;

        public CloudSaver(IVisualizer<IWordsCloudBuilder> visualizer)
        {
            this.visualizer = visualizer;
        }

        public Result<None> Save(string filename)
        {
            return visualizer.Draw()
                .Then(bmp => bmp.Save(filename));
        }
    }
}
