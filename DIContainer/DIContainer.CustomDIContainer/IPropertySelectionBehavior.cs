using System;
using System.Reflection;

namespace DIContainer.CustomDIContainer
{
    /// <summary>
    /// Определяет поведение DI-контейнера при выборе свойств для внедрения во время создания типа.
    /// </summary>
    public interface IPropertySelectionBehavior
    {
        /// <summary>
        /// Определяет, должно ли свойство класса внедряться контейнером при создании его типа.
        /// </summary>
        /// <param name="concreteType"> Конкретный тип создаваемого экземпляра класса. </param>
        /// <param name="propertyInfo"> Информация о свойстве. </param>
        /// <returns> True, если свойство должно быть внедрено. </returns>
        bool SelectProperty(Type concreteType, PropertyInfo propertyInfo);
    }
}