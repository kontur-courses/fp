using System;
using TagCloud.ExceptionHandler;
using TagCloud.Forms;
using TagCloud.Interfaces;
using TagCloud.Settings;
using TagCloud.TagCloudVisualization.Visualization;
using TagCloud.Words;

namespace TagCloud.Actions
{
    public class DrawingAction : IUiAction
    {
        private readonly ImageBox imageBox;
        private readonly IRepository wordsRepository;
        private readonly ITagGenerator tagGenerator;
        private readonly ImageSettings imageSettings;
        private readonly IExceptionHandler exceptionHandler;
        
        public DrawingAction(ImageBox imageBox, IRepository wordsRepository, 
                                ITagGenerator tagGenerator, ImageSettings imageSettings, IExceptionHandler exceptionHandler)
        {
            this.imageBox = imageBox;
            this.wordsRepository = wordsRepository;
            this.tagGenerator = tagGenerator;
            this.imageSettings = imageSettings;
            this.exceptionHandler = exceptionHandler;
        }
        public string Category { get; } = "Tag Cloud";
        public string Name { get; } = "Draw";
        public string Description { get; } = "Draw new Tag Cloud";

        public void Perform()
        {
            tagGenerator.GetTags(wordsRepository.Get())
                .Then(tags => new TagCloudVisualizer(imageBox, imageSettings, tags))
                .Then(tagCloud => tagCloud.GetTagCloudImage())
                .RefineError("Failed, trying to get a Tag Cloud image")
                .OnFail(exceptionHandler.HandleException);
//            new TagCloudVisualizer(imageBox, imageSettings, tags).GetTagCloudImage();
        }
    }

    
} 