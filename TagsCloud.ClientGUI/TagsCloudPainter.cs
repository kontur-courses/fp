using System.Windows.Forms;
using TagsCloud.ClientGUI.Infrastructure;
using TagsCloud.Common;
using TagsCloud.Core;
using TagsCloud.Visualization;

namespace TagsCloud.ClientGUI
{
    public class TagsCloudPainter
    {
        private readonly PathSettings pathSettings;
        private readonly CloudVisualization visualizer;
        private readonly TagsHelper tagsHelper;

        public TagsCloudPainter(PictureBoxImageHolder pictureBox, PathSettings pathSettings,
            CloudVisualization visualizer, TagsHelper tagsHelper)
        {
            PictureBox = pictureBox;
            this.pathSettings = pathSettings;
            this.visualizer = visualizer;
            this.tagsHelper = tagsHelper;
        }

        public PictureBoxImageHolder PictureBox { get; }

        public void Paint(ICircularCloudLayouter cloud)
        {
            var words = tagsHelper.GetWords(pathSettings.PathToText, pathSettings.PathToBoringWords);

            visualizer.Paint(cloud, words.GetValueOrThrow());

            PictureBox.Refresh();
            Application.DoEvents();
        }
    }
}