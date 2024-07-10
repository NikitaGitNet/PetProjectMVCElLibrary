using AutoMapper;
using BLL.Models.DTO.ApplicationUser;
using BLL.Models.DTO.Book;
using BLL.Models.DTO.Booking;
using BLL.Models.DTO.Comment;
using DAL.Domain.Entities;
using PetProjectMVCElLibrary.ViewModel.Book;
using PetProjectMVCElLibrary.ViewModel.Booking;
using PetProjectMVCElLibrary.ViewModel.Comment;
using System;

namespace PetProjectMVCElLibrary.Service.Mapper
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile() 
        {
            CreateMap<BookDTO, Book>();
            CreateMap<Book, BookDTO>();

            CreateMap<BookViewModel, BookDTO>();
            CreateMap<BookDTO, BookViewModel>();

            CreateMap<Comment, CommentDTO>()
                .ForMember(x => x.CommentText, opt => opt.MapFrom(src => src.Text));
            CreateMap<CommentDTO, Comment>()
                .ForMember(x => x.Text, opt => opt.MapFrom(src => src.CommentText));

            CreateMap<CommentDTO, CommentViewModel>();
            CreateMap<CommentViewModel, CommentDTO>();

            CreateMap<ApplicationUserDTO, ApplicationUser>();
            CreateMap<ApplicationUser, ApplicationUserDTO>();

            CreateMap<BookingDTO, Booking>();
            CreateMap<Booking, BookingDTO>();

            CreateMap<BookingDTO, BookingViewModel>();
            CreateMap<BookingViewModel, BookingDTO>();
        }
    }
}
