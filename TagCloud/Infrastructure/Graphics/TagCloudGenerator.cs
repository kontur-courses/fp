using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.Infrastructure.Text;
using TagCloud.Infrastructure.Text.Conveyors;
using TagCloud.Infrastructure.Text.Information;

namespace TagCloud.Infrastructure.Graphics
{
    public class TagCloudGenerator : IImageGenerator
    {
        private readonly IReader<string> reader;
        private readonly IEnumerable<IConveyor<string>> conveyors;
        private readonly IPainter<string> painter;

        public TagCloudGenerator(IReader<string> reader, IEnumerable<IConveyor<string>> conveyors, IPainter<string> painter)
        {
            this.reader = reader;
            this.conveyors = conveyors;
            this.painter = painter;
        }

        public Image Generate()
        {
            var tokens = reader.ReadTokens();
            var analyzedTokens = conveyors.Aggregate(
                tokens.Select(line => (line, new TokenInfo())),
                (current, filter) => filter.Handle(current).ToArray());
            return painter.GetImage(analyzedTokens);
        }
    }
}