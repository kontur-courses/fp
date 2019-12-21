using System;
using TagCloudGenerator.ResultPattern.DoHelper.Configs;

namespace TagCloudGenerator.ResultPattern.DoHelper
{
    public static class Do
    {
        public static Config<T1> Call<T1>(Action<T1> action) => new Config<T1>(action);

        public static Config<T1, T2, T3, TResult> Call<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func) =>
            new Config<T1, T2, T3, TResult>(func);
    }
}