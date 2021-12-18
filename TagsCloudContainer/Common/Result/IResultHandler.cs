using System;

namespace TagsCloudContainer.Common.Result
{
    public interface IResultHandler
    {
        void Handle(Action action);
    }
}