using System;
using System.IO;
using System.Windows.Forms;
using TagCloud.ExceptionHandler;
using TagCloud.Interfaces;
using TagCloud.TagCloudVisualization.Analyzer;
using TagCloud.Words;

namespace TagCloud.Actions
{
    public class LoadWordsAction : IUiAction
    {
        private readonly IRepository wordsRepository;
        private IWordAnalyzer wordAnalyzer;
        private IReader reader;
        private IExceptionHandler exceptionHandler;

        public LoadWordsAction(IRepository wordsRepository, IWordAnalyzer wordAnalyzer, 
            IReader reader, IExceptionHandler exceptionHandler)
        {
            this.wordsRepository = wordsRepository;
            this.wordAnalyzer = wordAnalyzer;
            this.reader = reader;
            this.exceptionHandler = exceptionHandler;
        }

        public string Category => "File";
        public string Name => "Tag Words";
        public string Description => "Load file with Tag WordsRepository";

        public void Perform()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                Result.Of(() => reader.Read(openFileDialog.FileName))
                    .Then(fileContent => wordAnalyzer.SplitWords(fileContent))
                    .Then(splittedWords => wordsRepository.Load(splittedWords))
                    .RefineError("Failed, trying to load tag words")
                    .OnFail(exceptionHandler.HandleException);
            }

        }
    }



}