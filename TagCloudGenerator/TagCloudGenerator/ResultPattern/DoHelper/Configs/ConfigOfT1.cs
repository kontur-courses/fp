using System;

namespace TagCloudGenerator.ResultPattern.DoHelper.Configs
{
    public class Config<T1>
    {
        private readonly Action<T1> action;

        public Config(Action<T1> action) => this.action = action;

        public void With(Result<T1> param1) => action(param1.Value);
    }
}