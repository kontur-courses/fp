using System;
using System.Collections.Generic;
using System.Drawing;
using TagCloudGenerator.Clients;
using TagCloudGenerator.GeneratorCore.TagClouds;
using TagCloudGenerator.GeneratorCore.Tags;

namespace TagCloudGenerator_Tests.TestFixtures
{
    public class CloudOptions<TTagCloud> : ITagCloudOptions<TTagCloud> where TTagCloud : ITagCloud
    {
        public string CloudVocabularyFilename { get; set; }
        public string ImageSize { get; set; }
        public string ExcludedWordsVocabularyFilename { get; set; }
        public string ImageFilename { get; set; }
        public int GroupsCount { get; set; }
        public string MutualFont { get; set; }
        public string BackgroundColor { get; set; }
        public string FontSizes { get; set; }
        public string TagColors { get; set; }

        public TTagCloud ConstructCloud(Color backgroundColor, Dictionary<TagType, TagStyle> tagStyleByTagType) =>
            (TTagCloud)Activator.CreateInstance(typeof(TTagCloud), backgroundColor, tagStyleByTagType);
    }
}