using TagsCloudContainer.CloudGenerators;
using TagsCloudContainer.FileProviders;
using TagsCloudContainer.TextAnalysers;
using TagsCloudContainer.Visualizers;

namespace TagsCloudContainer;

public class TagsCloudContainer : ITagsCloudContainer
{
    private readonly IFileReader fileReader;
    private readonly ICloudVisualizer visualizer;
    private readonly ITagsCloudGenerator cloudGenerator;
    private readonly ITextPreprocessor textPreprocessor;
    private readonly IImageProvider imageProvider;

    public TagsCloudContainer(ITagsCloudGenerator cloudGenerator, ICloudVisualizer visualizer,
        ITextPreprocessor textPreprocessor, IFileReader fileReader, IImageProvider imageProvider)
    {
        this.cloudGenerator = cloudGenerator;
        this.visualizer = visualizer;
        this.textPreprocessor = textPreprocessor;
        this.fileReader = fileReader;
        this.imageProvider = imageProvider;
    }

    public Result<None> GenerateImageToFile(string inputFile, string outputFile)
    {
        return fileReader.ReadFile(inputFile)
            .Then(textPreprocessor.Preprocess)
            .Then(cloudGenerator.Generate)
            .Then(visualizer.DrawImage)
            .Then(image => imageProvider.SaveImage(image, outputFile));
    }
}