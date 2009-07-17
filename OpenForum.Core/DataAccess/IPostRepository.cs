using System.Linq;
using OpenForum.Core.Models;

namespace OpenForum.Core.DataAccess
{
    public interface IPostRepository
    {
        void RebuildSearchIndex();
        IQueryable<Post> Find();
        IQueryable<Post> Search(string query);
        Post FindById(int id);
        void Submit(Post post);
    }
}
