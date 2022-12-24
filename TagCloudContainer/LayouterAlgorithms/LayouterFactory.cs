using System;
using TagCloudContainer.TaskResult;
using TagCloudContainer.UI;

namespace TagCloudContainer.LayouterAlgorithms
{
    public class LayouterFactory
    {
        private readonly Func<IUi> settingsProvider;

        public LayouterFactory(Func<IUi> settingsProvider)
        {
            this.settingsProvider = settingsProvider;
        }

        public Result<ICloudLayouterAlgorithm> Create()
        {
            var actualSettings = settingsProvider.Invoke();
            return actualSettings.Layouter switch
            {
                "d" => Result.OnSuccess(new CircularCloudLayouter(new Spiral(actualSettings)) as ICloudLayouterAlgorithm),
                _ => Result.OnFail<ICloudLayouterAlgorithm>("Wrong algorithm name")
            };
        }
    }
}