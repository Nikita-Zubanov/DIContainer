using System.Collections.Generic;

namespace DIContainer.DIExample.Services
{
    /// <summary>
    /// Предоставляет интерфейс для взаимодействия с библиотекой.
    /// </summary>
    public interface ILibraryService
    {
        /// <summary>
        /// Возвращает коллекцию книг.
        /// </summary>
        /// <returns> Коллекция книг. </returns>
        List<object> GetBooks();
    }
}