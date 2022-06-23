using System;
using System.Collections.Generic;
using DIContainer.DIExample.Helpers;
using DIContainer.DIExample.Repositories;

namespace DIContainer.DIExample.Services
{
    /// <summary>
    /// Сервис библиотеки.
    /// </summary>
    public class LibraryService : ILibraryService
    {
        /// <summary>
        /// Хранилище книг.
        /// </summary>
        public IBookRepository BookRepository { get; set; }

        /// <summary>
        /// Библиотекари.
        /// </summary>
        public IList<ILibrarian> Librarians { get; set; }

        /// <summary>
        /// Логгер.
        /// </summary>
        [DependencyProperty]
        public ILogger Logger { get; set; } = new BadLogger();
        
        /// <summary>
        /// Инициализирует поля объекта.
        /// </summary>
        /// <param name="bookRepository"> Хранилище книг. </param>
        public LibraryService(IBookRepository bookRepository)
        {
            BookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        /// <summary>
        /// Инициализирует поля объекта.
        /// </summary>
        /// <param name="bookRepository"> Хранилище книг. </param>
        /// <param name="librarians"> Библиотекари. </param>
        public LibraryService(IBookRepository bookRepository, IList<ILibrarian> librarians)
        {
            BookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            Librarians = librarians ?? throw new ArgumentNullException(nameof(librarians));
        }

        /// <summary>
        /// Возвращает коллекию книг.
        /// </summary>
        /// <returns> Коллекция книг. </returns>
        public List<object> GetBooks()
        {
            try
            {
                Logger.LogInfo($"Получение книг (метод {nameof(GetBooks)}) в сервисе библиотеки (сервис {nameof(LibraryService)}).");

                var books = BookRepository.GetBooks();

                if (Librarians == null)
                {
                    return books;
                }

                // Записать отданные книги в журнал.
                foreach (var librarian in Librarians)
                {
                    books.ForEach(b => librarian.WriteToJournal(b));
                }

                return books;
            }
            catch (Exception e)
            {
                Logger.LogError(e);
                throw;
            }
        }
    }
}