using System.Drawing;
using System.Windows.Forms;
using TagsCloud.Common;
using TagsCloud.Core;
using TagsCloud.ResultPattern;
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

        public Result<Graphics> Paint(ICircularCloudLayouter cloud)
        {
            var result = tagsHelper.GetWords()
                .Then(words => visualizer.Paint(cloud, words));

            PictureBox.Refresh();
            Application.DoEvents();

            return result;
        }
    }
}