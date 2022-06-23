using System;

namespace DIContainer.CustomDIContainer.Registrations
{
    /// <summary>
    /// Регистрация-одиночка позволяет создавать экземпляры класса единожды.
    /// </summary>
    internal class SingletonRegistration : Registration
    {
        /// <summary>
        /// Единственный экземпляр класса.
        /// </summary>
        private object _instance;

        /// <summary>
        /// Объект, управляющий синхронизацией потоков.
        /// </summary>
        private readonly object _locker = new object();

        /// <summary>
        /// Инициализирует поля объекта.
        /// </summary>
        /// <param name="concreteType"> Конкретный тип. </param>
        /// <param name="container"> DI-контейнер. </param>
        public SingletonRegistration(Type concreteType, Container container)
            : base(concreteType, container)
        {
        }

        /// <summary>
        /// Инициализирует поля объекта.
        /// </summary>
        /// <param name="createInstanceCallback"> Возвращает инициализированный экземпляр класса. </param>
        /// <param name="container"> DI-контейнер. </param>
        public SingletonRegistration(Func<object> createInstanceCallback, Container container) 
            : base(createInstanceCallback, container)
        {
        }

        /// <summary>
        /// Создает единственный экземпляр класса с инициализированной иерархией зависимых классов.
        /// </summary>
        /// <returns> Экземпляр класса. </returns>
        public override object CreateInstance()
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    if (_instance == null)
                    {
                        _instance = base.CreateInstance();
                    }
                }
            }

            return _instance;
        }
    }
}