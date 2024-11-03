using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace pz3;

class Program
{
    [DllImport("kernel32.dll")]
    private static extern IntPtr GetCurrentProcess();

    [DllImport("kernel32.dll")]
    private static extern uint GetCurrentProcessId();

    [DllImport("kernel32.dll")]
    private static extern bool DuplicateHandle(
        Handle hSourceProcessHandle,
        Handle hSourceHandle,
        Handle hTargetProcessHandle,
        out IntPtr lpTargetHandle,
        uint dwDesiredAccess,
        bool bInheritHandle,
        uint dwOptions);

    [DllImport("kernel32.dll")]
    static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

    [DllImport("kernel32.dll")]
    private static extern bool CloseHandle(IntPtr hObject);

    const uint PROCESS_QUERY_INFORMATION = 0x0400;
    const uint PROCESS_VM_READ = 0x0010;

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine(
                "1: Вывести все\n" +
                "2: По имени\n" +
                "3: По полному имени\n" +
                "4: По дескриптору\n" +
                "5: Информация о процессе\n" +
                "6: Информация о процессах, потоках, модулях\n" +
                "0: Выход\n"
            );

            Console.Write("Выберите действие: ");
            if (!int.TryParse(Console.ReadLine(), out int pressedKey))
            {
                Console.WriteLine("Некорректный ввод. Пожалуйста, введите число.");
                continue;
            }

            switch (pressedKey)
            {
                case 1:
                    Init(out string shortFileName, out string longFileName, out IntPtr processHandle);
                    Console.WriteLine(
                        $"Имя: {shortFileName}\n" +
                        $"Полное имя: {longFileName}\n" +
                        $"Дескриптор: {processHandle}\n"
                    );
                    break;
                case 2:
                    ShowByName();
                    break;
                case 3:
                    ShowByFullName();
                    break;
                case 4:
                    ShowByHandle();
                    break;
                case 5:
                    ShowProcessInfo();
                    break;
                case 6:
                    ShowProcesses();
                    break;
                case 0:
                    Console.WriteLine("Выход из программы.");
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    static void Init(out string shortFileName, out string longFileName, out IntPtr processHandle)
    {
        Process currentProcess = Process.GetCurrentProcess();

        shortFileName = currentProcess.ProcessName;
        longFileName = currentProcess.MainModule.FileName;
        processHandle = GetCurrentProcess();
    }

    static void ShowByName()
    {
        Console.Write("Введите имя процесса: ");
        string processName = Console.ReadLine();
        var processes = Process.GetProcessesByName(processName);
        ShowProcessDetails(processes);
    }

    static void ShowByFullName()
    {
        Console.Write("Введите полное имя процесса (с расширением): ");         
        string fullName = Console.ReadLine();         
        var processes = Process.GetProcesses().Where(p => p.ProcessName.Equals(fullName.Split('.')[0], 
            StringComparison.OrdinalIgnoreCase)).ToArray();        
        ShowProcessDetails(processes); 
    }

    static void ShowByHandle()
    {
        Init(out string shortFileName, out string longFileName, out IntPtr processHandle);        
        Console.Write("Введите дескриптор процесса: ");
        if (uint.TryParse(Console.ReadLine(), out uint handleValue))
        {
            try
            {
                Process proc = Process.GetProcessById((int)processHandle);
                Console.WriteLine($"Имя процесса: {shortFileName}");
                Console.WriteLine($"Полное имя: {longFileName}");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Процесс с указанным ID не найден.");
            }
        }
        else
        {
            Console.WriteLine("Некорректный дескриптор.");
        } 
    }

    private static void ShowProcessInfo()
    {
        Init(out string shortFileName, out string longFileName, out IntPtr processHandle);         
        uint processId = GetCurrentProcessId();         
        Process proc = Process.GetProcessById((int)processId);       
        
        Console.WriteLine($"Имя процесса: {shortFileName}");         
        Console.WriteLine($"Полное имя: {longFileName}");         
        Console.WriteLine($"ID: {proc.Id}");         
        Console.WriteLine($"Память: {proc.WorkingSet64 / 1024} KB");
    }

    /*
     * Информация о процессах, потоках, модулях
     */
    private static void ShowProcesses()
    {
        Console.WriteLine("Список всех процессов:");
        foreach (var process in Process.GetProcesses())
        {
            Console.WriteLine($"ID: {process.Id}\n" +                 
                              $" Имя: {process.ProcessName}");
        }
    } 

    private static void ShowProcessDetails(Process[] processes)
    {
        Init(out string shortFileName, out string longFileName, out IntPtr processHandle);

        if (processes.Length == 0)
        {
            Console.WriteLine("Процессы не найдены.");
            return;
        }

        foreach (var proc in processes)
        {
            Console.WriteLine($"ID: {proc.Id}\n" +
                              $"Имя: {shortFileName}\n" +
                              $"Полное имя: {longFileName}");
        }
    }
}