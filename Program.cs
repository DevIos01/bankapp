namespace Bankapp;

internal class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        List<User> allUsers = new List<User>();
        StartMenu(allUsers);
    }

    static void StartMenu(List<User> allUsers)
    {
        Ui.Logo();

        while (true)
        {
            Console.WriteLine("──────────────────────────────");
            Console.WriteLine("  [1] Logga in");
            Console.WriteLine("  [2] Skapa konto");
            Console.WriteLine("  [0] Avsluta");
            Console.WriteLine("──────────────────────────────");
            Console.Write("Val: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    // Login(allUsers);
                    break;
                case "2":
                    // CreateAccount(allUsers);
                    break;
                case "0":
                    return;
                default:
                    Ui.Error("Fel val, försök igen.");
                    break;
            }
        }
    }
}
