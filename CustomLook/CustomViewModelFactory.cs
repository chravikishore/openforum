using OpenForum.Core.ViewModels;

namespace CustomLook
{
    public class CustomViewModelFactory : ViewModelFactory
    {
        public override object GetIndexViewModel(IIndexViewModel viewModel)
        {
            CustomIndexViewModel result = new CustomIndexViewModel()
            {
                Header = "Here's some header text!",
                Footer = "Here's some footer text!",
                IndexViewModel = viewModel,
            };

            return result;
        }
    }
}
