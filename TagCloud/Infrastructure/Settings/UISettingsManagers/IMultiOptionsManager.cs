using System.Collections.Generic;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers
{
    public interface IMultiOptionsManager : IInputManager
    {
        public Dictionary<string, IEnumerable<string>> GetOptions();
        public Dictionary<string, string> GetSelectedOptions();
    }
}