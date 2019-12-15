namespace TagsCloudGenerator.Interfaces
{
    public interface IExecutable<TIn, TOut>
    {
        FailuresProcessing.Result<TOut> Execute(TIn input);
    }
}