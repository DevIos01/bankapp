using System;
namespace Bankapp;

public enum UserRole //Admin customer
{
    Customer,
    Admin
}
public class User
{
    public string FullName = "";
    public Guid UserID = Guid.NewGuid();
    public string Email = "";
    public string Phone = "";
    public string Username = "";
    public string Password = "";
    public UserRole Role = UserRole.Customer; //default role 
    public int FailedLoginAttempts = 0;
    public bool IsLocked => FailedLoginAttempts >= 3;

    public List<Account> Accounts = new List<Account>();

    public void DisplayAccountInfo()
    {
        Console.WriteLine("  Dina konton:");
        Console.WriteLine();

        if (Accounts.Count == 0)
        {
            Console.WriteLine("  Du har inga konton ännu.");
        }
        else
        {
            int rowNumber = 1;
            foreach (var account in Accounts)
            {
                // name, number and balance in lined-up columns, e.g. "1. Lönekonto  123456789  5 000,00 SEK"
                Console.WriteLine($"  {rowNumber++}. {account.AccountName,-15} {account.AccountNumber,-10} {account.Balance,12:N2} {account.Currency.ToString().ToUpper()}");
            }
        }
    }


    public void CreateAccount(List<User> allUsers)
    {
        User newUser = new User(); //Skapa konto

        Ui.Header("SKAPA KONTO", "Steg 1 av 2");
        newUser.Username = Ui.Ask("  Användarnamn: ");
        newUser.Password = Ui.AskPassword("  Lösenord:     ");


        if (allUsers.Any(u => u.Username == newUser.Username))
        {
            Console.WriteLine();
            Ui.Error("  Användarnamnet är upptaget, välj ett annat.");
            Ui.Pause();
            return;
        }

        Kyc(newUser); //ny meny
        Account personkonto = new Account
        {
            AccountName = "Personkonto",
            AccountNumber = new Account().CreateAccountNumber(),
            UserID = newUser.UserID,
            Currency = CurrencyType.Sek
        };

        newUser.Accounts.Add(personkonto);
        allUsers.Add(newUser);
        // SPARAR ALLA ANVÄNDARE
        UserStore.Save(allUsers);

        Console.WriteLine();
        Ui.Success("  Kontot har skapats!");
        Ui.Pause();
    }

    public void Kyc(User newUser)
    {
        Ui.Header("DINA UPPGIFTER", "Steg 2 av 2");
        newUser.FullName = Ui.Ask("  Fullständigt namn: ");
        newUser.Email = Ui.AskEmail("  E-post:            ");
        newUser.Phone = Ui.Ask("  Telefon:           ");
    }

    public User? Login(List<User> allUsers)
    {
        Ui.Header("LOGGA IN");
        Username = Ui.Ask("  Användarnamn: ");
        Password = Ui.AskPassword("  Lösenord:     ");
        Console.WriteLine();

        User? user = allUsers.FirstOrDefault(u => u.Username == Username); //find useraccount with typed username

        //controls if account is locked before checking password

        if(user != null && user.IsLocked)
        {
            Ui.Error("Kontot är låst.");
            Ui.Pause();
            return null;
        }

        if (user != null && user.Password == Password)
        {
            //correct password resets count
            
                user.FailedLoginAttempts = 0;
                // SPARAR NOLLSTÄLLNING
                UserStore.Save(allUsers);

            
            Ui.Success($"  Välkommen {Username}!");
            Ui.Pause();
            return user;
        }

        //if username exist but password is incorrect. Give 3 attempts before locking account  
        if (user != null && user.Role != UserRole.Admin)
        {
            user.FailedLoginAttempts++;

            // sparar räknare och låsning
            UserStore.Save(allUsers);

            if (user.IsLocked)
            {
                Ui.Error("  Du har angivit fel lösenord 3 gånger, kontot är nu låst.");
                Ui.Pause();
                return null;
            }
        }
        Ui.Error("  Fel användarnamn eller lösenord.");
        Ui.Pause();
        return null;

    }

    public bool TransferMoney(Account fromAccount, Account toAccount, decimal amount)
    {
        if (!Accounts.Contains(fromAccount) || !Accounts.Contains(toAccount))
        {
            return false;
        }

        if (fromAccount.AccountNumber == toAccount.AccountNumber)
        {
            return false;
        }
        if (amount <= 0 || amount > fromAccount.Balance)
        {
            return false;
        }


        decimal convertedAmount = amount;
        if (fromAccount.Currency != toAccount.Currency)
        {
            decimal amountInSek;

            switch (fromAccount.Currency)
            {
                case CurrencyType.Sek:
                    amountInSek = amount;
                    break;

                case CurrencyType.Usd:
                    amountInSek = amount * ExchangeRateManager.UsdToSek;
                    break;

                case CurrencyType.Eur:
                    amountInSek = amount * ExchangeRateManager.EurToSek;
                    break;

                default:
                    return false;
            }

            switch (toAccount.Currency)
            {
                case CurrencyType.Sek:
                    convertedAmount = amountInSek;
                    break;

                case CurrencyType.Usd:
                    convertedAmount = amountInSek / ExchangeRateManager.UsdToSek;
                    break;

                case CurrencyType.Eur:
                    convertedAmount = amountInSek / ExchangeRateManager.EurToSek;
                    break;

                default:
                    return false;
            }

            convertedAmount = Math.Round(convertedAmount, 2);

        //// Future improvement: support transfers between different currencies.
        //if (fromAccount.Currency != toAccount.Currency)
        //{
        //    return false;

        //}

        if (!fromAccount.Withdraw(amount))
        {
            return false;
        }


        if (!toAccount.Deposit(convertedAmount))

        if (!toAccount.Deposit(amount))

        {
            fromAccount.Deposit(amount); // Rollback
            return false;
        }

        // Point of integration for transaction logging
        return true;

    }



    public Account? SelectAccount()
    {
        if (Accounts.Count == 0)
        {
            Console.WriteLine("Du har inga konton!");
            return null;
        }
        while (true)
        {

            DisplayAccountInfo();
            Console.Write("Välj kontonummer: ");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int selection))
            {
                Console.WriteLine("Ogiltigt val!");
                continue;
            }
            if (selection < 1 || selection > Accounts.Count)
            {
                Console.WriteLine("Ogiltigt val!");
                continue;
            }
            return Accounts[selection - 1];

        }
        
    }
}

