namespace TagCloud.Core.Util
{
    public class TagStat
    {
        public TagStat(string word, int repeatsCount)
        {
            Word = word;
            RepeatsCount = repeatsCount;
        }

        public string Word { get; }
        public int RepeatsCount { get; }
    }
}