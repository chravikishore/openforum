using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using OpenForum.Core.DataAccess;

namespace OpenForum.Core.Models
{
    [Table(Name = "OpenForum_Reply")]
    public class Reply
    {
        private IUserRepository _userRepository;

        public Reply(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            CreatedDate = DateTime.Now;

            if (_userRepository.FindCurrentUser() != null)
            {
                CreatedById = _userRepository.FindCurrentUser().Id;
            }
        }

        public Reply()
            : this(Configurations.Current.UserRepository)
        {
        }

        internal Reply(Post post)
            : this()
        {
            PostId = post.Id;
        }

        [Column(DbType = "int IDENTITY(1,1) NOT NULL", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert, CanBeNull = false)]
        public int Id { get; private set; }

        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int PostId { get; private set; }

        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        private string CreatedById { get; set; }

        [Column(DbType = "datetime NOT NULL", CanBeNull = false)]
        public DateTime CreatedDate { get; private set; }

        private string _body;
        [Column(DbType = "nvarchar(MAX) NOT NULL", Storage = "_body", CanBeNull = false)]
        public string Body
        {
            get { return _body; }
            set { _body = HtmlHelper.FixUp(value); }
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

        protected void OnValidate(ChangeAction action)
        {
            Validate();
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Body))
            {
                throw new OpenForumException("Please provide text for your reply.");
            }
            else
            {
                HtmlHelper.Validate(Body);
            }
        }
    }
}
