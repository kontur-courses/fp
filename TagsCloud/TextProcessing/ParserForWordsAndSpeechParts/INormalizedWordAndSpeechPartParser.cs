namespace TagsCloud.TextProcessing.ParserForWordsAndSpeechParts
{
    public interface INormalizedWordAndSpeechPartParser
    {
        string[] ParseToNormalizedWordAndPartSpeech(string text);
    }
}