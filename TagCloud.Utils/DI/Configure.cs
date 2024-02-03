using Autofac;
using TagCloud.Utils.Files;
using TagCloud.Utils.Files.Interfaces;
using TagCloud.Utils.Images;
using TagCloud.Utils.Images.Interfaces;
using TagCloud.Utils.Words;
using TagCloud.Utils.Words.Interfaces;

namespace TagCloud.Utils.DI;

public class Configure
{
    public static void ConfigureUtils(ContainerBuilder builder)
    {
        builder
            .RegisterType<WordsService>()
            .As<IWordsService>()
            .SingleInstance();

        builder
            .RegisterType<ImageWorker>()
            .As<IImageWorker>()
            .SingleInstance();

        builder
            .RegisterType<FileService>()
            .As<IFileService>()
            .SingleInstance();
    }
}