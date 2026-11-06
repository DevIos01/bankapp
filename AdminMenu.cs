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
            Ui.Option("3", "Ändra växelkurser");
            Ui.Option("4", "Statistik för användare");
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

                case "3":
                    ChangeExchangeRates();
                    break;

                case "4":
                    UserStats.Show(allUsers);
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

        // customers get a personkonto, same as when they sign up themselves
        if (role == UserRole.Customer)
        {
            Account personkonto = new Account
            {
                AccountName = "Personkonto",
                AccountNumber = new Account().CreateAccountNumber(),
                UserID = newUser.UserID,
                Currency = CurrencyType.Sek
            };

            newUser.Accounts.Add(personkonto);
        }

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

    static void ChangeExchangeRates()
    {
        Ui.Header("VÄXELKURSER");
        Console.WriteLine($"  1 USD = {ExchangeRateManager.UsdToSek:N2} SEK");
        Console.WriteLine($"  1 EUR = {ExchangeRateManager.EurToSek:N2} SEK");
        Console.WriteLine();

        Ui.Option("1", "Ändra USD");
        Ui.Option("2", "Ändra EUR");
        Ui.Option("0", "Tillbaka");
        Console.WriteLine();

        int selection = Ui.AskNumber("  Val: ", 0, 2);

        if (selection == 0)
        {
            return;
        }

        CurrencyType currency;

        if (selection == 1)
        {
            currency = CurrencyType.Usd;
        }
        else
        {
            currency = CurrencyType.Eur;
        }

        while (true)
        {
            string input = Ui.Ask($"  Ny kurs, 1 {currency.ToString().ToUpper()} i SEK (0 = avbryt): ");

            if (input == "0")
            {
                return;
            }

            if (decimal.TryParse(input, out decimal rate) &&
                ExchangeRateManager.SetRate(currency, rate))
            {
                Console.WriteLine();
                Ui.Success($"Ny kurs: 1 {currency.ToString().ToUpper()} = {rate:N2} SEK");
                Ui.Pause();
                return;
            }

            Ui.Error("Ange en kurs större än 0, eller 0 för att avbryta.");
        }
    } //change USD and EUR rates
}
