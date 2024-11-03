namespace pz5;

class Program
{
    // Используем флаг для аварийного завершения потока
    private static bool criticalErrorOccurred = false;
    private static readonly object lockObject = new object();

    // Переменные для передачи данных между потоками
    private static double progressionValue = 1;
    private static bool dataAvailable = false;

    // Поток для вывода данных на консоль
    static void Thread1()
    {
        while (!criticalErrorOccurred)
        {
            lock (lockObject)
            {
                if (dataAvailable)
                {
                    Console.WriteLine($"Thread1: Получено значение {progressionValue}");
                    dataAvailable = false;
                }
            }

            Thread.Sleep(100);
        }

        Console.WriteLine("Thread1 завершён.");
    }

    // Поток для генерации геометрической прогрессии
    static void Thread2()
    {
        double ratio = 2; // Коэффициент прогрессии
        progressionValue = 1;

        while (!criticalErrorOccurred)
        {
            lock (lockObject)
            {
                progressionValue *= ratio;
                dataAvailable = true;
                Console.WriteLine($"Thread2: Сгенерировано значение {progressionValue}");
            }

            // Проверка критического значения
            if (progressionValue >= 1000)
            {
                Console.WriteLine("Thread2: Достигнуто критическое значение! Аварийное завершение.");
                criticalErrorOccurred = true;
                break;
            }

            // Блокируем поток на некоторое время, если значение достигло определённого порога
            if (progressionValue >= 128)
            {
                Console.WriteLine("Thread2: Значение достигло порога, поток временно заблокирован.");
                Thread.Sleep(2000); // Время блокировки, например, 2 секунды
                Console.WriteLine("Thread2: Возобновление работы после блокировки.");
            }

            Thread.Sleep(500); // Пауза перед следующим значением
        }

        Console.WriteLine("Thread2 завершён.");
    }

    static void Main(string[] args)
    {
        Thread t1 = new Thread(Thread1);
        Thread t2 = new Thread(Thread2);

        t1.Start();
        t2.Start();

        // Ожидаем завершения потоков
        t1.Join();
        t2.Join();

        Console.WriteLine("Работа программы завершена.");
    }
}