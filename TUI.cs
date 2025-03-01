    /// <summary>
    /// Класс для отображения данных в консоли (TUI)
    /// </summary>
    public class TUI
    {
        /// <summary>
        /// Метод для получения значения поля. Учитывает отсутствие поля.
        /// </summary>
        /// <param name="o"> Объект, значение поля которого хотим получить</param>
        /// <param name="p"> Поле, значение которого хотим получить</param>
        /// <returns></returns>
        private static string GetField(IJSONObject o, string p)
        {
            return o.GetAllFields().Contains(p) ? o.GetField(p) : "--";
        }

        /// <summary>
        /// List для хранения самих данных, заполняется (в конструкторе) при создании объекта типа TUI.
        /// </summary>
        private readonly List<Group> lst = [];

        /// <summary>
        /// Номер рассматриваемой группы
        /// </summary>
        private int cur = 0;

        /// <summary>
        /// Конструктор класса, заполняет lst
        /// </summary>
        /// <param name="mas">Данные для обработки</param>
        public TUI(List<IJSONObject> mas)
        {
            cur = 0;
            Dictionary<string, int> used = [];

            foreach (IJSONObject i in mas)
            {
                if (i.GetAllFields().Contains("isCategory"))
                {
                    used[GetField(i, "id")] = used.Count;
                    lst.Add(new Group(GetField(i, "id"), GetField(i, "label")));
                }
            }
            
            foreach (IJSONObject i in mas)
            {
                if (!i.GetAllFields().Contains("isCategory") || i.GetField("isCategory") == "false")
                {
                    if (i.GetAllFields().Contains("category") && used.ContainsKey(i.GetField("category")))
                    {
                        lst[used[i.GetField("category")]].Add(GetField(i, "label"));
                    }
                }
            }
        }

        /// <summary>
        /// "Главная" функция класса. Вызывается для отображения данных в консоли.
        /// Отслеживает текущую выбранную группу.
        /// </summary>
        public void TUIMain()
        {
            if (lst.Count == 0)
            {
                Console.WriteLine("Количество записей: 0. Отображать нечего");
                return;
            }
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Black;

            bool need_draw = true;
            while (lst.Count() > 0)
            {
                Console.Clear();
                if (need_draw)
                {
                    Draw();
                }
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.UpArrow)
                {
                    cur = (cur == 0) ? cur : cur - 1;
                    need_draw = true;
                }

                else if (key.Key == ConsoleKey.DownArrow)
                {
                    cur = (cur == lst.Count - 1) ? cur : cur + 1;
                    need_draw = true;
                }

                else if (key.Key == ConsoleKey.Enter)
                {
                    lst[cur].opened_in_tui ^= 1;
                    need_draw = true;
                }

                else if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
                else
                {
                    need_draw = false;
                }
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;

        }

        /// <summary>
        /// Метод для непосредственной "отрисовки" в консоли данных.
        /// Перехвачены исключения
        /// </summary>
        private void Draw()
        {
            try
            {
                int width = 0;
                foreach (Group grp in lst)
                {
                    width = Math.Max(width, grp.Label.Length);
                    foreach (string s in grp.GroupContent)
                    {
                        width = Math.Max(width, s.Length);
                    }
                }

                width += 2;

                for (int i = 0; i < lst.Count; ++i)
                {
                    Group grp = lst[i];
                    char is_cur = i == cur ? '▒' : ' ';

                    Console.WriteLine("┌" + new string('─', width) + "┐");
                    Console.WriteLine("│ " + grp.Label + new string(' ', width - 2 - grp.Label.Length) + " │" + is_cur);
                    Console.WriteLine((grp.opened_in_tui == 1 ? "├" : "└") + new string('─', width) + (grp.opened_in_tui == 1 ? "┤" : "┘") + is_cur);


                    if (grp.opened_in_tui == 1)
                    {

                        for (int j = 0; j < grp.GroupContent.Count; ++j)
                        {
                            string s = grp.GroupContent[j];
                            Console.WriteLine("│ " + s + new string(' ', width - 2 - s.Length) + " │" + is_cur);
                        }
                        Console.WriteLine("└" + new string('─', width) + "┘" + is_cur);
                    }
                    Console.WriteLine(new string(is_cur, width + 3));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }