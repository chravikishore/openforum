using System.Collections.Generic;
using OpenForum.Core.Models;
using OpenForum.Core.ViewModels.Interfaces;

namespace OpenForum.Core.ViewModels
{
	public class IndexViewModel : IIndexViewModel
	{
		public string PageTitle { get; set; }
		public string SearchQuery { get; set; }
		public string Message { get; set; }
		public IEnumerable<Post> Posts { get; set; }
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public bool IncludeDefaultStyles { get; set; }
		public bool IncludeValidationSummary { get; set; }
	}
}
