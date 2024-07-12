using BLL.Models.DTO.Genre;

namespace BLL.Interfaces
{
    public interface IGenreService
    {
        void CreateGenre(GenreDTO book);
        Task<GenreDTO?> GetGenre(Guid id);
        Task<GenreDTO?> GetGenreByName(string name);
        Task<IEnumerable<GenreDTO>> GetAllGenres();
        void DeleteGenre(Guid bookId);
    }
}
