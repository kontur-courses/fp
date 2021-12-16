using TagsCloudContainer.Results;

namespace TagsCloudApp.Actions
{
    public interface IAction
    {
        public Result<None> Perform();
    }
}