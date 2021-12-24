using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.ResultOf;

namespace TagsCloudVisualization.TextPreparers
{
    public class TextPreparer : ITextPreparer
    {
        private readonly IEnumerable<Func<string, bool>> filters;
        private readonly IEnumerable<Func<string, string>> preparations;

        public TextPreparer(IEnumerable<Func<string, bool>> filters, IEnumerable<Func<string, string>> preparations)
        {
            this.filters = filters;
            this.preparations = preparations;
        }

        public Result<IEnumerable<string>> PrepareText(IEnumerable<string> text)
        {
            return Result.Of(() => text
                .Where(word => !IsFiltered(word))
                .Select(PrepareWord)
                .ToList())
                .Then(x => x as IEnumerable<string>)
                .RefineError("Something went wrong when filtering or preparing words");
        }
        
        private string PrepareWord(string word) =>
            preparations.Aggregate(word, (current, preparation) => preparation(current));

        private bool IsFiltered(string word) => filters.Any(filter => filter(word));
    }
}