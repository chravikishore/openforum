using System;

namespace OpenForum.Core
{
    public class OpenForumException : Exception
    {
        public OpenForumException()
            : base()
        {
        }

        public OpenForumException(string message)
            : base(message)
        {
        }

        public OpenForumException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }
    }
}
