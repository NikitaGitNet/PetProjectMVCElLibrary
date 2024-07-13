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

namespace BLL.Services.Comment
{
    public class CommentService : ICommentService
    {
        private readonly UnitOfWorkRepository Database;
        private readonly IMapper _mapper;
        AppDbContext dbContext;
        public CommentService(AppDbContext context, IMapper mapper)
        {
            dbContext = context;
            Database = new UnitOfWorkRepository(context);
            _mapper = mapper;
        }
        /// <summary>
        /// Получение коммента из БД, маппинг в ДТО
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CommentDTO> GetComment(Guid id)
        {
            DAL.Domain.Entities.Comment? comment = await Database.CommentRepository.GetEntityByIdAsync(id);
            if (comment != null)
            {
                return _mapper.Map<CommentDTO>(comment);
            }
            throw new ValidationException("Книга не найдена", "");
        }
        /// <summary>
        /// Получение комментов для конкретной книги и маппинг их в ДТО
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CommentDTO>> GetCommentsByBookId(Guid id)
        {
            IEnumerable<DAL.Domain.Entities.Comment> comments = await Database.CommentRepository.GetEntityesByBookIdAsync(id);
            return _mapper.Map<IEnumerable<CommentDTO>>(comments);
        }
        /// <summary>
        /// Получение всех комментов из бд и маппинг их в ДТО
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CommentDTO>> GetAllComments()
        {
            IEnumerable<DAL.Domain.Entities.Comment> comments = await Database.CommentRepository.GetAllEntityesAsync();
            return _mapper.Map<IEnumerable<CommentDTO>>(comments);
        }
        /// <summary>
        /// Маппинг дто в энтити и с сохранение в БД
        /// </summary>
        /// <param name="commentDTO"></param>
        /// <returns></returns>
        public void CreateComment(CommentDTO commentDTO)
        {
            DAL.Domain.Entities.Comment comment = _mapper.Map<DAL.Domain.Entities.Comment>(commentDTO);
            Database.CommentRepository.SaveEntity(comment);
        }
        /// <summary>
        /// Удаление коммента из БД
        /// </summary>
        /// <param name="bookId"></param>
        public void DeleteComment(Guid bookId)
        {
            Database.CommentRepository.DeleteEntity(bookId);
        }
        public void DeleteRangeComments(IEnumerable<CommentDTO> commentDTOs)
        {
            IEnumerable<DAL.Domain.Entities.Comment> comments = _mapper.Map<IEnumerable<DAL.Domain.Entities.Comment>>(commentDTOs);
            Database.CommentRepository.DeleteRangeEntityes(comments);
        }
    }
}
