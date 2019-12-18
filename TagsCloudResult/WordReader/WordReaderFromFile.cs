using System;
using System.Collections.Generic;
using System.IO;

namespace TagsCloudResult
{
    public static class WordReaderFromFile 
    {
        public static Result<IEnumerable<string>> ReadWords(string path)
        {
            try
            {
                using (var fileStream = File.Open(path, FileMode.Open))
                {
                    var array = new byte[fileStream.Length];
                    fileStream.Read(array, 0, array.Length);
                    fileStream.Close();
                    var words = System.Text.Encoding.Default.GetString(array).Split('\n');
                    return words;
                }
            }
            catch (FileNotFoundException e)
            {
                return Result.Fail<IEnumerable<string>>($"No such file in directory: {path}");
            }
            catch (FileLoadException e)
            {
                return Result.Fail<IEnumerable<string>>($"File can`t be open, try to free him: {path}, from other resources");
            }
            catch (Exception e)
            {
                return Result.Fail<IEnumerable<string>>($"Unhandled error with file: {path}");
            }
            
        }
    }
}