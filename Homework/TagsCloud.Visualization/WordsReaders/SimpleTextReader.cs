using TagsCloud.Visualization.WordsReaders;

namespace TagsCloud.WebApi.Services
{
    public class SimpleTextReader : IWordsReadService
    {
        private string words;

        public SimpleTextReader(string words)
        {
            this.words = words;
        }
        
        public string Read() => words;
    }
}