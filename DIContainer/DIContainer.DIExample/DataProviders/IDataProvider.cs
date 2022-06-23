using System.Collections.Generic;

namespace DIContainer.DIExample.DataProviders
{
    /// <summary>
    /// Интерфейс взаимодействия с хранилищем данных.
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// Возвращает коллекцию объектов заданного типа.
        /// </summary>
        /// <param name="type"> Тип данных. </param>
        /// <returns> Коллекция объектов. </returns>
        List<object> GetEntities(string type);
    }
}