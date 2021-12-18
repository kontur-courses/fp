using System;

namespace TagsCloudContainer.Common.Result
{
    public interface IResultHandler
    {
        void Handle(Action action, string error = null);
        void AddHandledText(string text);
        string GetText();
    }
}