namespace PetProjectMVCElLibrary.Service.Logger
{
    /// <summary>
    /// Провайдер логгирования
    /// </summary>
    public class FileLoggerProvider : ILoggerProvider
    {
        string path;
        public FileLoggerProvider(string path)
        {
            this.path = path;
        }
        /// <summary>
        /// Создает и возвращает объект логгера. Для создания логгера используется путь к файлу, который передается через конструктор
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(path);
        }
        /// <summary>
        /// Управляет освобождение ресурсов. В данном случае пустая реализация
        /// </summary>
        public void Dispose() { }
    }
}
