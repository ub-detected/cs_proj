
/// <summary>
/// Основной класс программы
/// </summary>
public class Program {
    /// <summary>
    /// Главная функция программы
    /// </summary>
    private static void Main()
    {

        List<string> q = ["1.1", "1.2", "2", "3", "4", "5", "6.1", "6.2", "7"];
        List<IJSONObject> lst = [];

        while (true)
        {
            Info.Menu();
            string? s = Console.ReadLine();
            if (s == null || q.Contains(s) == false)
            {
                Console.WriteLine("Некорректный ввод.");
                continue;
            }

            Console.WriteLine(Info.del);
            if (s == "1.1")
            {
                string? path = Console.ReadLine();
                if (path == null)
                {
                    Console.WriteLine("Некорректный ввод.");
                    continue;
                }

                bool is_valid = JsonParser.JsonRead(out lst, 1, path);

                if (!is_valid)
                {
                    Console.WriteLine("Невалидный json-файл.");
                    continue;
                }

                Console.WriteLine("Файл успешно считан.");
            }

            if (s == "1.2")
            {
                Console.WriteLine("Введите данные в консоль. По окончании ввода нажмите сочетание клавиш Ctrl+Z.");
                Console.WriteLine(Info.del);

                string sum_inp = "";

                while (true)
                {
                    string? input = Console.ReadLine();
                    if (input == null)
                    {
                        break;
                    }
                    sum_inp += input;
                }

                Console.WriteLine(Info.del);

                bool is_valid = JsonParser.JsonRead(out lst, 0, sum_inp);
                if (!is_valid)
                {
                    Console.WriteLine("Были введены невалидные данные.");
                    continue;
                }

                Console.WriteLine("Данные успешно считаны.");
            }

            if (s == "2")
            {
                lst = JsonProcess.Filter(lst);
            }

            if (s == "3")
            {
                lst = JsonProcess.Sort(lst);
            }

            if (s == "4")
            {
                TUI t = new(lst);
                t.TUIMain();
            }
            
            if (s == "5")
            {
                GetGameData.WriteGameData();
            }

            if (s == "6.1") 
            {
                string? path = Console.ReadLine();
                if (path == null)
                {
                    Console.WriteLine("Некорректный ввод.");
                    continue;
                }

                if (!JsonParser.JsonWrite(lst, path))
                {
                    continue;
                }
            }

            if (s == "6.2")
            {
                WriteData.ConsoleWrite(lst);
            }

            if (s == "7")
            {
                return;
            }

        }
    }
}