using FailuresProcessing;
using System.Linq;
using System.Reflection;
using TagsCloudGenerator.Attributes;
using TagsCloudGenerator.Interfaces;

namespace TagsCloudGenerator.Executors
{
    internal class PriorityExecutor<T> : IExecutor<T, T>
    {
        private readonly IExecutable<T, T>[] executables;

        public PriorityExecutor(IExecutable<T, T>[] executables) => 
            this.executables = executables
                .OrderByDescending(e => e
                    .GetType()
                    .GetCustomAttribute<PriorityAttribute>()
                    .Priority)
                .ToArray();

        public Result<T> Execute(T input)
        {
            var result = Result.Ok(input);
            foreach (var executable in executables)
                result = result.Then(z => executable.Execute(z));
            return result;
        }
    }
}