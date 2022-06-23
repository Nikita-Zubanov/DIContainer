using System;

namespace DIContainer.CustomDIContainer.Registrations
{
    /// <summary>
    /// "Временная" регистрация позволяет создавать новые экземпляры класса при каждом обращении к DI-контейнеру.
    /// </summary>
    internal class TransientRegistration : Registration
    {
        /// <summary>
        /// Инициализирует поля объекта.
        /// </summary>
        /// <param name="concreteType"> Конкретный тип. </param>
        /// <param name="container"> DI-контейнер. </param>
        public TransientRegistration(Type concreteType, Container container) : base(concreteType, container)
        {
        }

        /// <summary>
        /// Инициализирует поля объекта.
        /// </summary>
        /// <param name="createInstanceCallback"> Возвращает инициализированный экземпляр класса. </param>
        /// <param name="container"> DI-контейнер. </param>
        public TransientRegistration(Func<object> createInstanceCallback, Container container) : base(createInstanceCallback, container)
        {
        }
    }
}