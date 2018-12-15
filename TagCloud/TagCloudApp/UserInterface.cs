using System.Collections.Generic;
using System.IO;
using System.Linq;
using Functional;
using TagCloudCreation;

namespace TagCloudApp
{
    internal abstract class UserInterface
    {
        protected readonly TagCloudCreator Creator;
        protected readonly Dictionary<string, ITextReader> Readers;

        protected UserInterface(TagCloudCreator creator, IEnumerable<ITextReader> readers)
        {
            Creator = creator;
            Readers = readers.ToDictionary(g => g.Extension);
        }

        public abstract void Run(string[] startupArgs);

        protected Result<IEnumerable<string>> Read(string path)
        {
            return Readers.Get(Path.GetExtension(path))
                   .Then(reader => reader.ReadWords(path));

            }
    }
}
