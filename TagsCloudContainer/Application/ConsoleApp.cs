using System.Drawing.Imaging;
using DocumentFormat.OpenXml.Wordprocessing;
using TagsCloudContainer.TextReaders;
using TagsCloudContainer.Visualisators;
using TagsCloudContainer.WorkWithWords;

namespace TagsCloudContainer.Application
{
    public class ConsoleApp : IApp
    {
        private TextReaderGenerator _readerGenerator;
        private Settings _settings;
        private WordHandler _handler;
        private readonly IVisualisator _visualisator;

        public ConsoleApp(TextReaderGenerator readerGenerator, Settings settings,
            WordHandler handler, IVisualisator visualisator)
        {
            _readerGenerator = readerGenerator;
            _settings = settings;
            _handler = handler;
            _visualisator = visualisator;
        }

        public Result<None> Run()
        {
            var text = GetText(_settings.FileName).OnFail(ExitProgram);
            
            var boringText = GetText(_settings.BoringWordsFileName).OnFail(ExitProgram);
            
            var words = _handler.ProcessWords(text.Value, boringText.Value);
            var bitmap = _visualisator.Paint(words).OnFail(ExitProgram);

            var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            bitmap.Value.Save(projectDirectory + "\\Results", "Rectangles", ImageFormat.Png);
            
            return new Result<None>(null);
        }

        private Result<string> GetText(string fileName)
        {
            if (!File.Exists(fileName))
                return Result.Fail<string>($"File is not exists: {fileName}");
            var result = _readerGenerator
                .GetReader(fileName)
                .Then(x => x.GetTextFromFile(fileName))
                .RefineError("Can't get text from file");
            return result;
        }

        private void ExitProgram(string text)
        {
            Console.WriteLine(text);
            Environment.Exit(1);
        }
    }
}