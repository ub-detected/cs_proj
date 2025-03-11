# 1. Скачайте образ с Docker Hub.
### https://hub.docker.com/repository/docker/ubdetected/cs_proj/general
### Перед Вами - консольное приложение написанное на языке C#, которое работает с .json файлами и обрабатывает данные в них. 
### Консольное приложение является проектом, сделанным в рамках курса программы "Программная инженерия" НИУ ВШЭ. В рамках проекта было разработано консольное приложение для работы с данными о достижениях в игре Cultist Simulator. 
### Эти данные представлены в формате JSON. Пример такого .json файла - achievements.json # (лежит в данном репозитории).
# 2. Запуск.
### Запустите контейнер введя команду "docker run -it --rm --memory="100m" --cpus="1" -v <Путь до папки с вашим .json файлом>.json:/app/data.json cs-proj".

# 3. Взаимодействие с приложением(после запуска программы):
### Вам будет выведено меню с командами.
### Файл .json который был указан ранее при запуске контейнера (пункт 2) лежит по пути "/app/data.json" (можно его использовать для ввода данных через файл).
### Для выхода из интерфейса (пункт меню 4) нажмите клавишу "Esc".
