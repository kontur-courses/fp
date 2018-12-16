using Autofac;
using TagsCloudResult.Algorithms;
using TagsCloudResult.DataProviders;
using TagsCloudResult.ResultFormatters;
using TagsCloudResult.Settings;
using TagsCloudResult.SourceTextReaders;
using TagsCloudResult.TextPreprocessors;
using TagsCloudResult.TextPreprocessors.Filters;

namespace TagsCloudResult
{
    public class Ioc
    {
        public IContainer GetContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DefaultCloudSettings>().As<ICloudSettings>();
            builder.RegisterType<DefaultSourceFileSettings>().As<ISourceFileSettings>();
            builder.RegisterType<DefaultFontSettings>().As<IFontSettings>();
            builder.RegisterType<TxtSourceTextReader>().As<ISourceTextReader>();
            builder.RegisterType<BasicWordsPreprocessor>().As<IWordsPreprocessor>(); 
            builder.RegisterType<ArchimedeanSpiral>().As<ISpiral>();
            builder.RegisterType<CircularCloudAlgorithm>().As<IAlgorithm>();
            builder.RegisterType<CircularCloudLayouterResultFormatter>().As<IResultFormatter>();
            builder.RegisterType<DataProvider>().As<IDataProvider>();
            builder.RegisterTypes(typeof(BoringWordFilter), typeof(ShortWordFilter)).As<IWordFilter>();

            return builder.Build();
        }
    }
}
