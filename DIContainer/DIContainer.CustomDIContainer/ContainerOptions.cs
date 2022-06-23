namespace DIContainer.CustomDIContainer
{
    /// <summary>
    /// Конфигурация для DI-контейнера.
    /// </summary>
    public class ContainerOptions
    {
        /// <summary>
        /// Поведение DI-контейнера при выборе свойств для внедрения во время создания типа.
        /// По умолчанию контейнер не внедряет свойства.
        /// </summary>
        public IPropertySelectionBehavior PropertySelectionBehavior { get; set; }
    }
}