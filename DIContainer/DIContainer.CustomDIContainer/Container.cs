using System;
using System.Collections.Generic;
using System.Linq;
using DIContainer.CustomDIContainer.Lifestyles;

namespace DIContainer.CustomDIContainer
{
    /// <summary>
    /// DI-контейнер позволяет регистрировать экземпляры классов для абстракций и
    /// получать требуемый экземпляр класса с инициализированной иерархией.
    /// </summary>
    public class Container
    {
        /// <summary>
        /// Конфигурация для DI-контейнера.
        /// </summary>
        public ContainerOptions Options { get; }

        /// <summary>
        /// Коллекция производителей экземпляров классов.
        /// </summary>
        private readonly Stack<InstanceProducer> _instanceProducers;

        /// <summary>
        /// Инициализирует поля объекта.
        /// </summary>
        public Container()
        {
            Options = new ContainerOptions();
            _instanceProducers = new Stack<InstanceProducer>(); 
        }

        /// <summary>
        /// Регистрирует абстрактный тип и его замещаемый подтип.
        /// </summary>
        /// <typeparam name="TAbstract"> Абстрактный тип. </typeparam>
        /// <typeparam name="TConcrete"> Конкретный тип. </typeparam>
        public void Register<TAbstract, TConcrete>()
            where TAbstract : class
            where TConcrete : class, TAbstract
        {
            Register<TAbstract, TConcrete>(Lifestyle.DefaultLifestyle);
        }

        /// <summary>
        /// Регистрирует абстрактный тип и его замещаемый подтип.
        /// </summary>
        /// <typeparam name="TAbstract"> Абстрактный тип. </typeparam>
        /// <typeparam name="TConcrete"> Конкретный тип. </typeparam>
        /// <param name="lifestyle"> Управляет жизненным циклом создаваемых экземпляров класса. </param>
        public void Register<TAbstract, TConcrete>(Lifestyle lifestyle)
            where TAbstract : class
            where TConcrete : class, TAbstract
        {
            if (lifestyle == null)
            {
                throw new ArgumentNullException(nameof(lifestyle));
            }

            var registration = lifestyle.CreateRegistration(typeof(TConcrete), this);
            var instanceProducer = new InstanceProducer(typeof(TAbstract), registration);

            _instanceProducers.Push(instanceProducer);
        }

        /// <summary>
        /// Регистрирует абстрактный тип и делегат, возвращающий экземпляр класса его подтипа.
        /// </summary>
        /// <typeparam name="TAbstract"> Абстрактный тип. </typeparam>
        /// <param name="createInstanceCallback"> Возвращает инициализированный экземпляр класса. </param>
        public void Register<TAbstract>(Func<TAbstract> createInstanceCallback)
            where TAbstract : class
        {
            if (createInstanceCallback == null)
            {
                throw new ArgumentNullException(nameof(createInstanceCallback));
            }

            Register(createInstanceCallback, Lifestyle.DefaultLifestyle);
        }

        /// <summary>
        /// Регистрирует абстрактный тип и делегат, возвращающий экземпляр класса его подтипа.
        /// </summary>
        /// <typeparam name="TAbstract"> Абстрактный тип. </typeparam>
        /// <param name="createInstanceCallback"> Возвращает инициализированный экземпляр класса. </param>
        /// <param name="lifestyle"> Управляет жизненным циклом создаваемых экземпляров класса. </param>
        public void Register<TAbstract>(Func<TAbstract> createInstanceCallback, Lifestyle lifestyle)
            where TAbstract : class
        {
            if (createInstanceCallback == null)
            {
                throw new ArgumentNullException(nameof(createInstanceCallback));
            }

            if (lifestyle == null)
            {
                throw new ArgumentNullException(nameof(lifestyle));
            }

            var registration = lifestyle.CreateRegistration(createInstanceCallback, this);
            var instanceProducer = new InstanceProducer(typeof(TAbstract), registration);

            _instanceProducers.Push(instanceProducer);
        }

        /// <summary>
        /// Возвращает экземпляр производного класса абстрактного типа с инициализированной иерархией зависимостей.
        /// </summary>
        /// <typeparam name="TAbstract"> Абстрактный тип. </typeparam>
        /// <returns> Экземпляр производного класса абстрактного типа. </returns>
        public TAbstract GetInstance<TAbstract>()
            where TAbstract : class
        {
            return (TAbstract) GetInstance(typeof(TAbstract));
        }

        /// <summary>
        /// Возвращает экземпляр производного класса абстрактного типа с инициализированной иерархией зависимостей.
        /// </summary>
        /// <param name="type"> Тип абстрактного класса. </param>
        /// <returns> Экземпляр производного класса абстрактного типа. </returns>
        public object GetInstance(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var instanceProducer = _instanceProducers.FirstOrDefault(ip => ip.AbstractType == type);
            if (instanceProducer == null)
			{
                throw new NullReferenceException($"Не удалось сформировать экземпляр класса \"{type}\", так как он не был зарегистрирован.");
			}

            return instanceProducer.GetInstance();
        }

        /// <summary>
        /// Возвращает экземпляр производного класса абстрактного типа с инициализированной иерархией зависимостей.
        /// </summary>
        /// <param name="type"> Тип абстрактного класса. </param>
        /// <returns> Список экземпляров производных классов абстрактного типа. </returns>
        internal List<object> GetInstances(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var instanceProducers = _instanceProducers.Where(ip => ip.AbstractType == type);

            var instances = new List<object>();
            foreach (var instanceProducer in instanceProducers)
            {
                instances.Add(instanceProducer.GetInstance());
            }

            return instances;
        }

        /// <summary>
        /// Возвращает признак, указывающий, что переданный тип зарегистрирован.
        /// </summary>
        /// <param name="type"> Тип абстрактного класса. </param>
        /// <returns> True, если зарегистрирован. </returns>
        internal bool IsRegistered(Type type)
		{
            return _instanceProducers.Any(ip => ip.AbstractType == type);
        }
    }
}