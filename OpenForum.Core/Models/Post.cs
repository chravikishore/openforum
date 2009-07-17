using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using OpenForum.Core.DataAccess;

namespace OpenForum.Core.Models
{
    [Table(Name = "OpenForum_Post")]
    public class Post
    {
        private IUserRepository _userRepository;

        public Post(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            DateTime now = DateTime.Now;
            CreatedDate = now;
            LastPostDate = now;

            if (_userRepository.FindCurrentUser() != null)
            {
                string currentUserId = _userRepository.FindCurrentUser().Id;
                CreatedById = currentUserId;
                LastPostById = currentUserId;
            }
        }

        public Post()
            : this(Configurations.Current.UserRepository)
        {
        }

        public Post(string title, string body)
            : this()
        {
            Title = title;
            Body = body;
        }

        [Column(DbType = "int IDENTITY(1,1) NOT NULL", CanBeNull = false, IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int Id { get; private set; }

        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        private string CreatedById { get; set; }

        [Column(DbType = "datetime NOT NULL", CanBeNull = false)]
        public DateTime CreatedDate { get; private set; }

        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        private string LastPostById { get; set; }

        [Column(DbType = "datetime NOT NULL", CanBeNull = false)]
        public DateTime LastPostDate { get; private set; }

        [Column(DbType = "nvarchar(250) NOT NULL", CanBeNull = false)]
        public string Title { get; set; }

        private string _body;
        [Column(DbType = "nvarchar(MAX) NOT NULL", Storage = "_body", CanBeNull = false)]
        public string Body
        {
            get { return _body; }
            set { _body = HtmlHelper.FixUp(value); }
        }

        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int ViewCount { get; private set; }

        [Association(ThisKey = "Id", OtherKey = "PostId")]
        private EntitySet<Reply> _replies = new EntitySet<Reply>();
        public Reply[] Replies
        {
            get { return _replies.ToArray(); }
        }

        private User _createdBy;
        public User CreatedBy
        {
            get
            {
                if (_createdBy == null)
                {
                    _createdBy = _userRepository.FindById(CreatedById);
                }

                return _createdBy;
            }
        }

        private User _lastPostBy;
        public User LastPostBy
        {
            get
            {
                if (_lastPostBy == null)
                {
                    _lastPostBy = _userRepository.FindById(LastPostById);
                }

                return _lastPostBy;
            }
        }

        public Reply AddReply()
        {
            Reply reply = new Reply(this);

            _replies.Add(reply);

            LastPostDate = DateTime.Now;
            LastPostById = _userRepository.FindCurrentUser().Id;
            _lastPostBy = null; // reset the lazy load

            return reply;
        }

        public void IncrementViewCount()
        {
            ViewCount++;
        }

        public Reply FindReplyById(int id)
        {
            return Replies.Single(x => x.Id == id);
        }

        protected void OnValidate(ChangeAction action)
        {
            Validate();
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Title))
            {
                throw new OpenForumException("Please provide a title for your post.");
            }

            if (string.IsNullOrEmpty(Body))
            {
                throw new OpenForumException("Please provide text for your post.");
            }
            else
            {
                HtmlHelper.Validate(Body);
            }
        }

        public string GetFullText()
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine(Title);
            result.AppendLine(Body);

            foreach (var item in Replies)
            {
                result.AppendLine(item.Body);
            }

            return result.ToString();
        }
    }
}
