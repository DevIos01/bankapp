using System.Text.Json.Serialization;

namespace Bankapp;

[JsonDerivedType(typeof(Account), "normal")]
[JsonDerivedType(typeof(SavingsAccount), "savings")]
public class Account
{
    public long AccountNumber;
    public string AccountName = "";
    public CurrencyType Currency = CurrencyType.Sek;
<<<<<<< Updated upstream
    [JsonInclude]
    public decimal Balance { get; private set; }
    public Guid UserID;

    public List<Transaction> Transactions = new List<Transaction>();
    public bool Deposit(decimal amount)
    {
        if (amount <= 0)
            return false;
        Balance += amount;
        return true;
    }
    public bool Withdraw(decimal amount)
    {
        if (amount <= 0 || amount > Balance)
            return false;
        Balance -= amount;
        return true;
    }
    public void AddTransaction(Transaction transaction)
    {
        Transactions.Add(transaction);
    }
    public List<Transaction> GetTransactionLog()
    {
        return Transactions.OrderByDescending(t => t.Timestamp).ToList();
    }

    public bool OpenNewAccount(User owner)
    {
        Ui.Header("NYTT KONTO");

        string? accountName = AskAccountName("  Namn på konto: ");
        if (accountName is null)
            return false;

        Account newAccount = new Account
        {
            AccountName = accountName,
            AccountNumber = CreateAccountNumber(),
            UserID = owner.UserID
        };

        if (!ChooseCurrency(newAccount))
            return false;

        owner.Accounts.Add(newAccount);

        Console.WriteLine();
        Ui.Success($"Kontot {newAccount.AccountName} har öppnats.");
        Ui.Pause();
        return true;
    }

    public bool OpenSavingsAccount(User owner)
    {
        SavingsAccount newAccount = new SavingsAccount();

        Ui.Header("NYTT SPARKONTO", "3% årlig ränta");
        Console.WriteLine(
            $"  Exempel: 10 000 kr ger {newAccount.YearlyInterest(10000):N0} kr i ränta per år.");
        Console.WriteLine();
        Console.WriteLine("  Skriv 0 när som helst för att avbryta.");

        string? accountName = AskAccountName("  Namn på sparkonto: ");
        if (accountName is null)
            return false;

        newAccount.AccountName = accountName;
        newAccount.AccountNumber = CreateAccountNumber();
        newAccount.UserID = owner.UserID;

        owner.Accounts.Add(newAccount);

        Console.WriteLine();
        Ui.Success($"Sparkontot {newAccount.AccountName} har öppnats.");
        Ui.Pause();
        return true;
    }

    public bool ChooseCurrency(Account newAccount)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("  Vilken valuta vill du använda?");
            Ui.Option("1", "SEK");
            Ui.Option("2", "EUR");
            Ui.Option("3", "USD");
            Ui.Option("0", "Avbryt och gå tillbaka till BANKKONTON");
            Console.WriteLine();

            string choice = Ui.Ask("  Val: ");

            switch (choice)
            {
                case "1":
                    newAccount.Currency = CurrencyType.Sek;
                    return true;

                case "2":
                    newAccount.Currency = CurrencyType.Eur;
                    return true;

                case "3":
                    newAccount.Currency = CurrencyType.Usd;
                    return true;

                case "0":
                    return false;

                default:
                    Ui.Error("Välj 1, 2, 3 eller 0 för att avbryta.");
                    break;
            }
        }
    }

    private static string? AskAccountName(string prompt)
    {
        while (true)
        {
            string input = Ui.Ask(
                $"{prompt.TrimEnd().TrimEnd(':')} (0 = avbryt): ");

            if (input == "0")
                return null;

            if (int.TryParse(input, out _))
            {
                Ui.Error("Namn kan inte vara en siffra.");
                continue;
            }

            return input;
        }
    }

    public long CreateAccountNumber()
    {
        Random rnd = new Random();
        return rnd.Next(999999999);
    }
}
=======
    public decimal Balance { get;  set; }
    public Guid UserID;

    public bool Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            return false;
        }
        else if (Balance < amount)
        {
            return false;
        }
        else
        {
            Balance -= amount;
            return true;
        }
    }
    public bool Deposit(decimal amount)
    {
        if (amount > 0)
        {
            Balance += amount;
            return true;
        }
        return false;
    }
}
>>>>>>> Stashed changes
