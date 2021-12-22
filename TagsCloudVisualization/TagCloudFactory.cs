using System;
using System.Collections.Generic;
using System.Drawing;
using Autofac;
using TagsCloudVisualization.Default;
using TagsCloudVisualization.Infrastructure;
using TagsCloudVisualization.Infrastructure.Text;
using TagsCloudVisualization.Infrastructure.TextAnalysing;
using TagsCloudVisualization.Infrastructure.Visualisation;

namespace TagsCloudVisualization
{
    public class TagCloudFactory
    {
        private static readonly Dictionary<string, ITokenOrderer> Orders = new Dictionary<string, ITokenOrderer>()
        {
            ["sorted"] = new TokenSortedOrder(),
            ["mixed"] = new TokenShuffler()
        };

        public Result<TagCloud> CreateInstance(bool manhattan, string order)
        {
            var metric = manhattan ? 
                (Func<PointF, PointF, float>)CircularCloudMaker.ManhattanDistance :
                CircularCloudMaker.Distance;
            if (order == null || !Orders.ContainsKey(order))
                return Result.Fail<TagCloud>("Unknown order");
            var orderer = Orders[order];
            var builder = new ContainerBuilder();
            builder.RegisterType<TxtTextReader>().As<ITextReader>();
            builder.RegisterType<RandomTagColor>().As<ITagColorChooser>();
            builder.RegisterType<WordCounter>().As<ITokenWeigher>();
            builder.RegisterType<WordSelector>().UsingConstructor().As<IWordSelector>();
            builder.RegisterType<ReaderContainer>();
            builder.RegisterType<TokenGenerator>();
            builder.RegisterType<TagCloudMaker>();
            builder.RegisterType<TagCloudVisualiser>();
            builder.RegisterType<TagCloud>();
            builder.RegisterInstance(orderer).As<ITokenOrderer>();
            builder.RegisterInstance(new CircularCloudMaker(Point.Empty, metric)).As<ICloudMaker>();
            var container = builder.Build();
            using var scope = container.BeginLifetimeScope();
            return scope.Resolve<TagCloud>().AsResult();
        }
    }
}