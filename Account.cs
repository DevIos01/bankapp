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

        Ui.Header("NYTT KONTO");

        while (true)
        {
            string userInput = Ui.Ask("  Namn på konto: ");
            if (int.TryParse(userInput, out int number))
            {
                Ui.Error("Namn kan inte vara en siffra.");
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

        Console.WriteLine();
        Ui.Success($"Kontot {newAccount.AccountName} har öppnats.");
        Ui.Pause();
    }

    public void OpenSavingsAccount(User owner)
    {
        SavingsAccount newAccount = new SavingsAccount();

        Ui.Header("NYTT SPARKONTO", "3% årlig ränta");
        Console.WriteLine($"  Exempel: 10 000 kr ger {newAccount.YearlyInterest(10000):N0} kr i ränta per år.");
        Console.WriteLine();

        while (true)
        {
            string userInput = Ui.Ask("  Namn på sparkonto: ");
            if (int.TryParse(userInput, out int number))
            {
                Ui.Error("Namn kan inte vara en siffra.");
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

        Console.WriteLine();
        Ui.Success($"Sparkontot {newAccount.AccountName} har öppnats.");
        Ui.Pause();
    }

    public void ChooseCurrency(Account newAccount)
    {
        Console.WriteLine();
        Console.WriteLine("  Vilken valuta vill du använda?");
        Ui.Option("1", "SEK");
        Ui.Option("2", "EUR");
        Ui.Option("3", "USD");
        Console.WriteLine();

        int choice = Ui.AskNumber("  Val: ", 1, 3);

        switch (choice)
        {
            case 1:
                newAccount.Currency = Bankapp.CurrencyType.Sek;
                break;
            case 2:
                newAccount.Currency = Bankapp.CurrencyType.Eur;
                break;
            case 3:
                newAccount.Currency = Bankapp.CurrencyType.Usd;
                break;
        }
    }
    public long CreateAccountNumber()
    {
        Random rnd = new Random();
        long AccountNumber = rnd.Next(999999999);

        return AccountNumber;
    }
}
