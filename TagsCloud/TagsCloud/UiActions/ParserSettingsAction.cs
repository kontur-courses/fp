using System.Collections.Generic;
using TagsCloud.GUI;

namespace TagsCloud.UiActions
{
    public class ParserSettingsAction : IUiAction
    {
        private readonly HashSet<string> wordsToIgnore;
        public ParserSettingsAction(HashSet<string> wordsToIgnore)
        {
            this.wordsToIgnore = wordsToIgnore;
        }

        public string Category => "Настройки";
        public string Name => "Настройка отбора слов";
        public string Description => "Настройка отбора слов";

        public void Perform()
        {
            new ParserSettingsForm(wordsToIgnore).Show();
        }
    }
}
