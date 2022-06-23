using System;
using System.Linq;
using System.Reflection;
using DIContainer.CustomDIContainer;

namespace DIContainer.DIExample
{
    /// <summary>
    /// Определяет поведение DI-контейнера при выборе свойств для внедрения во время создания типа.
    /// </summary>
    public class DependencyAttributeSelectionBehaviour : IPropertySelectionBehavior
    {
        /// <summary>
        /// Если свойство помечено атрибутом <see cref="DependencyProperty"/>, то внедрять его при создании экземпляра класса.
        /// </summary>
        /// <param name="concreteType"> Конкретный тип создаваемого экземпляра класса. </param>
        /// <param name="propertyInfo"> Информация о свойстве. </param>
        /// <returns> True, если свойство должно быть внедрено. </returns>
        public bool SelectProperty(Type concreteType, PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(typeof(DependencyProperty)).Any();
        }
    }
}