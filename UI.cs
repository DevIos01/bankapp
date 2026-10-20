namespace Bankapp;

public static class Ui
{
    public static void Logo()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(
            """
             ____              _        _
            | __ )  __ _ _ __ | | __   / \   _ __  _ __
            |  _ \ / _` | '_ \| |/ /  / _ \ | '_ \| '_ \
            | |_) | (_| | | | |   <  / ___ \| |_) | |_) |
            |____/ \__,_|_| |_|_|\_\/_/   \_\ .__/| .__/
                                            |_|   |_|
            """
        );
        Console.ResetColor();
    }

    public static void Clear()
    {
        if (!Console.IsOutputRedirected)
            Console.Clear();
    }

    public static void Header(string title, string step = "")
    {
        Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔" + new string('═', 30) + "╗");
        Console.WriteLine("║" + Center(title) + "║");
        if (step != "")
            Console.WriteLine("║" + Center(step) + "║");
        Console.WriteLine("╚" + new string('═', 30) + "╝");
        Console.ResetColor();
        Console.WriteLine();
    }

    public static string Ask(string question)
    {
        while (true)
        {
            Console.Write(question);
            string? answer = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(answer))
                return answer.Trim();

            Error("  Får inte vara tomt.");
        }
    }

    public static void Pause()
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("Tryck Enter för att fortsätta...");
        Console.ResetColor();
        Console.ReadLine();
    }

    public static void Error(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static void Success(string text)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    static string Center(string text)
    {
        int left = (30 - text.Length) / 2;
        return text.PadLeft(left + text.Length).PadRight(30);
    }
}
