
    /// <summary>
    /// Интерфейс из условия
    /// </summary>
    public interface IJSONObject
    {
        /// <summary>
        /// Метод для получения всех полей
        /// </summary>
        /// <returns>Все поля</returns>
        IEnumerable<string> GetAllFields();

        /// <summary>
        /// Метод для получения значения конкретного поля
        /// </summary>
        /// <param name="fieldName">Поле</param>
        /// <returns>Значение поля</returns>
        string GetField(string fieldName);

        /// <summary>
        /// Метод для установления значения для конкретного поля
        /// </summary>
        /// <param name="fieldName">Поле</param>
        /// <param name="value">Значение</param>
        void SetField(string fieldName, string value);
    }
