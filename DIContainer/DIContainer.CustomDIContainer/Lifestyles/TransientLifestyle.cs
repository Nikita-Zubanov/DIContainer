using System;
using DIContainer.CustomDIContainer.Registrations;

namespace DIContainer.CustomDIContainer.Lifestyles
{
    /// <summary>
    /// Фабрика, порождающая экземпляры класса регистрации с жизненным циклом "transient".
    /// </summary>
    public class TransientLifestyle : Lifestyle
    {
        /// <summary>
        /// Создает экземпляр класса регистрации.
        /// </summary>
        /// <param name="concreteType"> Конкретный тип экземпляра класса. </param>
        /// <param name="container"> DI-контейнер. </param>
        /// <returns> Экземпляр класса регистрации. </returns>
        internal override Registration CreateRegistration(Type concreteType, Container container)
        {
            if (concreteType == null)
            {
                throw new ArgumentNullException(nameof(concreteType));
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            return new TransientRegistration(concreteType, container);
        }

        /// <summary>
        /// Создает экземпляр класса регистрации.
        /// </summary>
        /// <param name="createInstanceCallback"> Возвращает инициализированный экземпляр класса. </param>
        /// <param name="container"> DI-контейнер. </param>
        /// <returns> Экземпляр класса регистрации. </returns>
        internal override Registration CreateRegistration(Func<object> createInstanceCallback, Container container)
        {
            if (createInstanceCallback == null)
            {
                throw new ArgumentNullException(nameof(createInstanceCallback));
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            return new TransientRegistration(createInstanceCallback, container);
        }
    }
}