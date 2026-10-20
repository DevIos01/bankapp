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
}
