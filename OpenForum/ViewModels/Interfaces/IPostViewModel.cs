using OpenForum.Core.Models;

namespace OpenForum.Core.ViewModels.Interfaces
{
	public interface IPostViewModel : ITitledViewModel
	{
		Post Post { get; set; }
		bool IncludeDefaultStyles { get; set; }
		bool IncludeValidationSummary { get; set; }
		bool IncludeWysiwygEditor { get; set; }
	}
}
