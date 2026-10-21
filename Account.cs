namespace Bankapp;
public class Account
{
    public long AccountNumber;
    public string accountName;
    public CurrencyType Currency = CurrencyType.Sek;
    public decimal Balance { get; private set; }
    public Guid UserID;

    public void OpenNewAccount(User owner)
    {
        Account newAccount = new Account();

        Console.Write("Namn på konto - ");

        while (true)
        {
            string userInput = Console.ReadLine();
            if (int.TryParse(userInput, out int number))
            {
                Console.WriteLine("Namn kan inte vara en siffra.");
                continue;
            }
            else
            {
                newAccount.accountName = userInput;
                break;
            }
        }

        newAccount.AccountNumber = CreateAccountNumber();

        owner.Accounts.Add(newAccount)
    }
    public void CurrencyType(Account newAccount)
    {
        Console.WriteLine("Vilken valuta vill du använda?\n");
        Console.WriteLine("1. SEK");
        Console.WriteLine("2. EUR");
        Console.WriteLine("3. USD");
            while (true)
            {
                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        newAccount.Currency = Bankapp.CurrencyType.Sek;
                        break;
                    case "2":
                        newAccount.Currency = Bankapp.CurrencyType.Eur;
                        break;
                    case "3":
                        newAccount.Currency = Bankapp.CurrencyType.Usd;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt tal / Försök igen");
                        break;
                }
                return;
            }
    }
    public long CreateAccountNumber()
    {
        Random rnd = new Random();
        long AccountNumber = rnd.Next(999999999);

        return AccountNumber;
    }
}
