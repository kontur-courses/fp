namespace TagsCloudGenerator.Interfaces
{
    internal interface IExecutor<TIn, TOut>
    {
        FailuresProcessing.Result<TOut> Execute(TIn input);
    }
}