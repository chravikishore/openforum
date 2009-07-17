using OpenForum.Core.Models;

namespace OpenForum.Core.ViewModels
{
    public interface IReplyViewModel : ITitledViewModel
    {
        Post Post { get; set; }
        Reply Reply { get; set; }
        bool IncludeDefaultStyles { get; set; }
        bool IncludeValidationSummary { get; set; }
        bool IncludeWysiwygEditor { get; set; }
    }
}
