using BLL.Interfaces;
using BLL.Models.DTO.Book;
using BLL.Models.DTO.Booking;

namespace PetProjectMVCElLibrary.Service
{
	public static class CheckBookingEnd
	{
		public static async Task<IEnumerable<BookingDTO>> DeletingExpired(IEnumerable<BookingDTO> bookingDTOs, IBookingService bookingService, IBookService bookService)
		{
			IEnumerable<BookingDTO> bookingsExpired = bookingDTOs.Where(x => x.FinishedOn < DateTime.Now);
			if (bookingsExpired.Any())
			{
				IEnumerable<BookDTO> bookDTOs = await bookService.GetAllBooks();
				bookDTOs = bookDTOs.Where(x => bookingDTOs.Select(z => z.BookId).Contains(x.Id));
				if (bookDTOs.Any())
				{ 
					foreach (BookDTO bookDTO in bookDTOs)
					{
						bookDTO.IsBooking = false;
					}
				}
				bookService.UpdateBooksRange(bookDTOs);
				bookingService.DeleteRangeBookings(bookingsExpired);
			}
			IEnumerable<BookingDTO> bookingsNonExpired = bookingDTOs.Where(x => x.FinishedOn > DateTime.Now);
			return bookingsNonExpired;

		}
	}
}
