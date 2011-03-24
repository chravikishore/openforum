using System.Data.Entity;
using OpenForum.Core.Models;

namespace OpenForum.Core.DataAccess
{
	internal class OpenForumDataContext : DbContext
	{
		public DbSet<Post> Posts { get; set; }
	}
}
