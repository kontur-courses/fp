namespace TagsCloud.WordValidators
{
    public class WordValidatorSettings
    {
        public readonly string[] ignoringPartsOfSpeech;

        public WordValidatorSettings(TagCloudSettings tagCloudSettings)
        {
            ignoringPartsOfSpeech = tagCloudSettings.ignoredPartOfSpeech;
        }
    }
}
