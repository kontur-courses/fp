using TagsCloudResult;

namespace TagsCloudBuilder.Drawer
{
    public interface IDrawer
    {
        Result<None> DrawAndSaveWords();
    }
}
