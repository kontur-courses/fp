using ResultOf;

namespace TagsCloudVisualization.TextInput;

public interface ITextInput
{
    Result<string> GetInputString();
}