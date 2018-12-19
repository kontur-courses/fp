using System;
using System.Windows.Forms;
using TagsCloudVisualization.InterfacesForSettings;
using TagsCloudVisualization.TagsCloud;
using TagsCloudVisualization.TagsCloud.CircularCloud;

namespace TagsCloudVisualization.App.Actions
{
    public class CompressedTagsCloudAction : IUiAction
    {
        public string Name => "Сжатое облако тэгов";
        public string Category => "Изображение";
        private readonly TagsCloudVisualizer visualizer;
        private readonly PictureBoxImageHolder imageHolder;
        private readonly ITagsCloudSettings tagsCloudSettings;

        public CompressedTagsCloudAction(TagsCloudVisualizer visualizer,
            PictureBoxImageHolder imageHolder, ITagsCloudSettings tagsCloudSettings)
        {
            this.tagsCloudSettings = tagsCloudSettings;
            this.imageHolder = imageHolder;
            this.visualizer = visualizer;
        }
        public void Perform()
        {
            tagsCloudSettings.TypeTagsCloud = TypeTagsCloud.CompressedTagsCloud;
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