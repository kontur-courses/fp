using System.Drawing;
using System.Windows.Forms;
using TagsCloud.ClientGUI.Infrastructure;
using TagsCloud.Common;
using TagsCloud.ResultPattern;
using TagsCloud.Visualization;

namespace TagsCloud.ClientGUI.Actions
{
    public class TagsCloudAction : IUiAction
    {
        private readonly ICloudLayouterFactory cloudFactory;
        private readonly TagsCloudPainter tagsCloudPainter;
        private readonly IImageHolder imageHolder;

        public TagsCloudAction(IImageHolder imageHolder,
            TagsCloudPainter tagsCloudPainter, ICloudLayouterFactory cloudFactory)
        {
            this.tagsCloudPainter = tagsCloudPainter;
            this.cloudFactory = cloudFactory;
            this.imageHolder = imageHolder;
        }

        public string Category => "Облако тегов";
        public string Name => "Нарисовать облако тегов";
        public string Description => "Рисование облака тегов по спирали";

        public void Perform()
        {
            var result = imageHolder.GetImageSize()
                .Then(size => new Point(size.Width / 2, size.Height / 2))
                .Then(center => tagsCloudPainter.Paint(cloudFactory.CreateCircularLayouter(center)));

            if (!result.IsSuccess)
                MessageBox.Show(result.Error);
        }
    }
}