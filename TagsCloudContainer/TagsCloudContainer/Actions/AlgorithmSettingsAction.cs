using TagsCloudContainer.Infrastructure;
using TagsCloudContainer.Services;

namespace TagsCloudContainer.Actions
{
    public class AlgorithmSettingsAction : IUiAction
    {
        private AlgorithmSettings algoSettings;
        private readonly ITagCloudService _tagCloudService;

        public AlgorithmSettingsAction(ITagCloudService tagCloudService, AlgorithmSettings algoSettings)
        {
            this._tagCloudService = tagCloudService;
            this.algoSettings = algoSettings;
        }

        public string Category => "Алгоритм";
        public string Name => "Настройки...";
        public string Description => "Изменить настройки алгоритма";
        public void Perform()
        {
            _tagCloudService.SetSettings(algoSettings);
        }
    }
}
