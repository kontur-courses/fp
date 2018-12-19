using System;
using TagsCloudContainer.CloudBuilder;
using TagsCloudContainer.CloudDrawers;
using TagsCloudContainer.FileReaders;
using TagsCloudContainer.TextParsers;

namespace TagsCloudContainer.CloudTagController
{
    public class CloudTagController : ICloudTagController
    {
        private readonly ICloudBuilder cloudBuilder;
        private readonly ICloudDrawer cloudDrawer;
        private readonly IFileReader fileReader;
        private readonly ITextParser textParser;

        public CloudTagController(IFileReader fileReader, ITextParser textParser, ICloudDrawer cloudDrawer,
            ICloudBuilder cloudBuilder)
        {
            this.fileReader = fileReader;
            this.textParser = textParser;
            this.cloudDrawer = cloudDrawer;
            this.cloudBuilder = cloudBuilder;
        }

        public void Work()
        {
            fileReader.Read()
                .OnFail(exception => Console.WriteLine(exception))
                .Then(text => textParser.Parse(text))
                .OnFail(exception => Console.WriteLine(exception))
                .Then(wordFrequency => cloudBuilder.BuildTagsCloud(wordFrequency))
                .OnFail(exception => Console.WriteLine(exception))
                .Then(tagsCloud => cloudDrawer.Draw(tagsCloud))
                .OnFail(exception => Console.WriteLine(exception));
        }
    }
}