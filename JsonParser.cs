
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

    /// <summary>
    /// Класс для получения данных из файла
    /// </summary>
    public static class JsonParser
    {
        /// <summary>
        /// Метод для получения данных из файла
        /// </summary>
        /// <param name="lst"> List для записи данных </param>
        /// <param name="type"> Тип вызова (для файла или для готовой к обработке строки)</param>
        /// <param name="s"> Путь/готовая строка (зависит от параметра type) </param>
        /// <param name="correct"> Флаг - гарантия корректности данных </param>
        /// <returns> Результат получения данных - успешно (true) или нет (false)</returns>
        static public bool JsonRead(out List<IJSONObject> lst, int type, string s, bool correct = false)
        {
            lst = [];
            List<IJSONObject> res = [];
            try
            {

                string json;
                if (type == 1)
                {
                    Encoding encoding = EncodingDetector.DetectFileEncoding(s);
                    json = string.Join("", File.ReadAllLines(s, encoding));
                }
                else
                {
                    json = s;
                }

                string abs_pattern = @"^\s*\{\s*""achievements""\s*:\s*\[\s*.*?\]\s*\}\s*$";
                if (!Regex.IsMatch(json, abs_pattern, RegexOptions.Singleline))
                {
                    return false;
                }

                string arr_pattern = @"""achievements""\s*:\s*\[(.*?)\]";
                Match arr_find = Regex.Match(json, arr_pattern, RegexOptions.Singleline);
                if (!arr_find.Success)
                {
                    return false;
                }

                string values = arr_find.Groups[1].Value;

                string record_pattern = @"\{(.*?)\}";
                MatchCollection records = Regex.Matches(values, record_pattern, RegexOptions.Singleline);

                foreach (Match record in records)
                {
                    string text = record.Groups[1].Value;
                    //" \"id\": \"A_ENDING_FOECAUGHTUP\", \"category\": \"A_CATEGORY_EXILE\", \"iconUnlocked\": \"year.stolen\",    \"singleDescription\": true, \"validateOnStorefront\": true,    \"label\": \"Call It A Christmas Present\",    \"descriptionunlocked\": \"I don't think it really qualifies as a cadaver, so much as a souvenir.\"  "                        

                    int cur = 0;
                    bool valid = true;
                    string key_now = "", val_now = "";
                    AchieveUnit node = new();

                    for (int j = 0; j < text.Length; ++j)
                    {
                        char c = text[j];
                        if (cur == 0)
                        {
                            if (c is not ' ' and not '\"')
                            {
                                valid = false;
                                break;
                            }
                            if (c == '\"')
                            {
                                cur = 1;
                            }
                            continue;
                        }
                        if (cur == 1)
                        {
                            if (c == '\"')
                            {
                                cur = 2;
                            }
                            else
                            {
                                key_now += c;
                            }
                            continue;
                        }
                        if (cur == 2)
                        {
                            if (c is not ' ' and not ':')
                            {
                                valid = false;
                                break;
                            }
                            if (c == ':')
                            {
                                cur = 3;
                            }
                            continue;
                        }
                        if (cur == 3)
                        {
                            if (c is not ' ' and not '\"')
                            {
                                if (correct)
                                {
                                    val_now += c;
                                    cur = 4;
                                    continue;
                                }
                                if (j + 4 < text.Length && text[j..(j + 4)] == "true")
                                {
                                    val_now = "true";
                                    node.SetField(key_now, val_now);
                                    key_now = val_now = "";
                                    j += 3;
                                    cur = 5;
                                    continue;
                                }
                                if (j + 5 < text.Length && text[j..(j + 5)] == "false")
                                {
                                    val_now = "false";
                                    node.SetField(key_now, val_now);
                                    key_now = val_now = "";
                                    j += 4;
                                    cur = 5;
                                    continue;
                                }
                                valid = false;
                                break;
                            }
                            if (c == '\"')
                            {
                                cur = 4;
                            }
                            continue;
                        }
                        if (cur == 4)
                        {
                            if (c == '\"')
                            {
                                cur = 5;
                                node.SetField(key_now, val_now);
                                key_now = val_now = "";
                            }
                            else
                            {
                                if (correct && c == ',')
                                {
                                    node.SetField(key_now, val_now);
                                    key_now = val_now = "";
                                    cur = 5;
                                    continue;
                                }
                                val_now += c;
                            }
                            continue;
                        }
                        if (cur == 5)
                        {
                            if (c is not ' ' and not ',')
                            {
                                valid = false;
                                break;
                            }
                            if (c == ',')
                            {
                                cur = 0;
                            }
                            continue;
                        }
                    }

                    if (correct)
                    {
                        node.SetField(key_now, val_now);
                    }
                    if (!correct && (!valid || cur != 5))
                    {
                        return false;
                    }
                    res.Add(node);
                }

                lst = res;
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
        static public bool JsonWrite(List<IJSONObject> lst, string path)
        {
            return WriteData.FileWrite(lst, path);
        }
    }
