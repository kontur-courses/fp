using System.Collections.Generic;
using YandexMystem.Wrapper.Models;

namespace TagsCloudContainer
{
    public class Mysteam : IMysteam
    {
        private readonly YandexMystem.Wrapper.Mysteam mysteam;

        public Mysteam()
        {
            mysteam = new YandexMystem.Wrapper.Mysteam();
        }

        public Result<List<WordModel>> GetWords(string text)
        {
            return Result.Of(() =>mysteam.GetWords(text));
        }
    }
}