using System.Windows.Forms;
using TagCloud.ExceptionHandler;
using TagCloud.Interfaces;
using TagCloud.TagCloudVisualization.Analyzer;
using TagCloud.Words;

namespace TagCloud.Actions
{
    public class LoadWordsAction : IUiAction
    {
        private readonly IWordRepository wordsRepository;
        private readonly IExceptionHandler exceptionHandler;
        private readonly IReader reader;
        private readonly IWordAnalyzer wordAnalyzer;

        public LoadWordsAction(IWordRepository wordsRepository, IWordAnalyzer wordAnalyzer,
            IReader reader, IExceptionHandler exceptionHandler)
        {
            this.wordsRepository = wordsRepository;
            this.wordAnalyzer = wordAnalyzer;
            this.reader = reader;
            this.exceptionHandler = exceptionHandler;
        }

        public string Category => "File";
        public string Name => "Tag Words";
        public string Description => "Select file with text, you would like to convert into TagCloud";

        public void Perform()
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                Result.Of(() => reader.Read(openFileDialog.FileName))
                    .Then(fileContent => wordAnalyzer.SplitWords(fileContent))
                    .Then(splitWords => wordsRepository.Load(splitWords))
                    .RefineError("Failed, trying to load tag words")
                    .OnFail(exceptionHandler.HandleException);
            }
        }
    }
}