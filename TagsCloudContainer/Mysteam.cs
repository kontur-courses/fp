using System.Collections.Generic;
using YandexMystem.Wrapper.Models;

namespace TagsCloudContainer
{
    public class Mysteam : IMysteam
    {
        private YandexMystem.Wrapper.Mysteam mysteam;

        public Mysteam()
        {
            mysteam = new YandexMystem.Wrapper.Mysteam();
        }
        public List<WordModel> GetWords(string text)
        {
            return mysteam.GetWords(text);
        }
    }
}