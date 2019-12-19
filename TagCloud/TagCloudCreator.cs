using System;
using System.IO;
using System.Linq;
using TagCloud.TagCloudPainter;
using TagCloud.TextPreprocessor.TextAnalyzers;
using TagCloud.TextPreprocessor.TextRiders;
using TagsCloud;

namespace TagCloud
{
    public static class TagCloudCreator
    {
        public static Result<None> Create(
            IFileTextRider[] fileTextRiders,
            ITextAnalyzer textAnalyzer, 
            ITagCloudPainter tagCloudPainter)
        {
            if(fileTextRiders.Length == 0)
                return Result.Fail<None>(new ArgumentException());
            
            return Result.Of(() => GetCorrectTextRider(fileTextRiders))
                .Then(rider => rider.GetTags())
                .Then(textAnalyzer.GetTagInfo)
                .Then(tagCloudPainter.Draw);
        }

        private static IFileTextRider GetCorrectTextRider(IFileTextRider[] textRiders)
        {
            var riderConfig = textRiders.First().RiderConfig;
            var fileExtension = Path.GetExtension(riderConfig.FilePath);
            
            var textRider = textRiders
                .FirstOrDefault(rider => rider.ReadingFormats.Contains(fileExtension));
            
            if(textRider == null)
                throw new Exception("It is not possible to read a file of this format");

            return textRider;
        }
    }
}