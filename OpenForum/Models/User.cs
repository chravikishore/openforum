using System;

namespace OpenForum.Core.Models
{
    public class User
    {
        public User(string userId, string username, string userUrl, string imageUrl)
        {
            Id = userId;
            Username = username;
            UserUrl = userUrl;
            ImageUrl = imageUrl;
        }

        public User(string userId, string username)
            : this(userId, username, null, null)
        {
        }

        public User(string userId)
            : this(userId, userId, null, null)
        {
        }

        public string Id { get; private set; }
        public string Username { get; private set; }
        public string UserUrl { get; private set;}
        public string ImageUrl { get; private set; }

        public bool HasUserUrl
        {
            get { return !string.IsNullOrEmpty(UserUrl); }
        }

        public bool HasImageUrl
        {
            get { return !string.IsNullOrEmpty(ImageUrl); }
        }
    }
}
