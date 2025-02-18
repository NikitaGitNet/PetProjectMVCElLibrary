﻿namespace PetProjectMVCElLibrary.Interfaces.Booking
{
    public interface IBookingViewModel
    {
        Guid Id { get; set; }
        DateTime CreateOn { get; set; }
        DateTime FinishedOn { get; set; }
        bool IssueBooking { get; set; }
        string? Email { get; set; }
        Guid BookId { get; set; }
        string? BooksTitle { get; set; }
        string? ReceiptCode { get; set; }
    }
}
