using ResultOf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudContainer
{
    class Program
    {
        static void Main(string[] args)
        {
            var clientControl = new ClientControlFunc(new ConsoleClientFunc());

            var boringWords = new List<string>();
            boringWords.AddRange(BoringWords.Prepositions);
            boringWords.AddRange(BoringWords.Pronouns);

            var pathToText = @"..\..\..\Files\test.txt";
            HelperFunctions.RepeatActionWhileNotFinish(clientControl.IsFinish, () =>
            {
                var result = Result.Ok(pathToText)
                    .Then(RecipientOfWords.ReadLineByLine)
                    .Then(words => words.FilterWords(boringWords.ToHashSet()))
                    .Then(Enumeration.ConvertToFrequency)
                    .Then(words => TagsCloud.PaintWords(TagsCloud.CreateCloud, words, clientControl.GetImageSettings))
                    .Then(image => FileSaver.SaveImage(image, clientControl.GetNameForImage, @"..\..\..\Files", ImageFormats.png));

                if (result.IsSuccess)
                    clientControl.ShowPathToImage(result.Value);
                else
                    clientControl.ShowMessage(result.Error);
            });

        }
    }

    public static class HelperFunctions
    {
        public static void RepeatActionWhileNotFinish(Func<bool> isFinish, Action action)
        {
            while (!isFinish()) action();
        }

        public static Result<Dictionary<string, object>> CombineResults<T>(this Dictionary<string, object> results, Result<T> newRes, string newResName)
        {
            if (!newRes.IsSuccess)
                return Result.Fail<Dictionary<string, object>>(newRes.Error);
            var resultsRes = Result.Ok(results);
            
            return resultsRes.Then(res => { res.Add(newResName, newRes.Value); return res; });
        }
    }
}
