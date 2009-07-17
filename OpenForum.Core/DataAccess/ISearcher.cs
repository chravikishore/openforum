using System.Collections.Generic;
using OpenForum.Core.Models;

namespace OpenForum.Core.DataAccess
{
    public interface ISearcher
    {
        int[] GetSearchResults(string queryText);
        void IndexPosts(IEnumerable<Post> posts, bool recreateIndex);
    }
}
