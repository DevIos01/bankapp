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

        //List<User> allUsers = new List<User>();
        //User user1 = new User();
        //user1.FullName = "Sima";
        //Account account1 = new Account();
        //account1.Balance = 500;
        //account1.Currency = CurrencyType.Usd;
        //account1.AccountNumber = 100;
        //Account account2 = new Account();
        //account2.Balance = 200;
        //account2.Currency = CurrencyType.Eur;
        //account2.AccountNumber = 200;
        //Account account3 = new Account();
        //account3.Balance = 300;
        //account3.Currency = CurrencyType.Sek;
        //account3.AccountNumber = 300;
        //user1.Accounts.Add(account1);
        //user1.Accounts.Add(account2);
        //user1.Accounts.Add(account3);
        ////user1.DisplayAccountInfo();
        //User user2 = new User();
        //Account? fromAccount = user1.SelectAccount();
        //Account? toAccount = user1.SelectAccount();
        //bool success = user1.TransferMoney(fromAccount, toAccount, 10);

        //if (success)
        //{
        //    Console.WriteLine("Transfer successful.");
        //}
        //else
        //{
        //    Console.WriteLine("Transfer failed.");
        //}
        //user1.DisplayAccountInfo();

        //Console.ReadKey();
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
