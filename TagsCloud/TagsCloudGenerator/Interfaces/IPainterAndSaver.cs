namespace TagsCloudGenerator.Interfaces
{
    public interface IPainterAndSaver
    {
        FailuresProcessing.Result<FailuresProcessing.None> PaintAndSave(
            (string word, float maxFontSymbolWidth, string fontName, System.Drawing.RectangleF wordRectangle)[] layoutedWords,
            string pathToSave);
    }
}