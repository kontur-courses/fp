using System.Collections.Generic;
using System.Linq;
using FunctionalStuff;
using MyStem.Wrapper.Workers.Lemmas;

namespace TagCloud.Core.Text.Preprocessing
{
    public class MyStemWordsConverter : IWordConverter
    {
        private readonly IUserNotifier notifier;
        private readonly ILemmatizer normalizer;

        public MyStemWordsConverter(ILemmatizer normalizer, IUserNotifier notifier)
        {
            this.normalizer = normalizer;
            this.notifier = notifier;
        }

        public IEnumerable<string> Normalize(IEnumerable<string> words) =>
            GetNormalizedOrOriginalWords(words.ToArray());

        private IEnumerable<string> GetNormalizedOrOriginalWords(string[] words) =>
            normalizer.GetWords(string.Join(" ", words))
                .Then(r => r
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(x => x.TrimEnd('?'))
                    .ToArray())
                .GetValueOrHandleError(words, notifier.Notify);
    }
}