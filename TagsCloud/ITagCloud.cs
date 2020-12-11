using TagsCloud.Result;

namespace TagsCloud
{
    public interface ITagCloud
    {
        public Result<None> MakeTagCloud();

        public Result<None> SaveTagCloud();
    }
}