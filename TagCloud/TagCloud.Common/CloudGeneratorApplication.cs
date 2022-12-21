using Autofac;
using ResultOf;
using TagCloud.Common.DI;
using TagCloud.Common.Options;
using TagCloud.Common.Saver;

namespace TagCloud.Common;

public class CloudGeneratorApplication
{
    public static Result<None> Run(VisualizationOptions visualizationOptions)
    {
        var container = CloudContainerBuilder.CreateContainer(visualizationOptions);
        var cloudCreator = container.Resolve<TagCloudCreator>();
        var cloudSaver = container.Resolve<ICloudSaver>();
        var imageResult = cloudCreator.CreateCloud(visualizationOptions.WordsOptions);
        return !imageResult.IsSuccess
            ? Result.Fail<None>($"Cloud doesn't created. {imageResult.Error}")
            : cloudSaver.SaveCloud(imageResult.Value);
    }
}