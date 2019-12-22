using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TagsCloud.ErrorHandler;
using TagsCloud.Visualization;
using TagsCloud.Visualization.Tag;
using TagsCloud.WordPreprocessing;
using TagsCloud.Writer;

namespace TagsCloud
{
    public class Application
    {
        private readonly IWordAnalyzer _wordStatisticGetter;
        private readonly ILayouter _layouter;
        private readonly IVisualizer _visualizer;
        private readonly Options _options;
        private readonly IWriter _writer;
        private readonly IWordGetter _wordGetter;
        private readonly IWordsProcessor _wordsProcessor;
        private readonly ImageFormat _imageFormat;
        private readonly IErrorHandler _errorHandler;

        private readonly char[] _delimiters = new char[]
            {',', '.', ' ', ':', ';', '(', ')', '—', '–', '[', ']', '!', '?', '\n'};
        

        public static readonly Dictionary<ImageFormat, string> ImageFormatDenotation =
            new Dictionary<ImageFormat, string>
            {
                {ImageFormat.Jpeg, "jpg"},
                {ImageFormat.Png, "png"},
                {ImageFormat.Bmp, "bmp"},
                {ImageFormat.Gif, "gif"}
            };

        public Application(IWordAnalyzer wordStatisticGetter, ILayouter layouter, IVisualizer visualizer,
            Options options,
            IWriter writer, IWordGetter wordGetter, IWordsProcessor wordsProcessor, IErrorHandler errorHandler,
            ImageFormat imageFormat = null)
        {
            this._wordStatisticGetter = wordStatisticGetter;
            this._layouter = layouter;
            this._visualizer = visualizer;
            this._options = options;
            this._writer = writer;
            this._wordGetter = wordGetter;
            this._wordsProcessor = wordsProcessor;
            this._imageFormat = imageFormat ?? ImageFormat.Jpeg;
            this._errorHandler = errorHandler;
        }

        public void Run()
        {
            var tags = GetTags();
            if (!tags.IsSuccess)
            {
                _errorHandler.Handle(tags.Error);
                return;
            }
            CreateImageAndSave(tags.Value);
        }

        private void CreateImageAndSave(IEnumerable<Tag> tags)
        {
            using (var bitmap = _visualizer.GetCloudVisualization(tags.ToList()))
            {
                var name = Path.GetFileName(_options.FilePath);
                var imgName = Path.ChangeExtension(name, ImageFormatDenotation[_imageFormat]);
                _writer.Write($"Your image located at:  {new FileInfo(imgName).FullName}");
                bitmap.Save(imgName, _imageFormat);
            }
        }

        public Result<IEnumerable<Tag>> GetTags()
        {
            var rawWords = _wordGetter.GetWords(_delimiters);
            if (!rawWords.IsSuccess)
            {
                _errorHandler.Handle(rawWords.Error);
                return Result.Fail<IEnumerable<Tag>>($"Can't read words");
            }
            var words = _wordsProcessor.ProcessWords(rawWords.Value);
            foreach (var error in words.Where(x=>!x.IsSuccess).Select(x => x.Error))
            {
                _errorHandler.Handle(error);
            }
            var statistics = _wordStatisticGetter.GetWordsStatistics(words.Where(x => x.IsSuccess).Select(x => x.Value));
            var result =  _layouter.GetTags(statistics);
            return result.Any(x => !x.IsSuccess) ? Result.Fail<IEnumerable<Tag>>($"Can't construct cloud'") :
                Result.Ok(result.Select(r => r.Value));
        }
    }
}