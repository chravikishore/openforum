using System.Collections.Generic;
using OpenForum.Core.Models;

namespace OpenForum.Core.ViewModels.Interfaces
{
	public interface IIndexViewModel : ITitledViewModel
	{
		string SearchQuery { get; set; }
		string Message { get; set; }
		int CurrentPage { get; set; }
		IEnumerable<Post> Posts { get; set; }
		int TotalPages { get; set; }
		bool IncludeDefaultStyles { get; set; }
		bool IncludeValidationSummary { get; set; }
	}
}
