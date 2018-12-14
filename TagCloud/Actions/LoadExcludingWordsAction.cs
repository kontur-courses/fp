using System.Windows.Forms;
using TagCloud.ExceptionHandler;
using TagCloud.Interfaces;
using TagCloud.TagCloudVisualization.Analyzer;
using TagCloud.Words;

namespace TagCloud.Actions

{
    internal class LoadExcludingWordsAction : IUiAction
    {
        private readonly IExcludingRepository wordsRepository;
        private readonly IWordAnalyzer analyzer;
        private readonly IExceptionHandler exceptionHandler;
        private readonly IReader reader;

        public LoadExcludingWordsAction(IExcludingRepository wordsRepository, IWordAnalyzer analyzer,
            IReader reader, IExceptionHandler exceptionHandler)
        {
            this.wordsRepository = wordsRepository;
            this.analyzer = analyzer;
            this.reader = reader;
            this.exceptionHandler = exceptionHandler;
        }

        public string Category => "File";
        public string Name => "Excluding Words";
        public string Description => "Load wordsRepository to exclude from Tag Cloud";

        public void Perform()
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                Result.Of(() => reader.Read(openFileDialog.FileName))
                    .Then(fileContent => analyzer.SplitWords(fileContent))
                    .Then(splittedWords => wordsRepository.Load(splittedWords))
                    .RefineError("Failed, trying to load excluding words")
                    .OnFail(exceptionHandler.HandleException);
            }
        }
    }
}