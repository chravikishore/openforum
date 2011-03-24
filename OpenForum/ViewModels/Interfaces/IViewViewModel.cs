using OpenForum.Core.Models;

namespace OpenForum.Core.ViewModels.Interfaces
{
	public interface IViewViewModel : ITitledViewModel
	{
		Post Post { get; set; }
		User CurrentUser { get; set; }
		bool IncludeDefaultStyles { get; set; }
		bool IncludeValidationSummary { get; set; }
	}
}
