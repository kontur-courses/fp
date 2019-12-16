namespace TagsCloudGenerator.Interfaces
{
    public interface IPainterAndSaver
    {
        FailuresProcessing.Result<FailuresProcessing.None> PaintAndSave(
            DTO.WordDrawingDTO[] layoutedWords,
            string pathToSave);
    }
}