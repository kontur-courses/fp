using System.IO;
using System.Windows.Forms;
using TagCloud.ExceptionHandler;
using TagCloud.Interfaces;
using TagCloud.TagCloudVisualization.Analyzer;
using TagCloud.Words;

namespace TagCloud.Actions

{
    class LoadExcludingWordsAction : IUiAction
    {
        private readonly IExcludingRepository wordsRepository;
        private IWordAnalyzer analyzer;
        private IReader reader;
        private IExceptionHandler exceptionHandler;

        public LoadExcludingWordsAction(IExcludingRepository wordsRepository, IWordAnalyzer analyzer, 
            IReader reader, IExceptionHandler exceptionHandler)
        {
            this.wordsRepository = wordsRepository;
            this.analyzer = analyzer;
            this.reader = reader;
            this.exceptionHandler = exceptionHandler;
        }
        public string Category { get; } = "File";
        public string Name { get; } = "Excluding Words";
        public string Description { get; } = "Load wordsRepository to exclude from Tag Cloud";
        public void Perform()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
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