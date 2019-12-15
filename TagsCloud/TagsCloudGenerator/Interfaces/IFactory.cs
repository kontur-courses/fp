namespace TagsCloudGenerator.Interfaces
{
    public interface IFactory<TResult>
        where TResult : IFactorial
    {
        FailuresProcessing.Result<TResult> CreateSingle();
        FailuresProcessing.Result<TResult[]> CreateArray();
    }
}