namespace TagsCloudContainer.ConsoleInterface.ConsoleActions
{
    public interface IUIAction
    {
        string GetDescription();

        void Handle();
    }
}
