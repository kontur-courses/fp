namespace TagsCloudContainer.Gui;

public interface ISettingsEditor<T>
{
    T ShowEdit(T settings);
}