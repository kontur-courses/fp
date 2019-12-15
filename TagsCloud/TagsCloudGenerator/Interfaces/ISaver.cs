namespace TagsCloudGenerator.Interfaces
{
    public interface ISaver : IFactorial
    {
        FailuresProcessing.Result<FailuresProcessing.None> SaveTo(string filePath, System.Drawing.Bitmap bitmap);
    }
}