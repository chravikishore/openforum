using OpenForum.Core.Models;

namespace OpenForum.Core.DataAccess
{
    public interface IUserRepository
    {
        User FindCurrentUser();
        User FindById(string userId);
    }
}
