using System;
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
            Console.WriteLine("Du har inga konton!");
        }
        else
        {
            int rowNumber = 1;
            foreach (var account in Accounts)
            {
                Console.WriteLine($"{rowNumber++} - Konto : {account.AccountNumber} | Saldo : {account.Balance} {account.Currency} "); 
            }
        }
    }


    public void CreateAccount(List<User> allUsers)
    {
        User newUser = new User(); //Skapa konto

        Ui.Header("SKAPA KONTO", "Steg 1 av 2");
        newUser.Username = Ui.Ask("  Användarnamn: ");
        newUser.Password = Ui.Ask("  Lösenord:     ");

        if (allUsers.Any(u => u.Username == newUser.Username))
        {
            Console.WriteLine();
            Ui.Error("  Användarnamnet är upptaget, välj ett annat.");
            Ui.Pause();
            return;
        }

        Kyc(newUser); //ny meny
        allUsers.Add(newUser);
        //--------skapa-konto-funktion som tar newUser.UserID--------

        Console.WriteLine();
        Ui.Success("  Kontot har skapats!");
        Ui.Pause();
    }

    public void Kyc(User newUser)
    {
        Ui.Header("DINA UPPGIFTER", "Steg 2 av 2");
        newUser.FullName = Ui.Ask("  Fullständigt namn: ");
        newUser.Email = Ui.Ask("  E-post:            ");
        newUser.Phone = Ui.Ask("  Telefon:           ");
    }

    public void Login(List<User> allUsers)
    {
        Ui.Header("LOGGA IN");
        Username = Ui.Ask("  Användarnamn: ");
        Password = Ui.Ask("  Lösenord:     ");
        Console.WriteLine();

        if (allUsers.Any(u => u.Username == Username && u.Password == Password))
        {
            Ui.Success($"  Välkommen {Username}!");
            Ui.Pause();
            return;
        }

        Ui.Error("  Fel användarnamn eller lösenord.");
        Ui.Pause();
    }
}

