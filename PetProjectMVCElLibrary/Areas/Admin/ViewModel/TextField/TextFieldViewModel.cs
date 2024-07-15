using PetProjectMVCElLibrary.Areas.Admin.Interfaces.ViewModel;

namespace PetProjectMVCElLibrary.Areas.Admin.ViewModel.TextField
{
    public class TextFieldViewModel : ITextFieldViewModel
    {
        public Guid Id { get; set; }
        public string? CodeWord { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
    }
}
