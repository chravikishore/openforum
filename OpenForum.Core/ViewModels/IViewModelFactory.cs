using System.Collections.Generic;
using OpenForum.Core.Models;

namespace OpenForum.Core.ViewModels
{
    public interface IViewModelFactory
    {
        object GetIndexViewModel(IIndexViewModel viewModel);
        object GetPostViewModel(IPostViewModel viewModel);
        object GetReplyViewModel(IReplyViewModel viewModel);
        object GetViewViewModel(IViewViewModel viewModel);
    }
}
