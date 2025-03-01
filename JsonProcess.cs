
    /// <summary>
    /// Класс для обработки данных, а именно фильтрации и сортировки
    /// </summary>
    public static class JsonProcess
    {
        /// <summary>
        /// Метод для получения от пользователя, параметров для сортировки/фильтрации
        /// </summary>
        /// <param name="lst">Обрабатываемые данные</param>
        /// <param name="flag">Флаг. Если true - параметры для сортировки, false - для фильтрации </param>
        /// <returns> Параметры сортировки/фильтрации </returns>
        private static (string, string) GetOptions(List<IJSONObject> lst, bool flag=false)
        {
            Dictionary<string, bool> all_keys = [];
            foreach (IJSONObject i in lst)
            {
                foreach (string j in i.GetAllFields())
                {
                    all_keys[j] = true;
                }
            }

            foreach (string j in all_keys.Keys)
            {
                Console.WriteLine(j);
            }

            string? key_;
            Console.WriteLine(Info.del);

            while (true)
            {
                Console.WriteLine("Введите поле, выбрав из предложенных выше (указаны все поля из файла):");
                Console.WriteLine(Info.del);
                key_ = Console.ReadLine();

                if (key_ == null || all_keys.ContainsKey(key_) == false)
                {
                    Console.WriteLine("Некорректный ввод.");
                    Console.WriteLine(Info.del);
                    continue;
                }
                break;
            }

            if (flag)
            {
                return (key_, key_);
            }

            string? value_;
            while (true)
            {
                Console.WriteLine("Введите значение этого поля для фильтрации:");
                Console.WriteLine(Info.del);
                value_ = Console.ReadLine();

                if (value_ == null)
                {
                    Console.WriteLine("Некорректный (пустой) ввод.");
                    Console.WriteLine(Info.del);
                    continue;
                }
                break;
            }

            return (key_, value_);
        }

        /// <summary>
        /// Метод для фильтрации данных
        /// </summary>
        /// <param name="lst"> Данные </param>
        /// <returns> Данные после фильтрации </returns>
        public static List<IJSONObject> Filter(List<IJSONObject> lst)
        {
            try {
                string key_, value_;
                (key_, value_) = GetOptions(lst);
                List<IJSONObject> new_lst = [];

                foreach (IJSONObject i in lst)
                {
                    if (i.GetAllFields().Contains(key_) && i.GetField(key_) == value_)
                    {
                        new_lst.Add(i);
                    }
                }
                return new_lst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
        }

        /// <summary>
        /// Метод для сортировки данных
        /// </summary>
        /// <param name="lst"> Данные </param>
        /// <returns> Данные после сортировки </returns>
        public static List<IJSONObject> Sort(List<IJSONObject> lst)
        {
            try {
                string key_;
                (key_, key_) = GetOptions(lst, true);
                lst = [.. lst.OrderBy(x => x.GetAllFields().Contains(key_) ? x.GetField(key_) : null).ThenBy(x => x.GetAllFields().Contains(key_) ? 0 : 1)];
                return lst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return [];
            }
        }
    }
