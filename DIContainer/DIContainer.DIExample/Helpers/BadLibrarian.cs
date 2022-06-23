using System;

namespace DIContainer.DIExample.Helpers
{
    /// <summary>
    /// Плохой библиотекарь.
    /// </summary>
    public class BadLibrarian : ILibrarian
    {
        /// <summary>
        /// Сделать запись в журнале, если библиотекарь находится на рабочем месте, что не факт.
        /// </summary>
        /// <param name="book"> Книга. </param>
        public void WriteToJournal(object book)
        {
            var isWorkplaceLibrarian = new Random().Next(0, 5) == 0;

            if (isWorkplaceLibrarian)
            {
                Console.WriteLine($"[Запись журнала] Книга \"{book}\" была отдана.");
            }
        }
    }
}