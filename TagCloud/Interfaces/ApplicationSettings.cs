using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using TagCloud.WordsPreprocessing.TextAnalyzers;

namespace TagCloud.Interfaces
{
    public class ApplicationSettings
    {
        [Browsable(false)] public string FilePath { get; set; }

        [Browsable(false)] public Encoding TextEncoding { get; set; } = Encoding.UTF8;

        public Result<StreamReader> GetDocumentStream()
        {
            return Result.Of(() => new StreamReader(File.OpenRead(FilePath), TextEncoding), "Can not open the file");
        }

        [Browsable(false)] public ITextAnalyzer[] TextAnalyzers { get; }

        [Browsable(false)] public ITextAnalyzer CurrentTextAnalyzer { get; set; }


        public ApplicationSettings(ITextAnalyzer[] analyzers)
        {
            TextAnalyzers = analyzers;
            CurrentTextAnalyzer = TextAnalyzers.FirstOrDefault();
        }
    }
}
