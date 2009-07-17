using System.Data.Linq;
using OpenForum.Core.Models;

namespace OpenForum.Core.DataAccess
{
    internal class OpenForumDataContext : DataContext
    {
        public OpenForumDataContext(string connection)
            : base(connection)
        {
            DataLoadOptions options = new DataLoadOptions();
            options.LoadWith<Post>(x => x.Replies);
            LoadOptions = options;
        }

        public Table<Post> Posts
        {
            get { return GetTable<Post>(); }
        }

        public Table<Reply> Replies
        {
            get { return GetTable<Reply>(); }
        }
    }
}
