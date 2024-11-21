using System.Runtime.InteropServices;

namespace pz8;

class Program
{
    [DllImport("kernel32.dll")]
    static extern IntPtr HeapCreate(uint flOptions, uint dwInitialSize, uint dwMaximumSize);
    
    static void Main(string[] args)
    {
        Thread thread1 = new Thread(EndlessCycle);
        Thread thread2 = new Thread(EndlessCycle);
        Thread thread3 = new Thread(EndlessCycle);
        
        thread1.Start();
        thread2.Start();
        thread3.Start();
        
        thread1.Join();
        thread2.Join();
        thread3.Join();
    }

    static void EndlessCycle()
    {
        while (true)
        {
            Console.WriteLine($"Потоки запущены {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}