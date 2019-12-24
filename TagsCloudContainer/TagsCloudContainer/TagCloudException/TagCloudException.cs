using System;
using System.Runtime.Serialization;

namespace TagsCloudContainer
{
    public class TagCloudException : Exception
    {
        public TagCloudException()
            : base()
        {
        }

        public TagCloudException(String message)
            : base(message)
        {
        }

        public TagCloudException(String message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected TagCloudException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}