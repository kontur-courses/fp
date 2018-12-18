using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TagCloud.Core.Util;

namespace TagCloud.Core.WordsParsing.WordsReading
{
    public interface IWordsReader
    {
        /// <summary>
        ///     <returns>
        ///         Regular expression represents extension of files allowed by this reader (including dot)
        ///     </returns>
        /// </summary>
        /// <remarks>
        ///     Dot is needed here to differ ".abc_x" and ".x"
        /// </remarks>
        Regex AllowedFileExtension { get; }

        Result<IEnumerable<string>> ReadFrom(Stream stream);
    }
}