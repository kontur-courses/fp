namespace TagsCloudVisualization.Interfaces
{
    public interface IVisualizator
    {
        public void Visualize(IVisualizatorSettings settings, ICloud cloud);
    }
}
