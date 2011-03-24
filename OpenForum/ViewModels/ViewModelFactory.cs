using OpenForum.Core.ViewModels.Interfaces;

namespace OpenForum.Core.ViewModels
{
	public class ViewModelFactory : IViewModelFactory
	{
		public virtual object GetIndexViewModel(IIndexViewModel viewModel)
		{
			return viewModel;
		}

		public virtual object GetViewViewModel(IViewViewModel viewModel)
		{
			return viewModel;
		}

		public virtual object GetPostViewModel(IPostViewModel viewModel)
		{
			return viewModel;
		}

		public virtual object GetReplyViewModel(IReplyViewModel viewModel)
		{
			return viewModel;
		}
	}
}
