using System;

namespace DIContainer.DIExample.Helpers
{
    /// <summary>
    /// Плохой логгер.
    /// </summary>
    public class BadLogger : ILogger
    {
        /// <summary>
        /// Сообщение, если логгер забыл сообщение.
        /// </summary>
        private const string ForgotMessage = "Блин, забыл сообщение(((";

        /// <summary>
        /// Записать (возможно) информационное сообщение.
        /// </summary>
        /// <param name="message"> Тест сообщения. </param>
        public void LogInfo(string message)
        {
            var newMessage = new Random().Next(0, 2) == 0
                ? ForgotMessage
                : message;

            Log(newMessage,  "Info");
        }

        /// <summary>
        /// Записать исключение (только сообщение, если повезет).
        /// </summary>
        /// <param name="ex"> Исключение. </param>
        public void LogError(Exception ex)
        {
            var newMessage = new Random().Next(0, 2) == 0
                ? ForgotMessage
                : ex.Message;

            Log(newMessage, "Error");
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