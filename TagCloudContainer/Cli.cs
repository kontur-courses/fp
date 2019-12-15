using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TagCloudContainer.Api;
using TagCloudContainer.fluent;
using TagCloudContainer.Implementations;
using TagCloudContainer.ResultMonad;

namespace TagCloudContainer
{
    public class Cli
    {
        public static Dictionary<string, Func<string, IWordProvider>> wordProviders =
            new Dictionary<string, Func<string, IWordProvider>>
                {{"txt", inputFile => new TxtFileReader(inputFile)}};

        private Dictionary<string, Dictionary<string, Type>> cliElements =
            new Dictionary<string, Dictionary<string, Type>>();

        public void CollectCliElements()
        {
            var cliAttributeType = typeof(CliElementAttribute);
            var cliRoleType = typeof(CliRoleAttribute);

            var assembly = Assembly.GetAssembly(cliAttributeType);
            foreach (var type in assembly.DefinedTypes.Where(t =>
                t.GetCustomAttributes(cliAttributeType).Any()))
            {
                var attributes = type.GetCustomAttributes(cliAttributeType).Select(t => (CliElementAttribute) t);
                var roleName = type.ImplementedInterfaces.First(i =>
                    i.GetCustomAttributes(cliRoleType).Any(a => a.GetType() == cliRoleType)).Name;
                var attribute = attributes.First();

                if (!cliElements.ContainsKey(roleName))
                {
                    cliElements.Add(roleName, new Dictionary<string, Type>());
                }

                cliElements[roleName].Add(attribute.CliName, type);
            }
        }

        public Result<Type> GetTypeByCliElementName<TRole>(string name)
        {
            if (cliElements.Count == 0)
            {
                return Result.Fail<Type>("Use -p argument to allow implementation specification");
            }

            var roleName = typeof(TRole).Name;
            if (cliElements.ContainsKey(roleName))
            {
                if (cliElements[roleName].ContainsKey(name))
                {
                    return Result.Ok(cliElements[roleName][name]);
                }

                return Result.Fail<Type>($"Wrong argument value: {roleName}:{name}");
            }

            return Result.Fail<Type>($"Wrong argument name: {roleName}");
        }

        public Result<Func<string, IWordProvider>> GetWordProviderByName(string name)
        {
            if (wordProviders.ContainsKey(name))
            {
                return wordProviders[name];
            }

            return Result.Fail<Func<string, IWordProvider>>($"Couldn't find given Word Provider: {name}");
        }
    }
}