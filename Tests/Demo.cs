using System;
using System.Drawing;
using System.IO;
using Autofac;
using Autofac.Core;
using NUnit.Framework;
using WordCloudGenerator;

namespace Tests
{
    [TestFixture]
    public class Demo
    {
        private IContainer container;

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Preparer>().As<IPreparer>()
                .WithParameter("filter", new Func<string, bool>(str => str.Length >= 3));
            builder.RegisterType<Painter>().As<IPainter>();
            builder.RegisterInstance(FontFamily.GenericSansSerif).As<FontFamily>();

            builder.RegisterType<OrderedChoicePalette>().As<IPalette>();
            builder.RegisterType<CircularLayouter>().As<ILayouter>();

            container = builder.Build();
        }

        [Test]
        public void Demo1()
        {
            var text = Reader.ReadFile("text.txt");
            var preparer = new Preparer(new[] {"что", "если", "это", "как"}, word => word.Length > 3);
            var prepared = preparer.CreateWordFreqList(text, 200);
            var algorithm = AlgorithmFabric.Create(AlgorithmType.Exponential);
            var graphicalWords = algorithm(prepared);
            var painter = new Painter(FontFamily.GenericSansSerif, 
                new RandomChoicePalette(new[]
                    {Color.Peru, Color.Pink, Color.Green, Color.Red, Color.Blue, Color.Black}, Color.White),
                size => new CircularLayouter(size));

            var img = painter.Paint(graphicalWords);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "demo1.jpg");
            Saver.SaveImage(img, path);
            
            Console.WriteLine($"Изображение сохранено в {path}");
        }
    }
}