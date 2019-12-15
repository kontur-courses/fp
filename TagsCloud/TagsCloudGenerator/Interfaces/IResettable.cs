namespace TagsCloudGenerator.Interfaces
{
    public interface IResettable
    {
        FailuresProcessing.Result<FailuresProcessing.None> Reset();
    }
}