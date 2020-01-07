using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagsCloudForm.CircularCloudLayouterSettings;
using TagsCloudForm.Common;

namespace TagsCloudForm.WordFilters
{
    public class BoringWordsFilter : IWordsFilter
    {
        public Result<IEnumerable<string>> Filter(ICircularCloudLayouterWithWordsSettings settings,
            IEnumerable<string> words)
        {
            HashSet<string> boringWords;
            try
            {
                var settingsFilename = settings.BoringWordsFile;
                boringWords = File.ReadAllLines(settingsFilename).ToHashSet(StringComparer.OrdinalIgnoreCase);
            }
            catch (FileNotFoundException)
            {
                return new Result<IEnumerable<string>>("Не удалось загрузить файл с boring words: файл отсутствует",
                    words);
            }
            catch (Exception e)
            {
                return new Result<IEnumerable<string>>("Не удалось загрузить файл с boring words: " + e.Message, words);
            }

            return Filter(boringWords, words);
        }

        public Result<IEnumerable<string>> Filter(HashSet<string> boringWords, IEnumerable<string> words)
        {
            return Result.Of(() => words.Where(x => !boringWords.Contains(x)), words);
        }
    }
}
