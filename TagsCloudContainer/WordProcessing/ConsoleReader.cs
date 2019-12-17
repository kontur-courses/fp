using System;
using System.Collections.Generic;
using ResultOf;

namespace TagsCloudContainer.WordProcessing
{
    public class ConsoleReader : IWordProvider
    {
        public Result<IEnumerable<string>> GetWords()
        {
            var words = new HashSet<string>();
            while (true)
            {
                var word = Console.ReadLine();
                if (word == "")
                    break;
                words.Add(word);
            }

            return words;
        }
    }
}