
    /// <summary>
    /// Структура для хранения информации об единице данных (одном достижении).
    /// </summary>
    public readonly struct AchieveUnit : IJSONObject
    {
        /// <summary>
        /// Словарь для хранения ключ-значения
        /// </summary>
        private readonly Dictionary<string, string> map = [];
        
        /// <summary>
        /// Конструктор
        /// </summary>
        public AchieveUnit(){}

        /// <summary>
        /// Метод для получения всех полей
        /// </summary>
        /// <returns>Все поля</returns>
        public readonly IEnumerable<string> GetAllFields()
        {
            return map.Keys;
        }

        /// <summary>
        /// Метод для получения значения конкретного поля
        /// </summary>
        /// <param name="fieldName">Поле</param>
        /// <returns>Значение поля</returns>
        public string GetField(string fieldName)
        {
            return map[fieldName];
        }

        /// <summary>
        /// Метод для установления значения для конкретного поля
        /// </summary>
        /// <param name="fieldName">Поле</param>
        /// <param name="value">Значение</param>
        public void SetField(string fieldName, string value)
        {
            map[fieldName] = value;
        }
    }
