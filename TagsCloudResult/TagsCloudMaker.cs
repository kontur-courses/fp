using TagsCloudResult.ApplicationRunning;

namespace TagsCloudResult
{
    public class TagsCloudMaker
    {
        private IAppRunner runner;

        public TagsCloudMaker(IAppRunner runner)
        {
            this.runner = runner;
            runner.Run();
        }
    }
}