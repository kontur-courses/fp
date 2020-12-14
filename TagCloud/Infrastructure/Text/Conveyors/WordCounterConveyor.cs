using System.Collections.Generic;
using System.Linq;
using TagCloud.Infrastructure.Text.Information;

namespace TagCloud.Infrastructure.Text.Conveyors
{
    public class WordCounterConveyor : IConveyor<string>
    {
        public IEnumerable<(string token, TokenInfo info)> Handle(IEnumerable<(string token, TokenInfo info)> tokens)
        {
            tokens = tokens.ToList();
            var counts = GetCount(tokens.Select(pair => pair.token));
            foreach (var word in counts.Keys)
            {
                var (_, info) = tokens.First(pair => pair.token == word);
                info.Frequency = counts[word];
                yield return (word, info);
            }
        }

        private static Dictionary<string, int> GetCount(IEnumerable<string> tokens) => tokens
            .GroupBy(token => token)
            .ToDictionary(
                x => x.Key,
                x => x.Count());
    }
}