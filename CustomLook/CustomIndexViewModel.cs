using OpenForum.Core.ViewModels;

namespace CustomLook
{
    public class CustomIndexViewModel
    {
        public string Header { get; set; }
        public string Footer { get; set; }
        public IIndexViewModel IndexViewModel { get; set; }
    }
}
