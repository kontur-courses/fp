using TagsCloudContainer.Algorithm;
using TagsCloudContainer.Infrastucture.Extensions;
using TagsCloudContainer.Infrastucture.Settings;
using TagsCloudContainer.Infrastucture.Visualization;

namespace TagsCloudContainer.Client
{
    public class GUITagCloudClient : ITagCloudClient
    {
        private readonly PictureBox pictureBox;
        private readonly ImageSettings imageSettings;
        private readonly IDrawer drawer;
        private readonly ICloudLayouter cloudLayouter;
        private IWordProcessor wordProcessor;

        public GUITagCloudClient(PictureBox pictureBox, ImageSettings imageSettings,
            IDrawer drawer, ICloudLayouter cloudLayouter, IWordProcessor wordProcessor)
        {
            this.pictureBox = pictureBox;
            this.imageSettings = imageSettings;
            this.drawer = drawer;
            this.cloudLayouter = cloudLayouter;
            this.wordProcessor = wordProcessor;
        }

        public void DrawImage(string sourceFilePath, string boringFilePath)
        {
            var wordsCount = wordProcessor.CalculateFrequencyInterestingWords(sourceFilePath, boringFilePath);

            if (!wordsCount.IsSuccess)
            {
                MessageBox.Show(wordsCount.Error);
                return;
            }
            
            var rectangles = cloudLayouter.GetRectangles(wordsCount.Value);

            if (!rectangles.IsSuccess)
            {
                MessageBox.Show(rectangles.Error);
                return;
            }

            drawer.Draw(rectangles.Value, pictureBox, imageSettings);
        }

        public void SaveImage(string filePath)
        {
            var resultSaving = pictureBox.SaveImage(filePath);

            if (!resultSaving.IsSuccess)
            {
                MessageBox.Show(resultSaving.Error);
                return;
            }
        }
    }
}