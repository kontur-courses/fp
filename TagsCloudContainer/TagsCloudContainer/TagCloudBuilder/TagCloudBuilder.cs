using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using Autofac;
using System.IO;
using System;

namespace TagsCloudContainer
{
    public class TagCloudBuilder : ITagCloudBuilder
    {
        private readonly TextHandler fileHandler;
        private readonly ITagCloudBuildingAlgorithm algorithmToBuild;

        public TagCloudBuilder(TextHandler fileHandler,
            ITagCloudBuildingAlgorithm algorithmToBuild)
        {
            this.fileHandler = fileHandler;
            this.algorithmToBuild = algorithmToBuild;
        }

        public Result<IEnumerable<Tag>> GetTagsCloud()
        {
            var frequencyDictResult = fileHandler.GetWordsFrequencyDict();
            if (!frequencyDictResult.IsSuccess)
                return Result.Fail<IEnumerable<Tag>>(frequencyDictResult.Error);
            var tagsCloudResult = algorithmToBuild.GetTags(frequencyDictResult.Value);
            if (!tagsCloudResult.IsSuccess)
                return Result.Fail<IEnumerable<Tag>>(tagsCloudResult.Error);
            return tagsCloudResult;
        }
    }
}