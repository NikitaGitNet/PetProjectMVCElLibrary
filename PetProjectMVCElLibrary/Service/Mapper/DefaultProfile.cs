﻿using AutoMapper;
using BLL.Models.DTO.ApplicationUser;
using BLL.Models.DTO.Author;
using BLL.Models.DTO.Book;
using BLL.Models.DTO.Booking;
using BLL.Models.DTO.Comment;
using BLL.Models.DTO.Genre;
using BLL.Models.DTO.TextField;
using DAL.Domain.Entities;
using PetProjectMVCElLibrary.Areas.Admin.ViewModel.TextField;
using PetProjectMVCElLibrary.ViewModel.Author;
using PetProjectMVCElLibrary.ViewModel.Authorization;
using PetProjectMVCElLibrary.ViewModel.Book;
using PetProjectMVCElLibrary.ViewModel.Booking;
using PetProjectMVCElLibrary.ViewModel.Comment;
using PetProjectMVCElLibrary.ViewModel.Genre;
using PetProjectMVCElLibrary.ViewModel.User;
using System;

namespace PetProjectMVCElLibrary.Service.Mapper
{
    /// <summary>
    /// Профиль авто мапера
    /// </summary>
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

            CreateMap<ApplicationUserDTO, ApplicationUserViewModel>()
                .ForMember(x => x.Password, opt => opt.MapFrom(src => ""));
            CreateMap<ApplicationUserViewModel, ApplicationUserDTO>();

            CreateMap<ApplicationUserDTO, RegisterViewModel>()
                .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(x => x.Password, opt => opt.MapFrom(src => src.Password));
            CreateMap<RegisterViewModel, ApplicationUserDTO>()
                .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(x => x.Password, opt => opt.MapFrom(src => src.Password));

            CreateMap<BookingDTO, Booking>();
            CreateMap<Booking, BookingDTO>();

            CreateMap<BookingDTO, BookingViewModel>();
            CreateMap<BookingViewModel, BookingDTO>();

            CreateMap<GenreDTO, Genre>();
            CreateMap<Genre, GenreDTO>();

            CreateMap<GenreDTO, GenreViewModel>();
            CreateMap<GenreViewModel, GenreDTO>();

            CreateMap<AuthorDTO, Author>();
            CreateMap<Author, AuthorDTO>();

            CreateMap<AuthorDTO, AuthorViewModel>();
            CreateMap<AuthorViewModel, AuthorDTO>();

            CreateMap<TextField, TextFieldDTO>();
            CreateMap<TextFieldDTO, TextField>();

            CreateMap<TextFieldDTO, TextFieldViewModel>();
            CreateMap<TextFieldViewModel, TextFieldDTO>();
        }
    }
}
