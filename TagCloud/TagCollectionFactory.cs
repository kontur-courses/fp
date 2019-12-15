using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using ResultOf;
using TagCloud.IServices;
using TagCloud.Models;

namespace TagCloud
{
    public class TagCollectionFactory : ITagCollectionFactory
    {
        private readonly IWordsToTagsParser parser;
        private readonly IWordsHandler wordsHandler;

        public TagCollectionFactory(IWordsHandler wordsHandler, IWordsToTagsParser parser)
        {
            this.wordsHandler = wordsHandler;
            this.parser = parser;
        }

        public Result<List<Tag>> Create(ImageSettings imageSettings, string path)
        {
            //var primaryCollection = wordsHandler.GetWordsAndCount(path);
            //var collectionAfterConversion = wordsHandler.Conversion(primaryCollection);
            //return parser.GetTagsRectangles(collectionAfterConversion, imageSettings)
            //    .OrderByDescending(t => t.Count)
            //    .ToList();
            return wordsHandler.GetWordsAndCount(path)
                .Then(wordsAndCount => wordsHandler.Conversion(wordsAndCount))
                .Then(wordsAfterConversion => parser.GetTagsRectangles(wordsAfterConversion,imageSettings)
                    .OrderByDescending(t => t.Count)
                    .ToList());
        }
    }
}