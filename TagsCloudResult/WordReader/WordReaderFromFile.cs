using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagsCloudResult.Infrastructure.Common;

namespace TagsCloudResult
{
    public static class WordReaderFromFile 
    {
        public static Result<IEnumerable<string>> ReadWords(AppSettings settings)
        {
            try
            {
                using (var fileStream = File.Open(settings.Path, FileMode.Open))
                {
                    var array = new byte[fileStream.Length];
                    fileStream.Read(array, 0, array.Length);
                    fileStream.Close();
                    var words = System.Text.Encoding.Default.GetString(array).Split('\n');
                    return string.IsNullOrWhiteSpace(words[words.Length - 1]) 
                        ? Result.Ok(words.SkipLast(1)) 
                        : words;
                }
            }
            catch (FileNotFoundException e)
            {
                return Result.Fail<IEnumerable<string>>($"No such file: {settings.Path}");
            }
            catch (IOException e)
            {
                return Result.Fail<IEnumerable<string>>($"File can`t be open, try to free him: {settings.Path}, from other resources");
            }
            catch (Exception e)
            {
                return Result.Fail<IEnumerable<string>>($"Unhandled error \"{e}\" with file: {settings.Path}");
            }
            
        }
    }
}