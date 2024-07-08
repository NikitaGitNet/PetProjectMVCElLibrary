using AutoMapper;
using BLL.Infrastructure;
using BLL.Interfaces;
using BLL.Models.DTO.Book;
using BLL.Models.DTO.Comment;
using DAL.Domain;
using DAL.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly UnitOfWorkRepository Database;
        private readonly IMapper mapper;
        AppDbContext dbContext;
        public CommentService(AppDbContext context)
        {
            dbContext = context;
            Database = new UnitOfWorkRepository(context);
            mapper = new MapperConfiguration(cfg => cfg.CreateMap<CommentDTO, Comment>()).CreateMapper();
        }
        /// <summary>
        /// Получение коммента из БД, конверция в ДТО
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CommentDTO> GetComment(Guid id)
        {
            Comment? comment = await Database.CommentRepository.GetEntityByIdAsync(id);
            if (comment != null) 
            {
                return mapper.Map<Comment, CommentDTO>(comment);
            }
            throw new ValidationException("Книга не найдена", "");
        }
        public async Task<IEnumerable<CommentDTO>> GetAllComments()
        {
            IEnumerable<Comment> comments = await Database.CommentRepository.GetAllEntityesAsync();
            return mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(comments);
        }
        public async Task CreateComment(CommentDTO commentDTO)
        {
            Comment comment = mapper.Map<CommentDTO, Comment>(commentDTO);
            await Database.CommentRepository.SaveEntityAsync(comment);
        }
        public void DeleteComment(Guid bookId)
        {
            Database.CommentRepository.DeleteEntity(bookId);
        }
        public void DeleteRangeComments(IEnumerable<CommentDTO> commentDTOs)
        {
            IEnumerable<Comment> comments = mapper.Map<IEnumerable<CommentDTO>, IEnumerable<Comment>>(commentDTOs);
            Database.CommentRepository.DeleteRangeEntityes(comments);
        }
        public async Task SaveCommentAsync(CommentDTO commentDTO)
        {
            Comment comment = mapper.Map<CommentDTO, Comment>(commentDTO);
            Book? book = await Database.BookRepository.GetEntityByIdAsync(commentDTO.BookId);
            if (book != null)
            {
                comment.Book = book;
            }
            var a = dbContext.ApplicationUsers.FirstOrDefault(x => x.Id == commentDTO.UserId);
            if (a != null) 
            {
                comment.User = a;
            }
            await Database.CommentRepository.SaveEntityAsync(comment);
        }
    }
}
