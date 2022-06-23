namespace DIContainer.DIExample.Helpers
{
    /// <summary>
    /// Предоставляет методы ведения журнала.
    /// </summary>
    public interface ILibrarian
    {
        /// <summary>
        /// Сделать запись в журнале.
        /// </summary>
        /// <param name="book"> Книга. </param>
        void WriteToJournal(object book);
    }
}
