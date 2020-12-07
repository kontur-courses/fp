using NUnit.Framework;
using TagCloud.Core.Text;

namespace TagCloud.Core.Tests
{
    public class StubUserNotifier : IUserNotifier
    {
        public void Notify(string message) => Assert.Fail(message);
    }
}