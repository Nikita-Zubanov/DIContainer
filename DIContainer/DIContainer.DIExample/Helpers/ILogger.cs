using System;

namespace DIContainer.DIExample.Helpers
{
    /// <summary>
    /// Предоставляет интерфейс для ведения логов.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Записать информационное сообщение.
        /// </summary>
        /// <param name="message"> Тест сообщения. </param>
        void LogInfo(string message);

        /// <summary>
        /// Записать исключение.
        /// </summary>
        /// <param name="ex"> Исключение. </param>
        void LogError(Exception ex);
    }
}