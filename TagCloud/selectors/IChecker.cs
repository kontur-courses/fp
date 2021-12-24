namespace TagCloud.selectors
{
    public interface IChecker<T>
    {
        Result<T> IsValid(T source);
    }
}