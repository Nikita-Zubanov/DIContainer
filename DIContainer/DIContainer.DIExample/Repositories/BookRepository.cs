using System;
using System.Collections.Generic;
using DIContainer.DIExample.DataProviders;

namespace DIContainer.DIExample.Repositories
{
    /// <summary>
    /// Хранилище, предоставляющее методы для взаимодействия с книгами.
    /// </summary>
    public class BookRepository : IBookRepository
    {
        /// <summary>
        /// Провайдер данных.
        /// </summary>
        private readonly IDataProvider _dataProvider;

        /// <summary>
        /// Коллекция книг, полученных при последнем запросе.
        /// </summary>
        public List<object> CacheBooks { get; private set; }

        /// <summary>
        /// Инициализирует поля объекта.
        /// </summary>
        /// <param name="dataProvider"> Провайдер данных. </param>
        public BookRepository(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        /// <summary>
        /// Возвращает коллекцию книг.
        /// </summary>
        /// <returns> Коллекция книг. </returns>
        /// <param name="fromCache"> Получить книги из кэша. </param>
        public List<object> GetBooks(bool fromCache = false)
        {
            if (!fromCache)
            {
                var books = _dataProvider.GetEntities("Book");
                CacheBooks = books;

                return books;
            }

            if (CacheBooks == null)
            {
                CacheBooks = _dataProvider.GetEntities("Book");
            }

            return CacheBooks;
        }
    }
}