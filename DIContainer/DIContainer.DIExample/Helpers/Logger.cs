using System;

namespace DIContainer.DIExample.Helpers
{
    /// <summary>
    /// Логгер.
    /// </summary>
    public class Logger : ILogger
    {
        /// <summary>
        /// Записать информационное сообщение.
        /// </summary>
        /// <param name="message"> Тест сообщения. </param>
        public void LogInfo(string message)
        {
            Log(message, "Info");
        }

        /// <summary>
        /// Записать исключение.
        /// </summary>
        /// <param name="ex"> Исключение. </param>
        public void LogError(Exception ex)
        {
            Log(ex.ToString(), "Error");
        }

        /// <summary>
        /// Записывает сообщение в консоль.
        /// </summary>
        /// <param name="message"> Текст сообщения. </param>
        /// <param name="messageType"> Тип сообщения. </param>
        private void Log(string message, string messageType)
        {
            Console.WriteLine($"[{messageType}] {message}");
        }
    }
}