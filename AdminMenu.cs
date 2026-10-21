using System;
using System.Collections.Generic;
using System.Text;

namespace Bankapp;

public static class AdminMenu
{
    public static void Show(List<User> allUsers)
    {
        while (true)
        {
            Console.WriteLine("[1] Skapa användare");
            Console.WriteLine("[2] Visa låsta användare ");
            Console.WriteLine("[0] Logga ut");
            Console.Write("Val: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateUser(allUsers);
                    break;

                case "2":
                    UnlockUser(allUsers);
                    break;

                case "0":
                    return;

                default:
                    Ui.Error("Fel val, försök igen.");
                    break;
            }
        }
    }

    static void CreateUser(List<User> allUsers)
    {
        Console.Write("Användarnamn: ");
        string username = Console.ReadLine() ?? "";

        if (allUsers.Any(user => user.Username == username))
        {
            Ui.Error("Användarnamnet är upptaget.");
            return;
        }

        Console.Write("Lösenord: ");
        string password = Console.ReadLine() ?? "";

        Console.WriteLine("[1] Kund");
        Console.WriteLine("[2] Admin");
        Console.Write("Välj roll: ");
        string? roleChoice = Console.ReadLine();

        UserRole role;

        if (roleChoice == "1")
        {
            role = UserRole.Customer;
        }
        else if (roleChoice == "2")
        {
            role = UserRole.Admin;
        }
        else
        {
            Ui.Error("Ogiltigt rollval.");
            return;
        }

        User newUser = new User
        {
            Username = username,
            Password = password,
            Role = role
        };

        allUsers.Add(newUser);
        UserStore.Save(allUsers);

        Ui.Success("Användaren har skapats.");
    }
    
    }
}
