using System.Collections.Generic;
using TagsCloudVisualization.Parsers;
using TagsCloudVisualization.ResultOf;
using TagsCloudVisualization.TextPreparers;

namespace TagsCloudVisualization.TextHandlers
{
    public class TextHandler : ITextHandler
    {
        private readonly IParser parser;
        private readonly ITextPreparer preparer;

        public TextHandler(ITextPreparer preparer, IParser parser)
        {
            this.preparer = preparer;
            this.parser = parser;
        }
        
        public Result<IEnumerable<string>> Handle(string filePath)
        {
            return parser
                .ParseWords(filePath)
                .Then(preparer.PrepareText);
        }
    }
}