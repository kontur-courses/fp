using System.Collections.Generic;

namespace TagCloud.Infrastructure.Settings.UISettingsManagers
{
    public interface IOptionsManager : IInputManager
    {
        public IEnumerable<string> GetOptions();
    }
}