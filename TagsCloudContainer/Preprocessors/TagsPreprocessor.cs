using System;
using System.Collections.Generic;
using System.Linq;
using TagsCloudContainer.Common;
using TagsCloudContainer.Extensions;

namespace TagsCloudContainer.Preprocessors
{
    public class TagsPreprocessor
    {
        private readonly IPreprocessor[] preprocessors;
        public static readonly Type[] AllPreprocessors = AppDomain.CurrentDomain.GetAssemblies()
            .First(a => a.FullName.Contains("TagsCloudContainer"))
            .GetTypes()
            .Where(t => t.IsInstanceOf<IPreprocessor>())
            .ToArray();

        public TagsPreprocessor(IPreprocessor[] preprocessors)
        {
            this.preprocessors = (from preprocessor in preprocessors
                    let prop = preprocessor.GetType()
                        .GetProperty(nameof(State))
                    where (State) prop.GetValue(null) == State.Active
                    select preprocessor)
                .ToArray();
        }

        public List<SimpleTag> Process(IEnumerable<SimpleTag> tags)
        {
            var processedTags = preprocessors.Aggregate
                    (tags, (current, preprocessor) => preprocessor.Process(current))
                .ToList();
            if (processedTags.Count == 0)
                throw new Exception("With this preprocessors, you can`t get any tags, and then can`t visualize it");
            return processedTags;
        }

        public static Type[] GetActivPreprocessors()
            => AppDomain.CurrentDomain.GetAssemblies()
                .First(a => a.FullName.Contains("TagsCloudContainer"))
                .GetTypes()
                .Where(t => t.IsInstanceOf<IPreprocessor>())
                .ToArray();
    }
}