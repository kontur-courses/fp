using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using TagsCloud.ErrorHandling;
using TagsCloud.TagsCloudVisualization;
using TagsCloud.WordPrework;

namespace TagsCloud
{
    public class Application
    {
        private readonly IWordAnalyzer wordAnalyzer;
        private readonly ITagCloudLayouter tagCloudLayouter;
        private readonly ITagsCloudVisualizer tagsCloudVisualizer;
        private readonly IEnumerable<Result<string>> words;
        private readonly IErrorHandler errorHandler;

        public Application(IWordAnalyzer wordAnalyzer, ITagCloudLayouter tagCloudLayouter, 
            ITagsCloudVisualizer tagsCloudVisualizer, IEnumerable<Result<string>> words, IErrorHandler errorHandler)
        {
            this.wordAnalyzer = wordAnalyzer;
            this.tagCloudLayouter = tagCloudLayouter;
            this.tagsCloudVisualizer = tagsCloudVisualizer;
            this.words = words;
            this.errorHandler = errorHandler;
        }

        public void Run(Options options)
        {
            foreach (var word in words)
                if (!word.IsSuccess)
                {
                    errorHandler.Handle(word.Error);
                    return;
                }
            var tags = GetTags(options);
            if (!tags.IsSuccess)
            {
                errorHandler.Handle(tags.Error);
                return;
            }
            DrawTagCloudAndSave(options, tags.Value)
                .OnFail(e => errorHandler.Handle(e));
        }

        public Result<List<Tag>> GetTags(Options options)
        {
            var frequency = options.PartsToUse.Any()
                ? wordAnalyzer.GetSpecificWordFrequency(options.PartsToUse)
                : wordAnalyzer.GetWordFrequency(new HashSet<PartOfSpeech>(options.BoringParts));
            if (!frequency.IsSuccess)
                return Result.Fail<List<Tag>>(frequency.Error);
            return tagCloudLayouter.GetTags(frequency.Value);
        }

        public Result<None> DrawTagCloudAndSave(Options options, List<Tag> tags)
        {
            var bitmap = tagsCloudVisualizer.GetCloudVisualization(tags);
            if (!bitmap.IsSuccess)
                return Result.Fail<None>(bitmap.Error);
            var name = Path.GetFileName(options.File);
            var newName = Path.ChangeExtension(name, "jpg");
            bitmap.Value.Save(newName, ImageFormat.Jpeg);
            return Result.Ok();
        }
    }
}
