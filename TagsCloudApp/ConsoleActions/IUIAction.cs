namespace TagsCloudApp.ConsoleActions;

public interface IUIAction
{
    string GetDescription();

    void Handle();
}