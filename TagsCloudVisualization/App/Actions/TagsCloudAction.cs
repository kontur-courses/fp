using System;
using System.Windows.Forms;
using TagsCloudVisualization.TagsCloud.CircularCloud;

namespace TagsCloudVisualization.App.Actions
{
    public class TagsCloudAction : IUiAction
    {
        public string Name => "Облако тэгов";
        public string Category => "Изображение";
        private readonly TagsCloudVisualizer visualizer;
        private readonly PictureBoxImageHolder imageHolder;

        public TagsCloudAction(TagsCloudVisualizer visualizer, PictureBoxImageHolder imageHolder)
        {
            this.imageHolder = imageHolder;
            this.visualizer = visualizer;
        }
        public void Perform()
        {
            try
            {
                CircularCloudLayouter.IsCompressedCloud = false;
                var image = visualizer.DrawCircularCloud();
                imageHolder.RecreateImage(image);
                imageHolder.Refresh();
                Application.DoEvents();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}