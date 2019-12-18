using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudGenerator.CloudLayouter
{
    public class Cloud
    {
        public Point Center { get; }
        public List<Word> Words { get; }

        public Cloud(Point center)
        {
            Center = center;
            Words = new List<Word>();
        }

        public Cloud AddWord(Word word)
        {
            Words.Add(word);

            return this;
        }

        public Cloud AddWords(IEnumerable<Word> words)
        {
            foreach (var word in words)
            {
                AddWord(word);
            }

            return this;
        }
    }
}