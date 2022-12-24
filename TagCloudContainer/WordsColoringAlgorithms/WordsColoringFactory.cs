using System;
using TagCloudContainer.TaskResult;
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
                "d" => Result.OnSuccess(new DefaultWordsPainter() as IWordsPainter),
                "gd" => Result.OnSuccess(new GradientDependsOnSizePainter() as IWordsPainter),
                "g" => Result.OnSuccess(new GradientWordsPainter() as IWordsPainter),
                _ => Result.OnFail<IWordsPainter>("Wrong coloring algorithm")
            };
        }
    }
}