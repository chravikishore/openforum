using System.Collections.Generic;
using OpenForum.Core.Models;

namespace OpenForum.Core.DataAccess.Interfaces
{
	public interface IPostRepository
	{
		void RebuildSearchIndex();
		IEnumerable<Post> Find();
		IEnumerable<Post> Search(string query);
		Post FindById(int id);
		void Submit(Post post);
	}
}
