using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenForum.Core.DataAccess.Interfaces;
using OpenForum.Core.Models;

namespace OpenForum.Core.DataAccess
{
	public class PostRepository : IPostRepository
	{
		private ISearcher _searcher;

		public PostRepository(ISearcher searcher)
		{
			_searcher = searcher;
		}

		public PostRepository()
			: this(new Searcher())
		{
		}

		public void RebuildSearchIndex()
		{
			Post[] posts = Find().ToArray();
			ThreadPool.QueueUserWorkItem(x => _searcher.IndexPosts(posts, true));
		}

		public Post FindById(int id)
		{
			return DataContextManager.Posts.SingleOrDefault(x => x.Id == id);
		}

		public IEnumerable<Post> Find()
		{
			return DataContextManager.Posts.OrderByDescending(x => x.LastPostDate);
		}

		public IEnumerable<Post> Search(string query)
		{
			List<int> searchResults = new List<int>(_searcher.GetSearchResults(query));
			return Find().Where(x => searchResults.Contains(x.Id));
		}

		public void Submit(Post post)
		{
			if (post.Id == 0)
			{
				DataContextManager.InsertOnSubmit(post);
			}

			DataContextManager.SubmitChanges();

			_searcher.IndexPosts(new Post[] { post }, false);
		}
	}
}
