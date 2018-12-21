namespace CloodLayouter.Infrastructer
{
    public interface IConverter<TInput, TOutput>
    {
        TOutput Convert(TInput data);
    }
}