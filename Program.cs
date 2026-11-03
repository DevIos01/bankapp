using System;
namespace Bankapp;

internal class Program
{
    static void EnsureDefaultAdmin(List<User> allUsers)
    {
        // Creates default admin user
        if (allUsers.Any(user => user.Username == "admin"))
        {
            return;
        }

        User admin = new User
        {
            Username = "admin",
            Password = "admin",
            Role = UserRole.Admin
        };

        allUsers.Add(admin);
        UserStore.Save(allUsers);
    }        // Creates default admin user

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        List<User> allUsers = UserStore.Load();
        EnsureDefaultAdmin(allUsers);
        StartMenu(allUsers);
    }

    static void StartMenu(List<User> allUsers)
    {
        while (true)
        {
            // clear and draw the logo every time, so it's back after login/logout
            Ui.Clear();
            Ui.Logo();
            Console.WriteLine();
            Ui.Option("1", "Logga in");
            Ui.Option("2", "Skapa konto");
            Ui.Option("0", "Avsluta");
            string choice = Ui.Choice();

            switch (choice)
            {
                case "1":
                    User? loggedInUser = new User().Login(allUsers);

                    if (loggedInUser != null &&
                        loggedInUser.Role == UserRole.Admin)
                    {
                        AdminMenu.Show(allUsers);
                    }
                    else if (loggedInUser != null)
                    {
                        UserMenu.MainMenu(loggedInUser, allUsers);
                    }

                    break;

                case "2":
                    new User().CreateAccount(allUsers);
                    break;
                case "0":
                    return;
                default:
                    Ui.Error("Fel val, försök igen.");
                    Ui.Pause();
                    break;
            }
        }
    }
}
