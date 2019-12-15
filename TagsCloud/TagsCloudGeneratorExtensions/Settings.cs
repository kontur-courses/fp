using FailuresProcessing;
using TagsCloudGenerator.Interfaces;
using TagsCloudGenerator.Settings;

namespace TagsCloudGeneratorExtensions
{
    public class Settings : GeneratorSettings
    {
        public Settings(IPainterSettings painterSettings, IFactorySettings factorySettings) :
            base(painterSettings, factorySettings)
        {
            Reset();
        }

        public string[] TakenPartsOfSpeech { get; set; }

        public override Result<None> Reset()
        {
            TakenPartsOfSpeech = new string[0];
            return base.Reset();
        }
    }
}