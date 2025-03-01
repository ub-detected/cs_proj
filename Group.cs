
    /// <summary>
    /// Класс для хранения данных
    /// Используется при "отрисовке" TUI  
    /// </summary>
    /// <param name="id"> Значение поля id записи (единицы данных - достижение) </param>
    /// <param name="label"> Значение поля label записи (единицы данных - достижение)</param>
    public class Group(string id, string label)
    {
        /// <summary>
        /// Свойство для label
        /// </summary>
        public string Label { get; private set; } = label;

        /// <summary>
        /// Свойство для id
        /// </summary>
        public string Id { get; private set; } = id;

        /// <summary>
        /// Свойство для данных
        /// </summary>
        public List<string> GroupContent { get; private set; } = [];


        /// <summary>
        /// Флаг (1/0), развернута ли группа данных в TUI
        /// </summary>
        public int opened_in_tui = 0;

        /// <summary>
        /// Метод для добавления строки в группу.
        /// </summary>
        /// <param name="sublabel"> Строка для добавления </param>
        public void Add(string sublabel)
        {
            GroupContent.Add(sublabel);
        }
    }
