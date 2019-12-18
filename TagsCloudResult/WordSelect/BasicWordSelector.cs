using System;
using System.Collections.Generic;
using System.Drawing;
using TagsCloudResult.Infrastructure.Common;

namespace TagsCloudResult
{
    public static class BasicWordsSelector 
    {
       
        public static Result<IEnumerable<LayoutWord>> Select(IEnumerable<string> wordsList, WordSetting wordSetting)
        {
            if (new Font(wordSetting.FontName, 5).Name != wordSetting.FontName)
                return Result.Fail<IEnumerable<LayoutWord>>($"Not such font {wordSetting.FontName}");
            
            if (wordSetting.Color != "random" && Color.FromName(wordSetting.Color).Name != wordSetting.Color)
                return Result.Fail<IEnumerable<LayoutWord>>($"No such color {wordSetting.Color}");
            
            var wordsFrequency = GetWordsFrequency(wordsList);
            
            
            var layoutWords = new List<LayoutWord>();
            foreach (var (word, frequency) in wordsFrequency)
            {
                var font = new Font(wordSetting.FontName, frequency < 12 ? frequency + 6 : 18);                    
                var size = GetSize(word, frequency, font);
                var brush = new SolidBrush(GetColor(wordSetting));
                layoutWords.Add(new LayoutWord(word, brush, font, size));
            }
            
            return Result.Ok((IEnumerable<LayoutWord>)layoutWords);
        }

        private static Color GetColor(WordSetting wordSetting)
        {
            var random = new Random();
            
            return wordSetting.Color == "random"
                ? Color.FromArgb(random.Next(255), random.Next(255), random.Next(255))
                : Color.FromName(wordSetting.Color);
        }
            


        private static Dictionary<string, int> GetWordsFrequency(IEnumerable<string> words)
        {
            var wordsFrequency = new Dictionary<string, int>();
            foreach (var word in words)
            {
                var clearWord = SelectWord(word);
                if (clearWord is null)
                    continue;
                if (wordsFrequency.ContainsKey(clearWord))
                    wordsFrequency[clearWord]++;
                else
                    wordsFrequency[clearWord] = 1;
            }

            return wordsFrequency;
        }

        private static Size GetSize(string word, int frequency, Font font)
        {
            return new Size(word.Length * ((int) Math.Floor(font.Size) - (frequency == 1 ? 0 : 2)),
                font.Height);
        }

        private static string SelectWord(string word)
        {
            return word.Length < 4 ? null : word.ToLower().Trim();
        }
    }
}