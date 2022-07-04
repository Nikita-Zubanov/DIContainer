using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DIContainer.CustomDIContainer;
using DIContainer.CustomDIContainer.Lifestyles;
using DIContainer.DIExample;
using DIContainer.DIExample.DataProviders;
using DIContainer.DIExample.Helpers;
using DIContainer.DIExample.Repositories;
using DIContainer.DIExample.Services;

namespace DIContainer.Tests
{
	/// <summary>
	/// Тестирование примера реализации кастомного DI-контейнера.
	/// </summary>
	[TestClass]
    public class DIExampleTest
    {
        /// <summary>
        /// Два раза создает хранилище, демонстрируя, что возвращается один и тот же экземпляр,
        /// так как второе хранилище возвращает закэшированные книги из первого хранилища.
        /// </summary>
        [TestMethod]
        public void CachedBooksIsExists()
        {
            var container = new Container();
            // Используется хранилище с жизненным циклом "singleton".
            container.Register<IBookRepository, BookRepository>(Lifestyle.Singleton);
            container.Register<IDataProvider>(
                () => new DbProvider("server=none;db=none;"),
                Lifestyle.Singleton);

            var repository = container.GetInstance<IBookRepository>();
            // Получить книги из хранилища, притом закэшировав их.
            var books = repository.GetBooks();

            var newRepository = container.GetInstance<IBookRepository>();
            // Получены закэшированные при первом запросе (GetBooks) книги.
            var cachedBooks = ((BookRepository)newRepository).CacheBooks;

            CollectionAssert.AreEquivalent(books, cachedBooks);
        }

        /// <summary>
        /// Два раза создает хранилище, и дважды создаются разные хранилища,
        /// так как у второго хранилища закэшированные книги равны null.
        /// </summary>
        [TestMethod]
        public void CachedBooksIsEmpty()
        {
            var container = new Container();
            // Используется хранилище с жизненным циклом "transient".
            container.Register<IBookRepository, BookRepository>(Lifestyle.Transient);
            container.Register<IDataProvider>(
                () => new DbProvider("server=none;db=none;"),
                Lifestyle.Transient);

            var repository = container.GetInstance<IBookRepository>();
            // Получить книги из хранилища, притом закэшировав их в хранилище.
            repository.GetBooks();

            var newRepository = container.GetInstance<IBookRepository>();
            var cachedBooks = ((BookRepository)newRepository).CacheBooks;

            // Закэшированные книги равны null, так как первое и второе хранилища – два разных объекта.
            Assert.IsNull(cachedBooks);
        }

        /// <summary>
        /// Проверяет, что в сервис библиотеки внедряется адекватный логгер <see cref="Logger"/>, вместо плохого <see cref="BadLogger"/>.
        /// Примечание: логгер можно внедрить только через свойство. 
        /// </summary>
        [TestMethod]
        public void InjectDependenciesThroughProperty()
        {
            var container = new Container();
            container.Options.PropertySelectionBehavior = new DependencyAttributeSelectionBehaviour();
            container.Register<ILibraryService, LibraryService>();
            container.Register<IBookRepository, BookRepository>();
            container.Register<ILogger, Logger>();
            container.Register<IDataProvider>(() => new DbProvider("String connection."));

            var library = (LibraryService)container.GetInstance<ILibraryService>();

            Assert.IsTrue(library.Logger is Logger);
        }

        /// <summary>
        /// Проверяет, что можно внедрять массивы.
        /// </summary>
        [TestMethod]
        public void InjectDependencyArray()
        {
            var container = new Container();
            container.Register<ILibraryService, LibraryService>();
            container.Register<IBookRepository, BookRepository>();
            container.Register<ILogger, Logger>();
            container.Register<IDataProvider>(() => new DbProvider("String connection."));
            // Зарегистрировать реализации для внедрения массива.
            container.Register<ILibrarian, Librarian>();
            container.Register<ILibrarian, BadLibrarian>();

            var library = (LibraryService)container.GetInstance<ILibraryService>();

            Assert.IsTrue(library.Librarians.Count == 2);
            Assert.IsTrue(library.Librarians.First() is BadLibrarian);
            Assert.IsTrue(library.Librarians.Last() is Librarian);
        }

        /// <summary>
        /// Проверяет, что сгенерируется исключение, если нужная зависимость не будет зарегистрирована.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ThrowExceptionIfDependencyIsNotRegistered()
        {
            var container = new Container();
            /// Для <see cref="BookRepository"/> необходимо зарегестрировать тип с интерфейсом <see cref="IDataProvider"/>.
            container.Register<IBookRepository, BookRepository>();

            container.GetInstance<IBookRepository>();
        }
    }
}
