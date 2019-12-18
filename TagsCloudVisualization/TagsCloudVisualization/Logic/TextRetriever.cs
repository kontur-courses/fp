using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using ErrorHandler;

namespace TagsCloudVisualization.Logic
{
    public static class TextRetriever
    {
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;

        public static Result<string> RetrieveTextFromFile(string path, Encoding encoding)
        {
            if (path == null)
                return Result.Fail<string>("Path is null");
            if (!File.Exists(path))
                return Result.Fail<string>($"Path {path} doesn't exist");
            if (!IsFileAccessible(path))
                return Result.Fail<string>($"Couldn't get acces to file {path}'");
            var text = File.ReadAllText(path, encoding);
            return text;
        }
        
        public static Result<string> RetrieveTextFromFile(string path)
        {
            return RetrieveTextFromFile(path, DefaultEncoding);
        }

        private static bool IsFileAccessible(string path)
        {
            var fileSecurity = File.GetAccessControl(path);
            var accessRules = fileSecurity.GetAccessRules(true, true, typeof(NTAccount));
            foreach (var accessRule in accessRules)
            {
                var fileSystemRule = (FileSystemAccessRule) accessRule;
                if (fileSystemRule.AccessControlType == AccessControlType.Deny &&
                    (fileSystemRule.FileSystemRights == FileSystemRights.FullControl
                     || fileSystemRule.FileSystemRights == FileSystemRights.Read
                     || fileSystemRule.FileSystemRights == FileSystemRights.ReadAndExecute
                     || fileSystemRule.FileSystemRights == FileSystemRights.Modify))
                    return false;
            }
            return true;
        }
    }
}