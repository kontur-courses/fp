using Autofac;
using TagCloud.Utility.Container;
using TagCloud.Utility.Runner;

namespace TagCloud.Utility
{
    public static class TagCloudProgram
    {
        public static Result<None> Execute(Options options)
        {
            return options.AsResult()
                .Then(CheckOptionsOnNull)
                .Then(Helper.CheckPaths)
                .Then(ContainerConfig.Configure)
                .Then(ResolveRunner)
                .Then(RunProgram);
        }

        private static Result<None> RunProgram(ITagCloudRunner container)
        {
            return Result
                .OfAction(container.Run)
                .RefineError("Error while running a program");
        }

        private static Result<ITagCloudRunner> ResolveRunner(IContainer container)
        {
            return Result
                .Of(container.Resolve<ITagCloudRunner>)
                .RefineError("Error while resolving runner");
        }

        private static Result<Options> CheckOptionsOnNull(Options options)
        {
            return options != null
                ? Result.Ok(options)
                : Result.Fail<Options>($"{nameof(options)} was null)");
        }
    }
}