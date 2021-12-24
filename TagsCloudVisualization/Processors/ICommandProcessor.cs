namespace TagsCloudVisualization.Processors
{
    public interface ICommandProcessor<T>
    {
        public int Run(T options);
    }
}