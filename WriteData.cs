using System.Security;
using System.Text;

    /// <summary>
    /// Класс для записи данных в файл/консоль.
    /// </summary>
    public static class WriteData
    {

        /// <summary>
        /// Метод для записи данных в консоль
        /// </summary>
        /// <param name="lst"> Данные для записи </param>
        public static void ConsoleWrite(List<IJSONObject> lst)
        {
            string empty_string = "    ";
            Console.WriteLine("{\n  \"achievements\": [\r\n");

            for (int i = 0; i < lst.Count; ++i)
            {
                Console.WriteLine(empty_string + "{");
                foreach (string j in lst[i].GetAllFields())
                {
                    Console.Write(empty_string + empty_string + "\"" + j + "\": ");
                    string plus = j == "isCategory" ? "" : "\"";
                    Console.WriteLine(empty_string + empty_string + plus + lst[i].GetField(j) + plus);
                }
                Console.WriteLine(empty_string + "}" + (i != lst.Count - 1 ? "," : ""));
            }

            Console.WriteLine("   ]");
            Console.WriteLine("}");
        }

        /// <summary>
        /// Метод для записи данных в файл.
        /// Перехватывает все необходимые исключения:
        /// FileNotFoundException,
        /// SecurityException,
        /// DirectoryNotFoundException,
        /// IOException,
        /// UnauthorizedAccessException,
        /// И другие исключения.
        /// При перехвате метод выводит подробное описание исключений.
        /// </summary>
        /// <param name="lst"> Данные для записи </param>
        /// <param name="path"> Путь к файлу для записи </param>
        /// <returns> Результат записи - успешно (true) или нет (false) </returns>
        public static bool FileWrite(List<IJSONObject> lst, string path)
        {
            try
            {
                FileStream fs;

                if (!File.Exists(path))
                {
                    fs = new(path, FileMode.CreateNew);
                }
                else
                {
                    File.WriteAllText(path, "");
                    fs = new(path, FileMode.Open);
                }
                using StreamWriter sw = new(fs, Encoding.Default);

                string empty_string = "    ";
                sw.WriteLine("{\n  \"achievements\": [\r\n");

                for (int i = 0; i < lst.Count; ++i)
                {
                    sw.WriteLine(empty_string + "{");
                    int cnt_enum = lst[i].GetAllFields().Count();

                    foreach (string j in lst[i].GetAllFields())
                    {
                        --cnt_enum;
                        sw.Write(empty_string + empty_string + "\"" + j + "\": ");
                        string plus = j == "isCategory" ? "" : "\"";
                        sw.WriteLine(empty_string + empty_string + plus + lst[i].GetField(j) + plus + (cnt_enum == 0 ? "" : ","));
                    }
                    sw.WriteLine(empty_string + "}" + (i != lst.Count - 1 ? "," : ""));
                }

                sw.WriteLine("   ]");
                sw.WriteLine("}");

                return true;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл по указанному пути не найден.");
                return false;
            }
            catch (SecurityException)
            {
                Console.WriteLine("Нет доступа к файлу.");
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Ошибка в указанном пути.");
                return false;
            }
            catch (IOException)
            {
                Console.WriteLine("Ошибка ввода/вывода.");
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Произошла ошибка при получении доступа к файлу.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
    }
