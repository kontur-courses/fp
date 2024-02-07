using WeCantSpell.Hunspell;

namespace TagCloudGenerator.TextProcessors
{
    public class BoringWordsTextProcessor : ITextProcessor
    {
        private readonly string toDicPath;
        private readonly string toAffPath;

        public BoringWordsTextProcessor(string toDicPath, string toAffPath)
        {
            this.toDicPath = toDicPath;
            this.toAffPath = toAffPath;
        }

        public Result<IEnumerable<string>> ProcessText(IEnumerable<string> text)
        {
            WordList wordList;
            try
            {
                wordList = WordList.CreateFromFiles(toDicPath, toAffPath);
            }
            catch
            {
                return Result<IEnumerable<string>>.Failure("The path to the dictionary is incorrect or the dictionary is unsuitable");
            }

            var words = new List<string>();
            foreach (var word in text)
            {
                var details = wordList.CheckDetails(word);
                var wordEntryDetails = wordList[string.IsNullOrEmpty(details.Root) ? word : details.Root];

                if (wordEntryDetails.Length != 0 && wordEntryDetails[0].Morphs.Count != 0)
                {
                    var po = wordEntryDetails[0].Morphs[0];

                    if (po == "po:pronoun" || po == "po:preposition"
                        || po == "po:determiner" || po == "po:conjunction")
                        continue;
                }

                words.Add(word);
            }

            return Result<IEnumerable<string>>.Success(words);
        }
    }
}