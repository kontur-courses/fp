namespace TagsCloudContainer
{
    public interface IResolver<TKey, TService>
    {
        public Result<TService> Get(TKey key);
    }
}