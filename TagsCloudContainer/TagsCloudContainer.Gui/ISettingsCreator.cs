namespace TagsCloudContainer.Gui;

public interface ISettingsCreator<out TSetting>
{
    TSetting? ShowCreate();
}