using ResultOf;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers
{
    public interface ISettingsManager
    {
        public string Title { get; }
        public string Help { get; }
        public Result<string> TrySet(string path);
        public string Get();
    }
}