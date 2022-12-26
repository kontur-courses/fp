using System.IO;
using System.Windows.Forms;
using ResultOf;
using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.App.Layouter;

namespace TagsCloudContainer.App.Actions
{
    public class TagsLayouterAction : IUiAction
    {
        private TagsLayouter tagsLayouter;
        private ITagsPainter tagsPainter;
        private readonly ITextReader textReader;
        private readonly IImageHolder imageHolder;

        public TagsLayouterAction(TagsLayouter tagsLayouter, ITagsPainter tagsPainter,
            ITextReader textReader, IImageHolder imageHolder)
        {
            this.tagsLayouter = tagsLayouter;
            this.tagsPainter = tagsPainter;
            this.textReader = textReader;
            this.imageHolder = imageHolder;
        }

        public string Category => "Облако тегов";
        public string Name => "Облако тегов";
        public string Description => "Облако тегов";

        public void Perform()
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = false,
                InitialDirectory = Path.GetFullPath("."),
                Filter = textReader.Filter
            };
            var res = dialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                var imageSize = imageHolder.GetImageSize();
                if (!imageSize.IsSuccess)
                {
                    MessageBox.Show(imageSize.Error);
                    return;
                }
                var graphics = imageHolder.StartDrawing();
                if (!graphics.IsSuccess)
                {
                    MessageBox.Show(graphics.Error);
                    return;
                }
                var result = textReader.ReadText(dialog.FileName)
                    .Then(text => tagsLayouter.PutAllTags(text, imageSize.Value))
                    .Then(tags => tagsPainter.Paint(tags, imageSize.Value, graphics.Value));
                if (!result.IsSuccess)
                    MessageBox.Show(result.Error);
                else
                    imageHolder.UpdateUi();
            }
        }
    }
}