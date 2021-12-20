using Mono.Options;
using ResultOf;
using TagsCloudContainer.Abstractions;

namespace TagsCloudContainer.Defaults.SettingsProviders;

public class InputSettings : IRequiredSettingsProvider
{
    public bool UseString { get; private set; }
    public bool UseFile { get; private set; }

    public string[] Paths { get; private set; } = Array.Empty<string>();
    public string Source { get; private set; } = string.Empty;
    public bool IsSet => UseFile || UseString;

    public Result State { get; private set; } = Result.Ok();

    public OptionSet GetCliOptions()
    {
        var options = new OptionSet()
            {
                {"files=", $"Specifies input files separated by '|'.", v =>
                    {
                        if (UseString)
                        {
                            State = Result.Fail("Can't use both file and string input providers");
                        }
                        else
                        {
                            Paths = v.Split('|');
                            UseFile = true;
                        }
                    }
                },
                {"string=", $"Specifies string to read.", v =>
                    {
                        if (UseFile)
                        {
                            State = Result.Fail("Can't use both file and string input providers");
                        }
                        else
                        {
                            Source = v;
                            UseString = true;
                        }
                    }
                }
            };

        return options;
    }
}