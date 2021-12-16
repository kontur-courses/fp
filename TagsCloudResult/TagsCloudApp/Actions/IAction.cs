using TagsCloudContainer;

namespace TagsCloudApp.Actions
{
    public interface IAction
    {
        public Result<None> Perform();
    }
}