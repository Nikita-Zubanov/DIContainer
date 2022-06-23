using System;

namespace DIContainer.DIExample
{
    /// <summary>
    /// Атрибут, указывающий, что свойство необходимо внедрять с помощью DI-контейнера.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DependencyProperty : Attribute
    {
    }
}