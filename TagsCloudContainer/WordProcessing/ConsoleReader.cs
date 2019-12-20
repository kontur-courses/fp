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
            Console.WriteLine("Enter words one at a time. Enter new line to stop");
            while (true)
            {
                var word = Console.ReadLine();
                if (word == "")
                    break;
                words.Add(word);
            }

            return words.Count == 0 
                ? Result.Fail<IEnumerable<string>>("No words been entered") 
                : words;
        }
    }
}