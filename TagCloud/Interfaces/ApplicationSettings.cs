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

        [Browsable(false)] public Encoding TextEncoding { get; set; } = Encoding.Default;

        public Result<StreamReader> GetDocumentStream()
        {
            if (FilePath is null || !File.Exists(FilePath))
                return Result.Fail<StreamReader>("File is not selected or not found");
            return Result.Ok(new StreamReader(File.OpenRead(FilePath), TextEncoding));
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
