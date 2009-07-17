using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenForum.Core.Models;

namespace OpenForum.Core.DataAccess
{
    public class DefaultPostRepository : IPostRepository
    {
        private ISearcher _searcher;

        public DefaultPostRepository(ISearcher searcher)
        {
            _searcher = searcher;
        }

        public DefaultPostRepository()
            : this(new Searcher())
        {
        }

        public void RebuildSearchIndex()
        {
            Post[] posts = Find().ToArray();
            ThreadPool.QueueUserWorkItem(x => _searcher.IndexPosts(posts, true));
        }

        public IQueryable<Post> Find()
        {
            return from item in DataContextManager.Posts
                   orderby item.LastPostDate descending
                   select item;
        }

        public IQueryable<Post> Search(string query)
        {
            List<int> searchResults = new List<int>(_searcher.GetSearchResults(query));
            return Find().Where(x => searchResults.Contains(x.Id));
        }

        public Post FindById(int id)
        {
            return DataContextManager.Posts.SingleOrDefault(x => x.Id == id);
        }

        public void Submit(Post post)
        {
            if (post.Id == 0)
            {
                DataContextManager.Posts.InsertOnSubmit((Post)post);
            }

            DataContextManager.SubmitChanges();

            _searcher.IndexPosts(new Post[] { post }, false);
        }
    }
}
