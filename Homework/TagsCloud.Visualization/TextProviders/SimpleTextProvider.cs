using TagsCloud.Visualization.Utils;

namespace TagsCloud.Visualization.TextProviders
{
    public class SimpleTextProvider : ITextProvider
    {
        private readonly string text;

        public SimpleTextProvider(string text) => this.text = text;

        public Result<string> Read() => text;
    }
}