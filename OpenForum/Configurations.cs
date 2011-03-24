using OpenForum.Core.DataAccess;
using OpenForum.Core.DataAccess.Interfaces;
using OpenForum.Core.ViewModels;
using OpenForum.Core.ViewModels.Interfaces;

namespace OpenForum.Core
{
	public class Configurations
	{
		internal static Configurations Current { get; set; }

		static Configurations()
		{
			Current = new Configurations();
		}

		public Configurations()
		{
			IncludeDefaultStyles = true;
			IncludeValidationSummary = true;
			IncludeWysiwygEditor = true;
			UserRepository = new UserRepository();
			PostRepository = new PostRepository();
			ViewModelFactory = new ViewModelFactory();
			MasterPageLocation = "~/Views/Shared/Site.Master";
			MasterPageContentPlaceHolderId = "MainContent";
			MasterPageTitlePlaceHolderId = "TitleContent";
		}

		public bool IncludeDefaultStyles { get; set; }
		public bool IncludeValidationSummary { get; set; }
		public bool IncludeWysiwygEditor { get; set; }
		public IUserRepository UserRepository { get; set; }
		public IPostRepository PostRepository { get; set; }
		public IViewModelFactory ViewModelFactory { get; set; }
		public string MasterPageLocation { get; set; }
		public string MasterPageContentPlaceHolderId { get; set; }
		public string MasterPageTitlePlaceHolderId { get; set; }
	}
}
