using System;
using TagCloudContainer.Result;
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

        public Result<ICloudLayouterAlgorithm> Create(Spiral spiral)
        {
            var actualSettings = settingsProvider.Invoke();
            return actualSettings.Layouter switch
            {
                "d" => new Result<ICloudLayouterAlgorithm>(null, new CircularCloudLayouter(spiral)),
                _ => new Result<ICloudLayouterAlgorithm>("Wrong algorithm name")
            };
        }
    }
}