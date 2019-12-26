using System;
using System.IO;

namespace TagsCloudForm.Common
{
    public class FileBlobStorage : IBlobStorage
    {
        public Result<byte[]> Get(string name)
        {
            try
            {
                var output = File.ReadAllBytes(name);
                return Result.Ok(output);
            }
            catch (Exception e)
            {
                return Result.Fail<byte[]>(e.Message);
            }
        }


        public void Set(string name, byte[] content)
        {
            File.WriteAllBytes(name, content);
        }
    }
}