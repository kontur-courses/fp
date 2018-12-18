using Autofac;
using TagCloud.Reader;
using TagCloud.Reader.FormatReader;

namespace TagCloud.Data.ContainerModules
{
    public class ReadersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TextFileReader>().As<IWordsFileReader>();
            builder.RegisterType<DocxReader>().As<IFormatReader>();
        }
    }
}