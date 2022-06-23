using System;

namespace DIContainer.DIExample.Helpers
{
    /// <summary>
    /// Библиотекарь.
    /// </summary>
    public class Librarian : ILibrarian
    {
        /// <summary>
        /// Сделать запись в журнале.
        /// </summary>
        /// <param name="book"> Книга. </param>
        public void WriteToJournal(object book)
        {
            Console.WriteLine($"[Запись журнала] Книга \"{book}\" была отдана.");
        }
    }
}