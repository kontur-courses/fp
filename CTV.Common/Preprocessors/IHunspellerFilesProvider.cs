namespace CTV.Common.Preprocessors
{
    public interface IHunspellerFilesProvider
    {
        public string GetDicFile();
        public string GetAffFile();
    }
}