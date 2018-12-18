using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ResultOfTask;

namespace TagsCloudPreprocessor
{
    public class XmlWordExcluder : IWordExcluder
    {
        private readonly string filename;

        private readonly XmlSerializer hashSetSerializer = new XmlSerializer(typeof(HashSet<string>));

        public XmlWordExcluder()
        {
            filename = "russianWords.xml";
        }

        public Result<HashSet<string>> GetExcludedWords()
        {
            return ReadForbiddenWords();
        }

        public Result<None> SetExcludedWord(string word)
        {
            var forbiddenWords = ReadForbiddenWords();
            forbiddenWords
                .Then(x => x.Add(word));
            return WriteForbiddenWords(forbiddenWords);
        }

        private Result<HashSet<string>> ReadForbiddenWords()
        {
            using (var file = new FileStream(filename, FileMode.OpenOrCreate))
            {
                return Result.Of(() =>
                        (HashSet<string>) hashSetSerializer.Deserialize(file),
                    "Can not read dictionary with forbidden words");
            }
        }

        private Result<None> WriteForbiddenWords(Result<HashSet<string>> forbiddenWords)
        {
            if (!forbiddenWords.IsSuccess) return Result.Fail(forbiddenWords.Error);
            using (var file = new FileStream(filename, FileMode.Create))
            {
                return Result.Of(() =>
                        hashSetSerializer.Serialize(file, forbiddenWords),
                    "Can not write dictionary with forbidden words");
            }
        }
    }
}