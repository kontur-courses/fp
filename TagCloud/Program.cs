using System;
using System.Collections.Generic;
using System.IO;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using ResultOf;

namespace TagCloud
{
    public class Program
    {
        private static void Main(string[] argv)
        {
            var args = new MainArgs(argv);
            ProcessArguments(args);
        }

        private static void ProcessArguments(MainArgs args)
        {
            var container = new WindsorContainer();
            container.Register(Component.For<WordVisualizer>().ImplementedBy<WordVisualizer>());
            container.Register(Component.For<ICloudLayouter>().ImplementedBy<CloudLayouter>());
            container.Register(Component.For<ICloudVisualizer>().ImplementedBy<CloudVisualizer>()
                .DependsOn(Dependency.OnValue<string>(args.ArgImagePath)));
            container.Register(Component.For<ITextAnalyzer>()
                .ImplementedBy<TextAnalyzer>()
                .DependsOn(Dependency.OnValue("text", new FileWordSource(args.ArgSourcePath)))
                .DependsOn(Dependency.OnValue("stopWords", new FileWordSource(args.ArgStopwordsPath))));
            container.Register(Component.For<IEnumerable<IPlacementStrategy>>().Instance(new List<IPlacementStrategy>
                {new SpiralStrategy(), new CenterMoveStrategy()}));
            
            var visualizer = container.Resolve<WordVisualizer>();
            var imageSettings = new ImageSettings(args.OptTextColor, args.OptBackgroundColor, args.OptFont,
                args.OptImageSize);
            
            visualizer.CreateCloudImage(imageSettings)
                .OnFail(Console.WriteLine);
        }
    }
}
