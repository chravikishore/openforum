using OpenForum.Core.Models;
using OpenForum.Core.ViewModels.Interfaces;

namespace OpenForum.Core.ViewModels
{
	public class ReplyViewModel : IReplyViewModel
	{
		public string PageTitle { get; set; }
		public Post Post { get; set; }
		public Reply Reply { get; set; }
		public bool IncludeDefaultStyles { get; set; }
		public bool IncludeValidationSummary { get; set; }
		public bool IncludeWysiwygEditor { get; set; }
	}
}
