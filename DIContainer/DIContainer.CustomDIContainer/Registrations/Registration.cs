using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DIContainer.CustomDIContainer.Registrations
{
    /// <summary>
    /// Регистрация позволяет создавать экземпляр класса вместе со всей его иерархией,
    /// инициализируя параметры конструктора и свойства (если пользователем предоставлена такая возможность).
    /// Позволяет наследникам управлять временем жизни создаваемого экземпляра класса.
    /// </summary>
    internal abstract class Registration
    {
        /// <summary>
        /// DI-контейнер.
        /// </summary>
        public Container Container { get; }

        /// <summary>
        /// Конкретный тип.
        /// </summary>
        public Type ConcreteType { get; }

        /// <summary>
        /// Возвращает инициализированный экземпляр класса.
        /// </summary>
        public Func<object> CreateInstanceCallback { get; }

        /// <summary>
        /// Инициализирует поля объекта.
        /// </summary>
        /// <param name="concreteType"> Конкретный тип. </param>
        /// <param name="container"> DI-контейнер. </param>
        protected Registration(Type concreteType, Container container)
        {
            ConcreteType = concreteType ?? throw new ArgumentNullException(nameof(concreteType));
            Container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <summary>
        /// Инициализирует поля объекта.
        /// </summary>
        /// <param name="createInstanceCallback"> Возвращает инициализированный экземпляр класса. </param>
        /// <param name="container"> DI-контейнер. </param>
        protected Registration(Func<object> createInstanceCallback, Container container)
        {
            CreateInstanceCallback = createInstanceCallback
                                     ?? throw new ArgumentNullException(nameof(createInstanceCallback));
            Container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <summary>
        /// Создает экземпляр класса с инициализированной иерархией зависимых классов.
        /// </summary>
        /// <returns> Экземпляр класса. </returns>
        public virtual object CreateInstance()
        {
            if (CreateInstanceCallback != null)
            {
                return CreateInstanceCallback();
            }

            var instance = CreateInstanceAndInitDependencies(ConcreteType);
            if (Container.Options.PropertySelectionBehavior != null)
            {
                InitializeProperties(ConcreteType, instance);
            }

            return instance;
        }

        /// <summary>
        /// Создает экземпляр класса с помощью его конструктора. Если в качестве параметров конструктора есть зависимости,
        /// то создает их с помощью DI-контейнера, рекурсивно инициализируя всю иерархию зависимых классов.
        /// </summary>
        /// <param name="instanceType"> Тип создаваемого экземпляра класса. </param>
        /// <returns> Экземпляр класса. </returns>
        private object CreateInstanceAndInitDependencies(Type instanceType)
        {
            var constructorsInfo = instanceType
                .GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .ToList();
            foreach (var constructorInfo in constructorsInfo)
            {
                if (!IsAllConstructorParametersRegistered(instanceType.FullName, constructorInfo))
				{
                    continue;
                }

                var constructorParams = constructorInfo
                    .GetParameters()
                    .Select(p =>
                    {
                        var paramType = p.ParameterType;

                        var isEnumerable = paramType.GetInterface(nameof(IEnumerable)) != null;
                        if (isEnumerable)
                        {
                            return CreateListFromGenericListType(paramType);
                        }

                        return Container.GetInstance(paramType);
                    })
                    .ToArray();

                return Activator.CreateInstance(instanceType, constructorParams);
            }

            throw new Exception($"Невозможно создать объект типа \"{instanceType}\", так как нет ни одного " +
                                "конструктора, все типы аргументов которого были бы зарегистрированы.");
        }

        /// <summary>
        /// Возвращает признак, указывающий на то, что все типы параметров конструктора зарегестрированы.
        /// </summary>
        /// <param name="instanceName"> Название типа экземпляра класса. </param>
        /// <param name="constructorInfo"> Информация о конструкторе. </param>
        /// <returns> True, если все типы параметров зарегестрированы. </returns>
        private bool IsAllConstructorParametersRegistered(string instanceName, ConstructorInfo constructorInfo)
        {
            return constructorInfo
                .GetParameters()
                .All(p =>
                    {
                        ValidateConstructorParameters(instanceName, p);

                        var paramType = p.ParameterType;

                        var isEnumerable = paramType.GetInterface(nameof(IEnumerable)) != null;
                        if (isEnumerable)
                        {
                            var itemType = paramType.GenericTypeArguments.First();
                            return Container.IsRegistered(itemType);
                        }

                        return Container.IsRegistered(paramType);
                    });
        }

        /// <summary>
        /// Инициализирует свойства экземпляра класса, если они помечены пользовательским атрибутом и равны null.
        /// </summary>
        /// <param name="instanceType"> Тип экземпляра класса. </param>
        /// <param name="instance"> Экземпляр класса. </param>
        private void InitializeProperties(Type instanceType, object instance)
        {
            var properties = instanceType.GetProperties();

            foreach (var prop in properties)
            {
                var isDependencies = Container.Options.PropertySelectionBehavior.SelectProperty(instanceType, prop);

                if (isDependencies)
                {
                    var propertyInstance = Container.GetInstance(prop.PropertyType);
                    prop.SetValue(instance, propertyInstance);
                }
            }
        }

        /// <summary>
        /// Проверяет, чтобы в качестве параметров конструктора не было строки, значимого типа, массива или перечисляемого
        /// объекта, так как данные типы данных невозможно зарегистрировать (в таком случае необходимо передавать делегат,
        /// возвращающий экземпляр класса, а не желаемый конкретный тип).
        /// </summary>
        /// <param name="instanceName"> Название типа экземпляра класса. </param>
        /// <param name="parameterInfo"> Информация о параметре. </param>
        private void ValidateConstructorParameters(string instanceName, ParameterInfo parameterInfo)
        {
            var paramType = parameterInfo.ParameterType;

            if (paramType == typeof(string)
                || paramType.IsValueType
                || paramType.IsArray)
            {
                throw new Exception($"Конструктор типа {instanceName} имеет параметр с недопустимым типом {paramType}. " +
                                    "Для разрешения конфликта для данного типа используйте замыкание.");
            }
        }

        /// <summary>
        /// Формирует список объектов по переданному типу списка абстракций.
        /// </summary>
        /// <param name="type"> Тип обобщенного списка абстракций. </param>
        /// <returns> Список объектов. </returns>
        private object CreateListFromGenericListType(Type type)
        {
            var itemType = type.GenericTypeArguments.First();
            var items = Container.GetInstances(itemType);

            var listValues = Array.CreateInstance(itemType, items.Count);
            for (var i = 0; i < items.Count; i++)
            {
                listValues.SetValue(items[i], i);
            }

            var genericListType = typeof(List<>);
            var concreteListType = genericListType.MakeGenericType(itemType);

            return Activator.CreateInstance(concreteListType, listValues);
        }
    }
}