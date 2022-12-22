using System.Text;

namespace TagsCloudContainer.TextReaders
{
    public class TxtReader : ITextReader
    {
        public Result<string> GetTextFromFile(string path)
        {
            var builder = new StringBuilder();
            using (var sr = new StreamReader(path))
            {
                var line = sr.ReadLine();
                while (line != null)
                {
                    builder.Append(line + Environment.NewLine);
                    line = sr.ReadLine();
                }
            }

            return builder.ToString().Ok();
        }
    }
}