using System;
using System.Linq;
using System.Web;
using OpenForum.Core.Models;

namespace OpenForum.Core.DataAccess
{
	internal static class DataContextManager
	{
		private const string WEB_CONTEXT_KEY = "OPEN_FORUM_WEB_CONTEXT_KEY";

		internal static IQueryable<Post> Posts
		{
			get { return Current.Posts; }
		}

		internal static void InsertOnSubmit(Post post)
		{
			Current.Posts.Add(post);
		}

		internal static void SubmitChanges()
		{
			Current.SaveChanges();
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

		private static OpenForumDataContext Current
		{
			get
			{
				OpenForumDataContext result;
				bool isInWebContext = HttpContext.Current != null;

				if (isInWebContext)
				{
					result = WebContext;
				}
				else
				{
					result = ThreadContext;
				}

				if (result == null)
				{
					result = new OpenForumDataContext();

					if (isInWebContext)
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
