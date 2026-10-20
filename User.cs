namespace Bankapp;

public class User
{
    public string FullName = "";
    public Guid UserID = Guid.NewGuid();
    public string Email = "";
    public string Phone = "";
    public string Username = "";
    public string Password = "";
    public List<Account> Accounts = new List<Account>();

    public void DisplayAccountInfo()
    {
        Console.WriteLine($"{FullName} bankkonton är :\n");

        if (Accounts.Count == 0)
        {
            Console.WriteLine("Du har inga konton!.");
        }
        else
        {
            int rowNumber = 1;
            foreach (var account in Accounts)
            {
                Console.WriteLine($"{rowNumber ++} - Konto : {account.AccountNumber} | Saldo : {account.Balance} {account.Currency} "); 
            }
        }
    }
}