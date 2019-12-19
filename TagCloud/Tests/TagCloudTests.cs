using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using TagCloud.TagCloudPainter;
using TagCloud.TextPreprocessor.Core;
using TagCloud.TextPreprocessor.TextAnalyzers;
using TagCloud.TextPreprocessor.TextRiders;

namespace TagCloud.Tests
{
    [TestFixture]
    public class TagCloudTests
    {
        
        
        [Test]
        public void FirstTest()
        {
            var fakeTextRider = A.Fake<IFileTextRider>();
//            var fakeTextRiderConfig = A.Fake<TextRiderConfig>();
//            A.CallTo(() => fakeTextRiderConfig.FilePath).Returns("test.txt");
//            A.CallTo(() => fakeTextRider.RiderConfig).Returns(fakeTextRiderConfig);
//            A.CallTo(() => fakeTextRider.ReadingFormats).Returns(new[] {".txt"});
            var fakeTextAnalyzer = A.Fake<ITextAnalyzer>();
            var fakeCloudPainter = A.Fake<ITagCloudPainter>();
            
            TagCloudCreator.Create(new [] { fakeTextRider }, fakeTextAnalyzer, fakeCloudPainter);

            A.CallTo(() => fakeTextRider.GetTags()).MustHaveHappened();
            A.CallTo(() => fakeTextAnalyzer.GetTagInfo(A<IEnumerable<Tag>>.Ignored)).MustHaveHappened();
            A.CallTo(() => fakeCloudPainter.Draw(A<IEnumerable<TagInfo>>.Ignored)).MustHaveHappened();
        }
    }
}