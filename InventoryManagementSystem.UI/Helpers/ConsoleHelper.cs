namespace InventoryManagementSystem.UI.Helpers;

public class ConsoleHelper
{
    public static void DrawHeader(string title)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"""
                            ╔══════════════════════════════════════╗
                            ║ {title.PadRight(36)} ║
                            ╚══════════════════════════════════════╝
                            """);
        Console.ResetColor();
        Console.WriteLine();
    }

    public static void Success(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"✔ {message}");
        Console.ResetColor();
    }

    public static void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"✖ {message}");
        Console.ResetColor();
    }

    public static void Warning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"⚠ {message}");
        Console.ResetColor();
    }

    public static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press any key to continue...");
        Console.ReadLine();
    }

    public static decimal ReadDecimal(string message)
    {
        decimal value;
        do
        {
            Console.Write(message);
        } while (!decimal.TryParse(Console.ReadLine(), out value));
        return  value;;
    }

    public static int ReadInt(string message)
    {
        int value;
        do
        {
            Console.Write(message);
        } while (!int.TryParse(Console.ReadLine(), out value));
        return value;
    }
}