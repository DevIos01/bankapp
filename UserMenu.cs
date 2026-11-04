using Bankapp;
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
                        Loan.ShowLoans(loggedInUser);
                        break;

                    case "2":
                        Loan.RequestLoan(loggedInUser, allUser, LoanRules.DefaultRate);
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
                Ui.Option("4", "Sätt in pengar");
                Ui.Option("5", "Överför pengar");
                Ui.Option("6", "Transaktionslogg");
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
                        DepositMenu(loggedInUser, allUser);
                        break;

                    case "5":
                        TransferMenu(loggedInUser, allUser);
                        break;

                    case "6":
                        TransactionLogMenu(loggedInUser);
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
                        {
                            Console.WriteLine();
                            Console.WriteLine($"  Nuvarande: {loggedInUser.FullName}");

                            string? newName = AskOrCancel("  Nytt namn (0 = tillbaka): ");
                            if (newName is null)
                                break;

                            loggedInUser.FullName = newName;
                            UserStore.Save(allUser);
                            Ui.Success("Sparat!");
                            Ui.Pause();
                            break;
                        }

                    case "2":
                        {
                            Console.WriteLine();
                            Console.WriteLine($"  Nuvarande: {loggedInUser.Email}");

                            string? newEmail = AskEmailOrCancel("  Ny e-post (0 = tillbaka): ");
                            if (newEmail is null)
                                break;

                            loggedInUser.Email = newEmail;
                            UserStore.Save(allUser);
                            Ui.Success("Sparat!");
                            Ui.Pause();
                            break;
                        }

                    case "3":
                        {
                            Console.WriteLine();
                            Console.WriteLine($"  Nuvarande: {loggedInUser.Phone}");

                            string? newPhone = AskOrCancel("  Nytt mobilnummer (0 = tillbaka): ");
                            if (newPhone is null)
                                break;

                            loggedInUser.Phone = newPhone;
                            UserStore.Save(allUser);
                            Ui.Success("Sparat!");
                            Ui.Pause();
                            break;
                        }

                    case "0":
                        return;

                    default:
                        Ui.Error("Fel val, försök igen.");
                        Ui.Pause();
                        break;
                }
            }
        }

        private static string? AskOrCancel(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim() ?? "";

                if (input == "0")
                    return null;

                if (!string.IsNullOrWhiteSpace(input))
                    return input;

                Ui.Error("Får inte vara tomt.");
            }
        }

        private static string? AskEmailOrCancel(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim() ?? "";

                if (input == "0")
                    return null;

                if (string.IsNullOrWhiteSpace(input))
                {
                    Ui.Error("Får inte vara tomt.");
                    continue;
                }

                if (input.Contains('@'))
                    return input;

                Ui.Error("E-postadressen måste innehålla @.");
            }
        }

        private static Account? ChooseAccount(User user, string prompt)
        {
            if (user.Accounts.Count == 0)
            {
                Ui.Error("Du har inga konton.");
                Ui.Pause();
                return null;
            }

            while (true)
            {
                Console.WriteLine();

                for (int i = 0; i < user.Accounts.Count; i++)
                {
                    Account account = user.Accounts[i];
                    Ui.Option(
                        $"{i + 1}",
                        $"{account.AccountName} — {account.AccountNumber} — " +
                        $"{account.Balance:N2} {account.Currency.ToString().ToUpper()}");
                }

                Ui.Option("0", "Avbryt och gå tillbaka till BANKKONTON");
                string input = Ui.Ask(prompt);

                if (input == "0")
                    return null;

                if (int.TryParse(input, out int accountChoice) &&
                    accountChoice >= 1 &&
                    accountChoice <= user.Accounts.Count)
                {
                    return user.Accounts[accountChoice - 1];
                }

                Ui.Error(
                    $"Välj ett konto mellan 1 och {user.Accounts.Count}, " +
                    "eller 0 för att avbryta.");
            }
        }

        private static decimal? AskAmount()
        {
            while (true)
            {
                string input = Ui.Ask(
                    "  Belopp (0 = avbryt och gå tillbaka till BANKKONTON): ");

                if (input == "0")
                    return null;

                if (decimal.TryParse(input, out decimal amount))
                    return amount;

                Ui.Error("Ange ett giltigt belopp, eller 0 för att avbryta.");
            }
        }

        private static void DepositMenu(User user, List<User> allUsers)
        {
            Ui.Header("SÄTT IN PENGAR");

            Account? destination = ChooseAccount(user, "  Välj konto: ");
            if (destination is null)
                return;

            decimal? amount = AskAmount();
            if (amount is null)
                return;

            bool success = TransactionService.Deposit(
                user, allUsers, destination, amount.Value);

            if (success)
            {
                Ui.Success(
                    $"Insättningen på {amount.Value:N2} " +
                    $"{destination.Currency.ToString().ToUpper()} genomfördes.");
            }
            else
            {
                Ui.Error("Insättningen misslyckades. Beloppet måste vara större än noll.");
            }

            Ui.Pause();
        }

        private static void TransferMenu(User user, List<User> allUsers)
        {
            Ui.Header("ÖVERFÖR PENGAR");

            Account? source = ChooseAccount(user, "  Välj från-konto: ");
            if (source is null)
                return;

            Account? destination = ChooseAccount(user, "  Välj till-konto: ");
            if (destination is null)
                return;

            decimal? amount = AskAmount();
            if (amount is null)
                return;

            bool success = TransactionService.Transfer(
                user, allUsers, source, destination, amount.Value);

            if (success)
            {
                Ui.Success(
                    $"Överföringen på {amount.Value:N2} " +
                    $"{source.Currency.ToString().ToUpper()} genomfördes.");
            }
            else
            {
                Ui.Error("Överföringen misslyckades. Kontrollera belopp och saldo.");
            }

            Ui.Pause();
        }

        private static void TransactionLogMenu(User user)
        {
            Ui.Header("TRANSAKTIONSLOGG");

            Account? account = ChooseAccount(user, "  Välj konto: ");
            if (account is null)
                return;

            List<Transaction> transactions = account.GetTransactionLog();

            Console.WriteLine();
            Console.WriteLine(
                $"  Transaktioner för {account.AccountName} ({account.AccountNumber}):");
            Console.WriteLine();

            if (transactions.Count == 0)
            {
                Console.WriteLine("  Inga transaktioner ännu.");
            }
            else
            {
                foreach (Transaction transaction in transactions)
                {
                    string amount = transaction.Exchange ??
                        $"{transaction.Amount:N2} {account.Currency.ToString().ToUpper()}";

                    Console.WriteLine(
                        $"  {transaction.Timestamp.ToLocalTime():yyyy-MM-dd HH:mm:ss} | " +
                        $"{transaction.Type} | {transaction.From} → {transaction.To} | " +
                        $"{amount} | " +
                        $"{transaction.Status}");
                }
            }

            Ui.Pause();
        }

        public static void TransferBetweenAccounts(User loggedInUser, List<User> allUser)
        {
            Console.WriteLine("Välj konto att överföra från:");
            Account? fromAccount = loggedInUser.SelectAccount();
            if (fromAccount is null)
            {
                return;
            }
            Console.WriteLine("Välj konto att överföra till:");
            Account? toAccount = loggedInUser.SelectAccount();
            if (toAccount is null)
            {
                return;
            }
            Console.Write("Ange belopp: ");
            string? input = Console.ReadLine();
            if (!decimal.TryParse(input, out decimal amount))
            {
                Ui.Error("Ogiltigt belopp.");
                return;
            }
            
            if (loggedInUser.TransferMoney(fromAccount, toAccount , amount))
            {
                Ui.Success("Överföringen lyckades.");
                UserStore.Save(allUser);
            }
            else
            {
                Ui.Error("Överföringen misslyckades.");
            }


        }
    }
}
