using System.Collections.Generic;
using System.Threading;
using OpenForum.Core.DataAccess;
using OpenForum.Core.Models;

namespace CustomUsers
{
    public class CustomUserRepository : IUserRepository
    {
        private Dictionary<string, User> _users = new Dictionary<string, User>()
        {
            { "Mr_Question", new User("Mr_Question", "MR QUESTION", "http://google.com", "../../../Content/mr_question.jpg")},
            { "Mr_Answer", new User("Mr_Answer", "MR ANSWER", "http://bing.com", "../../../Content/mr_answer.jpg")},
        };

        public User FindById(string userId)
        {
            User user;
            
            if (!_users.TryGetValue(userId, out user))
            {
                user = new User(userId, userId, "http://software.herbrandson.com", "../../../Content/none.gif");
            }

            return user;
        }

        public User FindCurrentUser()
        {
            string userId = Thread.CurrentPrincipal.Identity.Name;
            
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            return FindById(userId);;
        }
    }
}
