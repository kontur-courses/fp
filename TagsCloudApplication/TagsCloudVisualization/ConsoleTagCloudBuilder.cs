using TextConfiguration;

namespace TagsCloudVisualization
{
    public class ConsoleTagCloudBuilder
    {
        private readonly ITagProvider tagProvider;
        private readonly ITagCloudVisualizator visualizator;
        private readonly ITagCloudBuilderProperties properties;

        public ConsoleTagCloudBuilder(
            ITagProvider tagProvider, 
            ITagCloudVisualizator visualizator,
            ITagCloudBuilderProperties properties)
        {
            this.tagProvider = tagProvider;
            this.visualizator = visualizator;
            this.properties = properties;
        }

        public Result<None> Run()
        {
            return 
                tagProvider.ReadCloudTags(properties.InputFilename)
                .Then(cloudTags => visualizator.VisualizeCloudTags(cloudTags))
                .Then(image => ImageUtilities.SaveImage(image, properties.OutputFormat, properties.OutputFilename));
        }
    }
}
