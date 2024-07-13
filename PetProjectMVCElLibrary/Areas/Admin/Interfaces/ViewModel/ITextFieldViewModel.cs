﻿namespace PetProjectMVCElLibrary.Areas.Admin.Interfaces.ViewModel
{
    public interface ITextFieldViewModel
    {
        public Guid Id { get; set; }
        public string? CodeWord { get; set; }
        public string? SubTitle { get; set; }
        public string? TitleImagePath { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
    }
}
