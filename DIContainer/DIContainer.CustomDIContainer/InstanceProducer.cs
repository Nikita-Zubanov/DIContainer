using System;
using DIContainer.CustomDIContainer.Registrations;

namespace DIContainer.CustomDIContainer
{
    /// <summary>
    /// Производитель экземпляра абстрактного класса. 
    /// </summary>
    internal class InstanceProducer
    {
        /// <summary>
        /// Абстрактный тип порождаемого класса.
        /// </summary>
        public Type AbstractType { get; set; }

        /// <summary>
        /// Управляет созданием экземпляра класса абстрактного типа.
        /// </summary>
        public Registration Registration { get; set; }

        /// <summary>
        /// Инициализирует поля класса.
        /// </summary>
        /// <param name="abstractType"> Абстрактный тип порождаемого класса. </param>
        /// <param name="registration"> Управляет созданием экземпляра класса абстрактного типа. </param>
        public InstanceProducer(Type abstractType, Registration registration)
        {
            AbstractType = abstractType ?? throw new ArgumentNullException(nameof(abstractType));
            Registration = registration ?? throw new ArgumentNullException(nameof(registration));
        }

        /// <summary>
        /// Возвращает экземпляр подкласса абстрактного типа с инициализированной иерархией зависимостей.
        /// </summary>
        /// <returns> Экземпляр подкласса абстрактного типа. </returns>
        public object GetInstance()
        {
            return Registration.CreateInstance();
        }
    }
}