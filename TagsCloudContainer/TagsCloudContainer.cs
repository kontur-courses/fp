using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer
{
    public class TagsCloudContainer
    {
        public readonly List<IWordsSource> Sources = new List<IWordsSource>();

        public readonly List<Func<string, string>> PreprocessingFuncions =
            new List<Func<string, string>> {w => w.ToLower()};

        public IWordsLayouter Layouter = new CircularCloudLayouter();
        public IWordRenderer Renderer;

        public TagsCloudContainer AddSource(IWordsSource source)
        {
            Sources.Add(source);
            return this;
        }

        public TagsCloudContainer AddFromFile(string fileName) => AddSource(new WordsSourceFromFile(fileName));
        public TagsCloudContainer AddFromText(string text) => AddSource(new WordsSourceFromText(text));

        public TagsCloudContainer Preprocessing(Func<string, string> preprocessFunc)
        {
            PreprocessingFuncions.Add(preprocessFunc);
            return this;
        }

        public TagsCloudContainer Excluding(Func<string, bool> wordsPredicate)
            => Preprocessing(w => wordsPredicate(w) ? null : w);

        public TagsCloudContainer Excluding(params string[] words) => Excluding(words.Contains);
        public TagsCloudContainer Excluding(HashSet<string> words) => Excluding(words.Contains);

        public TagsCloudContainer Layouting(IWordsLayouter layouter)
        {
            Layouter = layouter;
            return this;
        }

        public TagsCloudContainer Rendering(IWordRenderer renderer)
        {
            Renderer = renderer;
            return this;
        }

        public virtual Result<None> Render()
        {
            return GetWords()
                .Then(PreprocessWords)
                .Then(Enumerable.ToArray)
                .Then(words => words.Select(word => new LayoutedWord(word.word, word.count)))
                .Then(Renderer.SizeWords)
                .Then(Layouter.LayoutWords)
                .Then(Renderer.Render);
        }

        protected Result<IEnumerable<(string word, int count)>> GetWords()
        {
            return Sources
                .Select(s => s.GetWords())
                .EnumerateOrFail()
                .Then(words => words
                    .SelectMany(word => word)
                    .GroupBy(w => w.word)
                    .Select(g => (g.Key,
                        g.Sum(gg => gg.count))));
        }

        protected string PreprocessWord(string word)
        {
            foreach (var preprocessingFuncion in PreprocessingFuncions)
            {
                word = preprocessingFuncion(word);
                if (word == null) return null;
            }

            return word;
        }

        protected IEnumerable<(string word, int count)> PreprocessWords(IEnumerable<(string word, int count)> words)
        {
            return words
                .Select(s => (word: PreprocessWord(s.word), s.count))
                .Where(w => w.word != null)
                .GroupBy(w => w.word)
                .Select(g => (g.Key,
                    g.Sum(gg => gg.count)));
        }
    }
}