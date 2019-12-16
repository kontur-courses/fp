namespace TagsCloudGenerator.Interfaces
{
    public interface IWordsLayouter : IFactorial
    {
        FailuresProcessing.Result<DTO.WordDrawingDTO[]> ArrangeWords(string[] words);
    }
}