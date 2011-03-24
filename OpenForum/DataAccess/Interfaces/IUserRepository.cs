using OpenForum.Core.Models;

namespace OpenForum.Core.DataAccess.Interfaces
{
	public interface IUserRepository
	{
		User FindCurrentUser();
		User FindById(string userId);
	}
}
