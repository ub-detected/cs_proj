
    /// <summary>
    /// Класс для получения данных со страницы игры (доп задача)
    /// </summary>
    public static class GetGameData
    {
        /// <summary>
        /// Метод для получения данных со страницы
        /// Перехвачены исключения и выведены их описания
        /// </summary>
        /// <param name="res"> Полученные данные</param>
        /// <returns> Результат получения данных - успешно (true) или нет (false) </returns>
        private static bool Process(out List<IJSONObject> res)
        {
            res = [];
            string url = "http://api.steampowered.com/ISteamUserStats/GetGlobalAchievementPercentagesForApp/v0002/?gameid=718670&format=json";
            
            try
            {
                HttpClient client = new();
                HttpResponseMessage response = client.GetAsync(url).Result;
                _ = response.EnsureSuccessStatusCode();

                string json = response.Content.ReadAsStringAsync().Result;

                json = json[26..^1];

                return JsonParser.JsonRead(out res, 0, json, true);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Метод для вывода на консоль данных (процент игроков получивших достижение (для каждого))
        /// Данные получены со страницы игры (доп задача)
        /// Вызывает внутри метод GetGameData.Process
        /// </summary>
        public static void WriteGameData()
        {
            if (!Process(out List<IJSONObject> res)) {
                Console.WriteLine("Не удалось получить данные.");
                return;
            }

            foreach (IJSONObject i in res)
            {
                Console.WriteLine($"Достижение - {i.GetField("name")}, процент получивших игроков - {i.GetField("percent")}.");
            }
        }
    }
