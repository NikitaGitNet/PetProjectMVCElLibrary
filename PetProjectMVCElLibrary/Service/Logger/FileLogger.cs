﻿namespace PetProjectMVCElLibrary.Service.Logger
{
    public class FileLogger : ILogger, IDisposable
    {
        string filePath;
        static object _lock = new object();
        public FileLogger(string path)
        {
            filePath = path;
        }
        IDisposable ILogger.BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose() { }

        bool ILogger.IsEnabled(LogLevel logLevel)
        {
            //return logLevel == LogLevel.Trace;
            return true;
        }

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
