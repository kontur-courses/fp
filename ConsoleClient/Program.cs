#region

using System.Drawing;
using Autofac;
using DeepMorphy;
using TagsCloudVisualization;
using TagsCloudVisualization.Interfaces;

#endregion

namespace ConsoleClient
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            var assemblyInfo = TagCloudCreator.GetAssemblyInfo();
            builder.RegisterAssemblyTypes(assemblyInfo)
                .AsImplementedInterfaces();

            var cloudLayouter = new CircularCloudLayouter(new Point(800, 800));
            var bitmap = new Bitmap(1600, 1600);
            var graphics = Graphics.FromImage(bitmap);

            builder.RegisterInstance(cloudLayouter).As<ICloudLayouter>();
            builder.RegisterInstance(new MorphAnalyzer());
            builder.RegisterInstance(bitmap);
            builder.RegisterInstance(graphics);
            builder.RegisterInstance(new Pen(Color.Red));
            builder.RegisterInstance(new Font(FontFamily.GenericMonospace, 12));

            var container = builder.Build();

            var cloudCreator = container.Resolve<ITagCloudCreator>();
            var image = cloudCreator.CreateAndSaveCloudFromTextFile("Sample.txt");
            image.GetValueOrThrow().Save("Sample.png");
        }
    }
}