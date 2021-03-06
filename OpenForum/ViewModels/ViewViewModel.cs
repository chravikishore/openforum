﻿using OpenForum.Core.Models;
using OpenForum.Core.ViewModels.Interfaces;

namespace OpenForum.Core.ViewModels
{
	public class ViewViewModel : IViewViewModel
	{
		public string PageTitle { get; set; }
		public Post Post { get; set; }
		public User CurrentUser { get; set; }
		public bool IncludeDefaultStyles { get; set; }
		public bool IncludeValidationSummary { get; set; }
	}
}
