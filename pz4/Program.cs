namespace pz4;

class Program
{
    public static DataBase db = new DataBase();

    public static void WorkerThreadMethod1()
    {
        Console.WriteLine("Поток 1 стартовал");
        db.SaveData("Данные из потока 1");
        Console.WriteLine("Поток 1 завершён");
    }

    public static void WorkerThreadMethod2()
    {
        Console.WriteLine("Поток 2 стартовал");
        db.SaveData("Данные из потока 2");
        Console.WriteLine("Поток 2 завершён");
    }

    static void Main(string[] args)
    {
        ThreadStart worker1 = new ThreadStart(WorkerThreadMethod1);
        ThreadStart worker2 = new ThreadStart(WorkerThreadMethod2);

        Thread t1 = new Thread(worker1);
        Thread t2 = new Thread(worker2);

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        Console.WriteLine("Все потоки завершены.");
    }
}