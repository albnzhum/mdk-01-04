namespace pz4;

public class DataBase
{
    public void SaveData(string text)
    {
        // Блокировка метода, чтобы он был доступен только одному потоку в данный момент
        lock (this)
        {
            Console.WriteLine($"Начинается сохранение данных: {text}");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Сохранение данных {text}... шаг {i + 1}");
                Thread.Sleep(500);
            }
            Console.WriteLine($"Завершено сохранение данных: {text}");
        }
    }
}