using System.Collections.Generic;

namespace TagsCloud.Interfaces
{
    public interface IWordsRectanglesScaler
    {
        public SortedDictionary<double, List<string>> ConvertFreqToProportions(
            SortedDictionary<int, List<string>> wordsFreq);
    }
}