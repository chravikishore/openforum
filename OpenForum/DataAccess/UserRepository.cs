using System.Threading;
using OpenForum.Core.DataAccess.Interfaces;
using OpenForum.Core.Models;

namespace OpenForum.Core.DataAccess
{
	public class UserRepository : IUserRepository
	{
		public User FindCurrentUser()
		{
			return new User(Thread.CurrentPrincipal.Identity.Name);
		}

		public User FindById(string userId)
		{
			return new User(userId);
		}
	}
}
