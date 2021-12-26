using System;
using System.Drawing;
using Autofac;
using DeepMorphy;
using TagsCloudVisualization;
using TagsCloudVisualization.Interfaces;

namespace ConsoleClient
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(typeof(TagCloudCreator).Assembly).AsImplementedInterfaces();

            var cloudLayouter = new CircularCloudLayouter(new Point(800, 800));
            using var bitmap = new Bitmap(1600, 1600);
            using var graphics = Graphics.FromImage(bitmap);
            using var pen = new Pen(Color.Red);
            using var font = new Font(FontFamily.GenericMonospace, 12);

            builder.RegisterInstance(cloudLayouter).As<ICloudLayouter>();
            builder.RegisterInstance(new MorphAnalyzer());
            builder.RegisterInstance(bitmap);
            builder.RegisterInstance(graphics);
            builder.RegisterInstance(pen);
            builder.RegisterInstance(font);

            var container = builder.Build();

            var cloudCreator = container.Resolve<ITagCloudCreator>();
            var resultImage = cloudCreator.CreateTagsCloudBitmapFromTextFile("Sample.txt")
                .OnFail(Console.WriteLine);

            if (!resultImage.IsSuccess) 
                return;
            try
            
            {
                resultImage.GetValueOrThrow().Save("Sample.png");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}