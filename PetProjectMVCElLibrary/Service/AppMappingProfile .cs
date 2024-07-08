using AutoMapper;
using BLL.Models.DTO.Book;
using DAL.Domain.Entities;
using System;

namespace PetProjectMVCElLibrary.Service
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<BookDTO, Book>().ReverseMap();
        }
    }
}
