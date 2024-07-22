namespace PetProjectMVCElLibrary.Service.Logger
{
    /// <summary>
    /// Основной класс логгера, реализует 2 интерфейса: ILogger и IDisposable.
    /// Содержит логику для осуществления логгирования
    /// </summary>
    public class FileLogger : ILogger, IDisposable
    {
        string filePath;
        static object _lock = new object();
        public FileLogger(string path)
        {
            filePath = path;
        }
        /// <summary>
        /// Этот метод возвращает объект IDisposable, который представляет некоторую область видимости для логгера. 
        /// В данном случае нам этот метод не важен, поэтому возвращаем значение this - ссылку на текущий объект класса, который реализует интерфейс IDisposable
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        IDisposable ILogger.BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose() { }
        /// <summary>
        /// Возвращает значения true или false, которые указыват, доступен ли логгер для использования. 
        /// Здесь можно здать различную логику. В частности, в этот метод передается объект LogLevel, и мы можем, к примеру, задействовать логгер в зависимости от значения этого объекта. 
        /// Но в данном случае просто возвращаем true, то есть логгер доступен всегда
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        bool ILogger.IsEnabled(LogLevel logLevel)
        {
            return true;
        }
        /// <summary>
        /// Этот метод предназначен для выполнения логгирования.
        /// И в данном методе как раз и производится запись в текстовый файл. Путь к этому файлу передается через конструктор.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId,
                    TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            lock (_lock)
            {
                File.AppendAllText(filePath, formatter(state, exception) + Environment.NewLine);
            }
        }
    }
}
