using System;
using System.Collections.Generic;
using System.Text;

namespace Bankapp
{
    public static class UserMenu
    {
        public static void MainMenu(User loggedInUser, List<User> allUser)
        {
            while (true)
            {
                Ui.Header("MINA SIDOR", $"Inloggad som {loggedInUser.Username}");
                Ui.Option("1", "Bankkonton");
                Ui.Option("2", "Ändra personuppgifter");
                Ui.Option("3", "Lån");
                Ui.Option("0", "Logga ut");
                string choice = Ui.Choice();

                switch (choice)
                {
                    case "1":
                        AccountMenu(loggedInUser, allUser);
                        break;

                    case "2":
                        EditKyc(loggedInUser, allUser);
                        break;

                    case "3":
                        LoanMenu(loggedInUser, allUser);
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
        public static void LoanMenu(User loggedInUser, List<User> allUser)
        {
            while (true)
            {
                Ui.Header("LÅN");
                Ui.Option("1", "Se dina lån");
                Ui.Option("2", "Ansök om lån");
                Ui.Option("0", "Tillbaka");
                string choice = Ui.Choice();

                switch (choice)
                {
                    case "1":
                        // ShowLoans(loggedInUser);
                        Console.WriteLine("  Kommer snart.");
                        Ui.Pause();
                        break;

                    case "2":
                        // RequestLoan(loggedInUser, allUser, LoanRules.DefaultRate);
                        Console.WriteLine("  Kommer snart.");
                        Ui.Pause();
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
        public static void AccountMenu(User loggedInUser, List<User> allUser)
        {
            while (true)
            {
                Ui.Header("BANKKONTON");
                Ui.Option("1", "Se bankkonton");
                Ui.Option("2", "Skapa bankkonto");
                Ui.Option("3", "Skapa sparkonto");
                Ui.Option("4", "Överföringar");
                Ui.Option("0", "Tillbaka");
                string choice = Ui.Choice();

                switch (choice)
                {
                    case "1":
                        Ui.Header("MINA KONTON");
                        loggedInUser.DisplayAccountInfo();
                        Ui.Pause();
                        break;

                    case "2":
                        new Account().OpenNewAccount(loggedInUser);
                        UserStore.Save(allUser);
                        break;

                    case "3":
                        new Account().OpenSavingsAccount(loggedInUser);
                        UserStore.Save(allUser);
                        break;

                    case "4":
                        // transfers come with BANK-14 / BANK-15
                        Console.WriteLine("  Kommer snart.");
                        Ui.Pause();
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
        public static void EditKyc(User loggedInUser, List<User> allUser)
        {
            while (true)
            {
                Ui.Header("PERSONUPPGIFTER");
                Ui.Option("1", "Ändra namn");
                Ui.Option("2", "Ändra e-post");
                Ui.Option("3", "Ändra mobilnummer");
                Ui.Option("0", "Tillbaka");
                string choice = Ui.Choice();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine();
                        Console.WriteLine($"  Nuvarande: {loggedInUser.FullName}");
                        loggedInUser.FullName = Ui.Ask("  Nytt namn: ");
                        UserStore.Save(allUser);
                        Ui.Success("Sparat!");
                        Ui.Pause();
                        break;
                    case "2":
                        Console.WriteLine();
                        Console.WriteLine($"  Nuvarande: {loggedInUser.Email}");
                        loggedInUser.Email = Ui.AskEmail("  Ny e-post: ");
                        UserStore.Save(allUser);
                        Ui.Success("Sparat!");
                        Ui.Pause();
                        break;
                    case "3":
                        Console.WriteLine();
                        Console.WriteLine($"  Nuvarande: {loggedInUser.Phone}");
                        loggedInUser.Phone = Ui.Ask("  Nytt mobilnummer: ");
                        UserStore.Save(allUser);
                        Ui.Success("Sparat!");
                        Ui.Pause();
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
}
