namespace DAL.Domain.Interfaces.Repository
{
    /// <summary>
    /// Интерфейс для классической реализации паттерна Repository
    /// Базовая логика взаимодействия, актуальная для всех энтити
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Получение всех entityes из БД
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllEntityesAsync();
        /// <summary>
        /// Получение entity из БД по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T?> GetEntityByIdAsync(Guid id);
        /// <summary>
        /// Сохранение entity в БД
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool SaveEntity(T entity);
        /// <summary>
        /// Обновление ентити в БД
        /// </summary>
        /// <param name="entity"></param>
        bool UpdateEntity(T entity);
        /// <summary>
        /// Удаление entity из БД на основании Id
        /// </summary>
        /// <param name="id"></param>
        void DeleteEntity(Guid id);
        /// <summary>
        /// Удаление entityes из БД на основании коллекции entityes
        /// </summary>
        /// <param name="entityes"></param>
        void DeleteRangeEntityes(IEnumerable<T> entityes);
    }
}
