using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Helpers
{
    public class TagCloudHelper
    {
        public static Result<List<Tag>> CreateTagsFromWords(IEnumerable<string> words, int amount, int minFontSize = 10, int maxFontSize = 50)
        {
            if (amount <= 0)
                return Result.Fail<List<Tag>>("The amount of words must be greater than zero");
            
            var frequencyDictionary = CreateFrequencyDictionary(words);
            
            if (frequencyDictionary.Count == 0)
                return new List<Tag>();

            var orderedFrequencyDict = frequencyDictionary
                .OrderByDescending(x => x.Value)
                .Take(amount)
                .ToDictionary( kvp => kvp.Key, kvp => kvp.Value );

            var minFrequency = orderedFrequencyDict.Min(x => x.Value);
            var maxFrequency = orderedFrequencyDict.Max(x => x.Value);

            return Result.Ok(CalculateAndCreateTags(orderedFrequencyDict, maxFrequency, minFrequency, maxFontSize, minFontSize));
        }

        public static void ShuffleTags(List<Tag> tags, List<Size> sizes)
        {
            var random = new Random();

            for (var i = tags.Count - 1; i >= 1 ; i--)
            {
                var j = random.Next(i + 1);
                (tags[j], tags[i]) = (tags[i], tags[j]);
                (sizes[j], sizes[i]) = (sizes[i], sizes[j]);
            }
        }

        public static Result<List<Size>> GenerateRectangleSizes(IEnumerable<Tag> tags)
        {
            using var gp = Graphics.FromImage(new Bitmap(1, 1));

            return Result.Ok(tags
                .Select(tag => gp.MeasureString(tag.Text, new Font("Arial", tag.FontSize))
                    .ToSize())
                .ToList());
        }

        private static Dictionary<string, int> CreateFrequencyDictionary(IEnumerable<string> words)
        {
            var dict = new Dictionary<string, int>();
            
            foreach (var word in words)
            {
                if (!dict.ContainsKey(word))
                    dict[word] = 0;
                dict[word]++;
            }

            return dict;
        }

        private static List<Tag> CalculateAndCreateTags(Dictionary<string, int> frequencyDictionary, 
            int maxFrequency, int minFrequency, int maxFontSize, int minFontSize)
        {
            var tags = new List<Tag>();
            
            foreach (var (text, frequency) in frequencyDictionary)
            {
                var fontSize = 0;
                
                if (maxFrequency - minFrequency != 0) 
                    fontSize = maxFontSize * (frequency - minFrequency) / (maxFrequency - minFrequency);

                if (fontSize < minFontSize)
                    fontSize = minFontSize;
                
                tags.Add(new Tag(text, fontSize, frequency));
            }

            return tags;
        }
    }
}