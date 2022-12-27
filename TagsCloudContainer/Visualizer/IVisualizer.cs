namespace TagsCloudContainer.Visualizer
{
    public interface IVisualizer
    {
        public Result<None> Visualize();
        public void Save();
    }
}