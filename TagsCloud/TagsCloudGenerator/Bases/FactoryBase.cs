using FailuresProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TagsCloudGenerator.Attributes;
using TagsCloudGenerator.Interfaces;

namespace TagsCloudGenerator.Bases
{
    public abstract class FactoryBase<TFactorial> : IFactory<TFactorial>
        where TFactorial : IFactorial
    {
        protected readonly Dictionary<string, TFactorial> factorials;
        protected readonly IFactorySettings factorySettings;
        protected readonly Func<IFactorySettings, string> getSingleFactorialId;
        protected readonly Func<IFactorySettings, string[]> getFactorialsIdsArray;

        public FactoryBase(
            TFactorial[] factorials,
            IFactorySettings factorySettings,
            Func<IFactorySettings, string> getSingleFactorialId, 
            Func<IFactorySettings, string[]> getFactorialsIdsArray)
        {
            this.factorials = factorials.ToDictionary(factorial =>
                factorial
                .GetType()
                .GetCustomAttribute<FactorialAttribute>()
                .FactorialId);
            this.factorySettings = factorySettings;
            this.getSingleFactorialId = getSingleFactorialId;
            this.getFactorialsIdsArray = getFactorialsIdsArray;
        }

        public virtual Result<TFactorial[]> CreateArray() =>
            CheckFactorialIds(getFactorialsIdsArray(factorySettings))
                .Then(ids => ids.Select(id => factorials[id]).ToArray())
                .RefineError($"Failed to create array of {typeof(TFactorial).Name}");

        public virtual Result<TFactorial> CreateSingle() =>
            CheckFactorialIds(new[] { getSingleFactorialId(factorySettings) })
                .Then(ids => ids.Select(id => factorials[id]).First())
                .RefineError($"Failed to create single element of {typeof(TFactorial).Name}");

        private Result<string[]> CheckFactorialIds(string[] factorialIds)
        {
            var sb = new StringBuilder();
            foreach (var factorialId in factorialIds)
                if (!factorials.ContainsKey(factorialId))
                    sb.Append($" \'{factorialId}\'");
            return
                sb.Length == 0 ?
                Result.Ok(factorialIds) :
                Result.Fail<string[]>($"Unknown ids:{sb}");
        }
    }
}