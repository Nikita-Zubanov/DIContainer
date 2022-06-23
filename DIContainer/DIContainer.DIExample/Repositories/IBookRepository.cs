using System.Collections.Generic;

namespace DIContainer.DIExample.Repositories
{
    /// <summary>
    /// Предоставляет интерфейс взаимодействия с книгами.
    /// </summary>
    public interface IBookRepository
    {
        /// <summary>
        /// Возвращает коллекцию книг.
        /// </summary>
        /// <param name="fromCache"> Получить книги из кэша. </param>
        /// <returns> Коллекция книг. </returns>
        List<object> GetBooks(bool fromCache = false);
    }
}