namespace TagsCloudGenerator.Interfaces
{
    public interface IPainter : IFactorial
    {
        FailuresProcessing.Result<FailuresProcessing.None> DrawWords(
            DTO.WordDrawingDTO[] layoutedWords,
            System.Drawing.Graphics graphics);
    }
}