using System.Collections.Generic;
using YandexMystem.Wrapper.Models;

namespace TagsCloudContainer
{
    public interface IMysteam
    {
        Result<List<WordModel>> GetWords(string text);
    }
}