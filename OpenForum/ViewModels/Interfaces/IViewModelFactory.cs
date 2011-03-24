
namespace OpenForum.Core.ViewModels.Interfaces
{
	public interface IViewModelFactory
	{
		object GetIndexViewModel(IIndexViewModel viewModel);
		object GetPostViewModel(IPostViewModel viewModel);
		object GetReplyViewModel(IReplyViewModel viewModel);
		object GetViewViewModel(IViewViewModel viewModel);
	}
}
