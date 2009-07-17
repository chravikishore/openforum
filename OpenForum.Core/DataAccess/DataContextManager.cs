using System;
using System.Data.Linq;
using System.IO;
using System.Web;
using OpenForum.Core.Models;
using OpenForum.Core.Properties;

namespace OpenForum.Core.DataAccess
{
    internal static class DataContextManager
    {
        private const string DEFAULT_CONNECTION_STRING = @"Data Source=.\SQLEXPRESS;AttachDBFilename=|DataDirectory|openforum.mdf;Integrated Security=True;User Instance=True";
        private const string WEB_CONTEXT_KEY = "OPEN_FORUM_WEB_CONTEXT_KEY";

        private static StringWriter _log = new StringWriter();

        static DataContextManager()
        {
            if (string.IsNullOrEmpty(Settings.Default.ConnectionString))
            {
                DatabaseCreationHelper.EnsureDatabaseExists(DEFAULT_CONNECTION_STRING);
            }
        }

        internal static Table<Post> Posts
        {
            get { return Current.Posts; }
        }

        internal static void SubmitChanges()
        {
            Current.SubmitChanges();
        }

        private static OpenForumDataContext WebContext
        {
            get { return (OpenForumDataContext)HttpContext.Current.Items[WEB_CONTEXT_KEY]; }
            set { HttpContext.Current.Items[WEB_CONTEXT_KEY] = value; }
        }

        [ThreadStatic]
        private static OpenForumDataContext _threadContext;
        private static OpenForumDataContext ThreadContext
        {
            get { return _threadContext; }
            set { _threadContext = value; }
        }

        private static bool IsInWebContext
        {
            get { return HttpContext.Current != null; }
        }

        private static string ConnectionString
        {
            get
            {
                string connectionString = Settings.Default.ConnectionString;

                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = DEFAULT_CONNECTION_STRING;
                }

                return connectionString;
            }
        }

        private static OpenForumDataContext Current
        {
            get
            {
                OpenForumDataContext result;

                if (IsInWebContext)
                {
                    result = WebContext;
                }
                else
                {
                    result = ThreadContext;
                }

                if (result == null)
                {
                    result = new OpenForumDataContext(ConnectionString);
                    //result.Log = _log; // NOTE: only uncomment this line for testing

                    if (IsInWebContext)
                    {
                        WebContext = result;
                    }
                    else
                    {
                        ThreadContext = result;
                    }
                }

                return result;
            }
        }
    }
}
