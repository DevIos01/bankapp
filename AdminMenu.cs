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
            Ui.Header("ADMIN");
            Ui.Option("1", "Skapa användare");
            Ui.Option("2", "Lås upp användare");
            Ui.Option("0", "Logga ut");
            string choice = Ui.Choice();

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
                    Ui.Pause();
                    break;
            }
        }
    } // admin menu

    static void CreateUser(List<User> allUsers)
    {
        Ui.Header("SKAPA ANVÄNDARE");
        string username = Ui.Ask("  Användarnamn: ");
        if (allUsers.Any(user => user.Username == username))
        {
            Ui.Error("Användarnamnet är upptaget.");
            Ui.Pause();
            return;
        }

        string password = Ui.AskPassword("  Lösenord:     ");

        Console.WriteLine();
        Ui.Option("1", "Kund");
        Ui.Option("2", "Admin");
        int roleChoice = Ui.AskNumber("  Roll: ", 1, 2);

        UserRole role;

        if (roleChoice == 1)
        {
            role = UserRole.Customer;
        }
        else
        {
            role = UserRole.Admin;
        }

        User newUser = new User
        {
            Username = username,
            Password = password,
            Role = role
        };

        allUsers.Add(newUser);
        UserStore.Save(allUsers);

        Console.WriteLine();
        Ui.Success("Användaren har skapats.");
        Ui.Pause();
    } //create user with user-type
    static void UnlockUser(List<User> allUsers)
    {
        Ui.Header("LÅS UPP ANVÄNDARE");
        List<User> lockedUsers =
            allUsers.Where(user => user.IsLocked).ToList();

        if (lockedUsers.Count == 0)
        {
            Console.WriteLine("  Det finns inga låsta användare.");
            Ui.Pause();
            return;
        }

        for (int i = 0; i < lockedUsers.Count; i++)
        {
            Ui.Option($"{i + 1}", lockedUsers[i].Username);
        }
        Ui.Option("0", "Tillbaka");
        Console.WriteLine();

        int selection = Ui.AskNumber("  Val: ", 0, lockedUsers.Count);

        if (selection == 0)
        {
            return;
        }

        User selectedUser = lockedUsers[selection - 1];
        selectedUser.FailedLoginAttempts = 0;

        UserStore.Save(allUsers);
        Console.WriteLine();
        Ui.Success($"{selectedUser.Username} har låsts upp.");
        Ui.Pause();
    } //unlock locked users
}
