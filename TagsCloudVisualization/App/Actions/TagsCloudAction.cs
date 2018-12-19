using System.Windows.Forms;
using TagsCloudVisualization.InterfacesForSettings;
using TagsCloudVisualization.TagsCloud;
using TagsCloudVisualization.TagsCloud.CircularCloud;

namespace TagsCloudVisualization.App.Actions
{
    public class TagsCloudAction : IUiAction
    {
        public string Name => "Облако тэгов";
        public string Category => "Изображение";

        private readonly TagsCloudVisualizer visualizer;
        private readonly PictureBoxImageHolder imageHolder;
        private readonly ITagsCloudSettings tagsCloudSettings;

        public TagsCloudAction(TagsCloudVisualizer visualizer,
            PictureBoxImageHolder imageHolder, ITagsCloudSettings tagsCloudSettings)
        {
            this.tagsCloudSettings = tagsCloudSettings;
            this.imageHolder = imageHolder;
            this.visualizer = visualizer;
        }
        public void Perform()
        {
            tagsCloudSettings.TypeTagsCloud = TypeTagsCloud.TagsCloud;
            var resultImage = visualizer.DrawCircularCloud();
            if (resultImage.IsSuccess)
            {
                var image = resultImage.Value;
                imageHolder.RecreateImage(image);
                imageHolder.Refresh();
                Application.DoEvents();
            }
            else
            {
                MessageBox.Show(resultImage.Error, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}