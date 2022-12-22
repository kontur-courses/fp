using System;
using TagCloudContainer.Result;
using TagCloudContainer.UI;

namespace TagCloudContainer.WordsColoringAlgorithms
{
    public class WordsColoringFactory
    {
        private readonly Func<IUi> settingsProvider;

        public WordsColoringFactory(Func<IUi> settingsProvider)
        {
            this.settingsProvider = settingsProvider;
        }

        public Result<IWordsPainter> Create()
        {
            var actualSettings = settingsProvider.Invoke();
            return actualSettings.WordsColoringAlgorithm switch
            {
                "d" => new Result<IWordsPainter>(null, new DefaultWordsPainter()),
                "gd" => new Result<IWordsPainter>(null, new GradientDependsOnSizePainter()),
                "g" => new Result<IWordsPainter>(null, new GradientWordsPainter()),
                _ => new Result<IWordsPainter>("Wrong coloring algorithm")
            };
        }
    }
}