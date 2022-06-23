using System;
using DIContainer.CustomDIContainer.Registrations;

namespace DIContainer.CustomDIContainer.Lifestyles
{
    /// <summary>
    /// Фабрика, делегирующая подклассам создание экземпляров класса регистрации с определенным жизненным циклом.
    /// </summary>
    public abstract class Lifestyle
    {
        /// <summary>
        /// Фабрика по умолчанию.
        /// </summary>
        public static readonly Lifestyle DefaultLifestyle = new SingletonLifestyle();

        /// <summary>
        /// Фабрика, создающая экземпляры класса регистрации с жизненным циклом "singleton".
        /// </summary>
        public static readonly Lifestyle Singleton = new SingletonLifestyle();

        /// <summary>
        /// Фабрика, создающая экземпляры класса регистрации с жизненным циклом "transient".
        /// </summary>
        public static readonly Lifestyle Transient = new TransientLifestyle();

        /// <summary>
        /// Создает экземпляр класса регистрации.
        /// </summary>
        /// <param name="concreteType"> Конкретный тип экземпляра класса. </param>
        /// <param name="container"> DI-контейнер. </param>
        /// <returns> Экземпляр класса регистрации. </returns>
        internal abstract Registration CreateRegistration(Type concreteType, Container container);

        /// <summary>
        /// Создает экземпляр класса регистрации.
        /// </summary>
        /// <param name="createInstanceCallback"> Возвращает инициализированный экземпляр класса. </param>
        /// <param name="container"> DI-контейнер. </param>
        /// <returns> Экземпляр класса регистрации. </returns>
        internal abstract Registration CreateRegistration(Func<object> createInstanceCallback, Container container);
    }
}