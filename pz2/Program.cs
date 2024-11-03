using System.Runtime.InteropServices;
using System.Text;

namespace pz2;

class Program
{
    [DllImport("user32.dll")]
    public static extern int GetSystemMetrics(int nIndex);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    private static extern int GetComputerName(StringBuilder nameBuffer, ref int bufferSize);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern uint GetVersion();

    static void Main(string[] args)
    {
        StringBuilder nameBuffer = new StringBuilder(256); // Буфер для имени компьютера
        int bufferSize = nameBuffer.Capacity; // Размер буфера

        GetComputerName(nameBuffer, ref bufferSize);
        Console.WriteLine("Имя компьютера: " + nameBuffer.ToString());

        uint version = GetVersion();
        uint majorVersion = Convert.ToUInt32(version & 0x000000FF);
        uint minorVersion = Convert.ToUInt32((version & 0x0000FF00) >> 8);
        Console.WriteLine("Версия ОС: " + majorVersion + "." + minorVersion);

        uint build = 0;
        if (version < 0x80000000) // Если версия ОС больше, чем Windows NT
        {
            build = Convert.ToUInt32((version & 0xFFFF0000) >> 16);
            Console.WriteLine("Сборка ОС: " + build);
        }
    }
}