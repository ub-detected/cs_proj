using System;
using System.IO;
using System.Text;


// AI_CODE_1
/// <summary>
/// Класс для получения кодировки файла
/// </summary>
public static class EncodingDetector
{
    /// <summary>
    /// Метод для определения кодировки файла
    /// Перехвачены исключения
    /// </summary>
    /// <param name="filePath">Путь к файлу</param>
    /// <returns>Кодировка</returns>
    public static Encoding DetectFileEncoding(string filePath)
    {
        try
        {
            byte[] bytes = File.ReadAllBytes(filePath);

            return bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF
                ? Encoding.UTF8
                : bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE
                    ? Encoding.UTF8
                    : bytes.Length >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF
                                ? Encoding.BigEndianUnicode
                                : IsUtf8(bytes) ? Encoding.UTF8 : Encoding.Default;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Encoding.Default;
        }
    }

    /// <summary>
    /// Проверка является кодировка файла UTF-8
    /// </summary>
    /// <param name="bytes">Содержимое файла</param>
    /// <returns>Результат (true/false) </returns>
    private static bool IsUtf8(byte[] bytes)
    {

        for (int i = 0; i < bytes.Length; i++)
        {
            if ((bytes[i] & 0x80) != 0)
            {
                if (i + 1 >= bytes.Length)
                {
                    return false;
                }

                byte b = bytes[i];
                if ((b & 0xC0) != 0x80)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
