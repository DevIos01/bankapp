using System.Text.Json.Serialization;

namespace Bankapp;

[JsonDerivedType(typeof(Account), "normal")]
[JsonDerivedType(typeof(SavingsAccount), "savings")]
public class Account
{
    public long AccountNumber;
    public string AccountName = "";
    public CurrencyType Currency = CurrencyType.Sek;
    public decimal Balance { get; private set; }
    public Guid UserID;

    public void OpenNewAccount(User owner)
    {
        Account newAccount = new Account();

        Console.Write("Namn på konto - ");

        while (true)
        {
            string? userInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(userInput) || int.TryParse(userInput, out int number))
            {
                Console.WriteLine("Namn kan inte vara en siffra.");
                continue;
            }
            else
            {
                newAccount.AccountName = userInput;
                break;
            }
        }

        newAccount.AccountNumber = CreateAccountNumber();
        newAccount.UserID = owner.UserID;
        ChooseCurrency(newAccount);

        owner.Accounts.Add(newAccount);
    }

    public void OpenSavingsAccount(User owner)
    {
        SavingsAccount newAccount = new SavingsAccount();

        Console.WriteLine("Sparkonto med 3% årlig ränta");
        Console.WriteLine("10 000 kr med 3% = 300 kr årligen");

        Console.Write("Namn på sparkonto - ");

        while (true)
        {
            string? userInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(userInput) || int.TryParse(userInput, out int number))
            {
                Console.WriteLine("Namn kan inte vara en siffra.");
                continue;
            }
            else
            {
                newAccount.AccountName = userInput;
                break;
            }
        }

        newAccount.AccountNumber = CreateAccountNumber();
        newAccount.UserID = owner.UserID;
        owner.Accounts.Add(newAccount);
    }

    public void ChooseCurrency(Account newAccount)
    {
        Console.WriteLine("Vilken valuta vill du använda?\n");
        Console.WriteLine("1. SEK");
        Console.WriteLine("2. EUR");
        Console.WriteLine("3. USD");
            while (true)
            {
                string? userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        newAccount.Currency = Bankapp.CurrencyType.Sek;
                        return;
                    case "2":
                        newAccount.Currency = Bankapp.CurrencyType.Eur;
                        return;
                    case "3":
                        newAccount.Currency = Bankapp.CurrencyType.Usd;
                        return;
                    default:
                        Console.WriteLine("Ogiltigt tal / Försök igen");
                        break;
                }
            }
    }
    public long CreateAccountNumber()
    {
        Random rnd = new Random();
        long AccountNumber = rnd.Next(999999999);

        return AccountNumber;
    }
}
