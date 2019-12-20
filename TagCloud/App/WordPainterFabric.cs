using System;
using System.Collections.Generic;
using System.Linq;
using ResultOf;
using TagCloud.Visualization.WordPainting;

namespace TagCloud.App
{
    public class WordPainterFabric : IWordPainterFabric
    {
        private readonly IEnumerable<IWordPainter> painters;
        private readonly ISettingsProvider settingsProvider;
        private IWordPainter painter;

        public WordPainterFabric(IEnumerable<IWordPainter> painters, ISettingsProvider settingsProvider)
        {
            this.painters = painters;
            this.settingsProvider = settingsProvider;
        }

        public Result<IWordPainter> GetWordPainter()
        {
            if (painter != null)
                return Result.Ok(painter);
            var settingsResult = settingsProvider.GetSettings();
            if (!settingsResult.IsSuccess)
                return Result.Fail<IWordPainter>(settingsResult.Error);
            var name = settingsResult.GetValueOrThrow().WordPainterAlgorithmName;
            painter = painters.FirstOrDefault(p => p.Name == name);
            if (painter == null)
                throw new ArgumentException($"Unknown painter algorithm: {name}");
            return Result.Ok(painter);
        }
    }
}