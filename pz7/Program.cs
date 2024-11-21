using System.Runtime.InteropServices;

namespace pz7;

class Program
{
    static void Main(string[] args)
    {
        MEMORYSTATUSEX mem = new MEMORYSTATUSEX();
        mem.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));

        if (_GlobalMemoryStatusEx(ref mem))
        {
            Console.WriteLine($"Объем физической памяти: {mem.ullTotalPhys} байт");
            Console.WriteLine($"Объем доступной физической памяти: {mem.ullAvailPhys} байт");
            Console.WriteLine($"Объем файла подкачки: {mem.ullTotalPageFile} байт");
            Console.WriteLine($"Объем доступного файла подкачки: {mem.ullAvailPageFile} байт");
            Console.WriteLine($"Объем виртуальной памяти: {mem.ullTotalVirtual} байт");
            Console.WriteLine($"Объем доступной виртуальной памяти: {mem.ullAvailVirtual} байт");
            Console.WriteLine($"Загрузка памяти: {mem.dwMemoryLoad} %");
        }
        else
        {
            Console.WriteLine("Ошибка при получение информации о памяти");
        }

        Console.ReadKey();
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
    }
    
    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "GlobalMemoryStatusEx", 
        SetLastError = true)]
    static extern bool _GlobalMemoryStatusEx(ref MEMORYSTATUSEX memEx);
}