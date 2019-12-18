using System;
using System.Collections.Generic;
using System.Linq;
using ResultOf;
using TagsCloudContainer.Interfaces;
using YandexMystem.Wrapper;
using YandexMystem.Wrapper.Models;

namespace TagsCloudContainer
{
    public class DefaultWordsFilter : IWordsFilter
    {
        private HashSet<string> excludedTypes;

        public DefaultWordsFilter(IEnumerable<string> excluded)
        {
            excludedTypes = new HashSet<string>();
            foreach (var type in excluded)
            {
                excludedTypes.Add(type);
            }
        }


        public Result<IEnumerable<string>> FilterWords(IEnumerable<string> words)
        {
            var wordsInfo = GetInfoAboutWords(words.ToArray());
            if (!wordsInfo.IsSuccess)
                return Result.Fail<IEnumerable<string>>(wordsInfo.Error).RefineError("Something wrong with mystem.exe");

            return FilterWords(wordsInfo.GetValueOrThrow());
        }

        private Result<IEnumerable<string>> FilterWords(List<WordModel> wordsInfos)
        {
            var res = new List<string>();
            foreach (var wordInfo in wordsInfos)
            {
                var flag = false;
                foreach (var type in excludedTypes)
                {
                    if (wordInfo.SourceWord.Analysis[0].Gr.Contains(type))
                        flag = true;
                }

                if (flag)
                    continue;
                res.Add(wordInfo.SourceWord.Text);
            }

            return Result.Ok(res.AsEnumerable());
        }

        private string WordsToString(string[] words)
        {
            var res = "";
            foreach (var str in words)
            {
                res += $"{str} ";
            }

            return res;
        }

        private Result<List<WordModel>> GetInfoAboutWords(string[] words)
        {
            try
            {
                var mst = new Mysteam();
                var res = mst.GetWords(WordsToString(words));
                return Result.Ok(res);
            }
            catch (Exception e)
            {
                return Result.Fail<List<WordModel>>(e.Message);
            }
        }
    }
}