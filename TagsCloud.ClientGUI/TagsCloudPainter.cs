using System.Windows.Forms;
using TagsCloud.Common;
using TagsCloud.Core;
using TagsCloud.Visualization;

namespace TagsCloud.ClientGUI
{
    public class TagsCloudPainter
    {
        private readonly CloudVisualization visualizer;
        private readonly TagsHelper tagsHelper;

        public TagsCloudPainter(PictureBoxImageHolder pictureBox, 
            CloudVisualization visualizer, TagsHelper tagsHelper)
        {
            PictureBox = pictureBox;
            this.visualizer = visualizer;
            this.tagsHelper = tagsHelper;
        }

        public PictureBoxImageHolder PictureBox { get; }

        public void Paint(ICircularCloudLayouter cloud)
        {
            visualizer.Paint(cloud, tagsHelper.GetWords().GetValueOrThrow());
            PictureBox.Refresh();
            Application.DoEvents();
        }
    }
}