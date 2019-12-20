using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using NHunspell;
using TagsCloudForm.CircularCloudLayouterSettings;
using TagsCloudForm.Common;

namespace TagsCloudForm.WordFilters
{
    public class SpellCheckerFilter : IWordsFilter
    {
        public Result<IEnumerable<string>> Filter(IEnumerable<string> words, LanguageEnum language)
        {
            Hunspell checker;
            if (language == LanguageEnum.English)
                try
                {
                    checker = new Hunspell(
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\", "en_us.aff"),
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\", "en_us.dic"));
                }
                catch (Exception e)
                {
                    return new Result<IEnumerable<string>>("Файлы словарей для Hunspell не найдены: "+e.Message, words);
                }
            else
                return new Result<IEnumerable<string>>("Chosen Language not supported", words);
            return Result.Ok(words.Where(x=>checker.Spell(x.ToLower())));
        }

        public Result<IEnumerable<string>> Filter(ICircularCloudLayouterWithWordsSettings settings, IEnumerable<string> words)
        {
            return Filter(words, settings.Language);
        }
    }
}
