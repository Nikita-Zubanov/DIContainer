using System;
using System.Collections.Generic;

namespace DIContainer.DIExample.DataProviders
{
    /// <summary>
    /// Провайдер данных, предоставляющий методы для взаимодействия с БД.
    /// </summary>
    public class DbProvider : IDataProvider
    {
        /// <summary>
        /// Инициализирует поля объекта.
        /// </summary>
        /// <param name="dbConnectionString">
        /// Строка подключения к БД.
        /// Необходима, чтобы продемонстрировать возможность регистрации с помощью
        /// делегата, возвращающего экземпляр класса.
        /// </param>
        public DbProvider(string dbConnectionString)
        {
            if (string.IsNullOrWhiteSpace(dbConnectionString))
            {
                throw new ArgumentNullException(nameof(dbConnectionString));
            }
        }

        /// <summary>
        /// Возвращает коллекцию объектов заданного типа.
        /// </summary>
        /// <param name="type"> Тип данных. </param>
        /// <returns> Коллекция объектов. </returns>
        public List<object> GetEntities(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            return new List<object>
            {
                new {Name = "Book 1", Type = type},
                new {Name = "Book 2", Type = type},
                new {Name = "Book 3", Type = type}
            };
        }
    }
}