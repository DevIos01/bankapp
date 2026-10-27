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
    } // admin menu

    static void CreateUser(List<User> allUsers)
    {
        string username = Ui.Ask("Användarnamn: ");
        if (allUsers.Any(user => user.Username == username))
        {
            Ui.Error("Användarnamnet är upptaget.");
            return;
        }

        Console.Write(" ");
        string password = Ui.Ask("Lösenord: ");

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
    } //create user with user-type
    static void UnlockUser(List<User> allUsers)
    {
        List<User> lockedUsers =
            allUsers.Where(user => user.IsLocked).ToList();

        if (lockedUsers.Count == 0)
        {
            Console.WriteLine("Det finns inga låsta användare.");
            return;
        }

        for (int i = 0; i < lockedUsers.Count; i++)
        {
            Console.WriteLine($"[{i + 1}] {lockedUsers[i].Username}");
        }

        Console.Write("Välj användare : ");
        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int selection) ||
            selection < 1 ||
            selection > lockedUsers.Count)
        {
            Ui.Error("Ogiltigt val.");
            return;
        }

        User selectedUser = lockedUsers[selection - 1];
        selectedUser.FailedLoginAttempts = 0;

        UserStore.Save(allUsers);
        Ui.Success($"{selectedUser.Username} har låsts upp.");
    } //unlock locked users 
}
