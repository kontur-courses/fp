using NUnit.Framework;
using TagsCloudResult;
using TagsCloudResult.Infrastructure.Common;
using TagsCloudResult.Layouter;

namespace TagsCloudResult_Tests
{
    [TestFixture]
    public class TagCLoudTests
    {
        [Test]
        public void GeneralTests()
        {
            var layouter = new CircularCloudLayouter();
            var tagCloud = new TagCloud(AppSettingsForTests.Settings);
            tagCloud.Create(WordReaderFromFile.ReadWords,
                BasicWordsSelector.Select,
                Compositor.Composite,
                CloudDrawer.Draw,
                ImageCreator.Save,
                layouter.PutNextRectangle);
        }
        
    }
}