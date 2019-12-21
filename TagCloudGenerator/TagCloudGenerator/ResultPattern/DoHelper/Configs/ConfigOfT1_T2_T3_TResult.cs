using System;

namespace TagCloudGenerator.ResultPattern.DoHelper.Configs
{
    public class Config<T1, T2, T3, TResult>
    {
        private readonly Func<T1, T2, T3, TResult> func;

        public Config(Func<T1, T2, T3, TResult> func) => this.func = func;

        public TResult With(Result<T1> param1, Result<T2> param2, Result<T3> param3) =>
            func(param1.Value, param2.Value, param3.Value);
    }
}