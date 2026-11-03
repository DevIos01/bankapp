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
        // the box grows if a title is longer than 30 characters
        int width = Math.Max(30, Math.Max(title.Length, step.Length) + 4);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔" + new string('═', width) + "╗");
        Console.WriteLine("║" + Center(title, width) + "║");
        if (step != "")
            Console.WriteLine("║" + Center(step, width) + "║");
        Console.WriteLine("╚" + new string('═', width) + "╝");
        Console.ResetColor();
        Console.WriteLine();
    }

    // one menu row, e.g. Ui.Option("1", "Logga in") -> "  [1] Logga in"
    public static void Option(string key, string text)
    {
        Console.WriteLine($"  [{key}] {text}");
    }

    // asks for the menu choice; use the answer in a switch ("1", "2", "0" ...)
    public static string Choice()
    {
        Console.WriteLine();
        Console.Write("  Val: ");
        return Console.ReadLine()?.Trim() ?? "";
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

    public static string AskPassword(string question)
    {
        // ReadKey doesn't work when input comes from a file or pipe
        if (Console.IsInputRedirected)
            return Ask(question);

        while (true)
        {
            Console.Write(question);
            string password = "";

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter)
                    break;

                if (key.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password[..^1];
                        Console.Write("\b \b");
                    }
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    password += key.KeyChar;
                    Console.Write('*');
                }
            }

            Console.WriteLine();

            // trimmed like Ask, so passwords saved earlier still match
            if (!string.IsNullOrWhiteSpace(password))
                return password.Trim();

            Error("  Får inte vara tomt.");
        }
    }

    public static int AskNumber(string question, int min, int max)
    {
        while (true)
        {
            string answer = Ask(question);

            if (int.TryParse(answer, out int number) && number >= min && number <= max)
                return number;

            Error($"  Välj ett tal mellan {min} och {max}.");
        }
    }

    public static string AskEmail(string question)
    {
        while (true)
        {
            string email = Ask(question);

            if (email.Contains('@'))
                return email;

            Error("  E-postadressen måste innehålla @.");
        }
    }

    public static void Pause()
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("  Tryck Enter för att fortsätta...");
        Console.ResetColor();
        Console.ReadLine();
    }

    // messages always get the same indent, with or without "  " in the text
    public static void Error(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("  " + text.TrimStart());
        Console.ResetColor();
    }

    public static void Success(string text)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("  " + text.TrimStart());
        Console.ResetColor();
    }

    static string Center(string text, int width)
    {
        int left = (width - text.Length) / 2;
        return text.PadLeft(left + text.Length).PadRight(width);
    }
}
