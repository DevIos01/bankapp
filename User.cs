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
                Console.WriteLine($"{rowNumber++} - Konto : {account.AccountNumber} | Saldo : {account.Balance:N2} {account.Currency} "); 
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
        // SPARAR ALLA ANVÄNDARE
        UserStore.Save(allUsers);
        //--------skapa-konto-funktion som tar newUser.UserID--------

        Console.WriteLine();
        Ui.Success("  Kontot har skapats!");
        Ui.Pause();
    }

    public void Kyc(User newUser)
    {
        Ui.Header("DINA UPPGIFTER", "Steg 2 av 2");
        newUser.FullName = Ui.Ask("  Fullständigt namn: ");
        string email = Ui.Ask("  E-post:            ");
        while (!email.Contains('@'))
        {
            Ui.Error("  E-postadressen måste innehålla @.");
            email = Ui.Ask("  E-post:            ");
        }
        newUser.Email = email;
        newUser.Phone = Ui.Ask("  Telefon:           ");
    }

    public User? Login(List<User> allUsers)
    {
        Ui.Header("LOGGA IN");
        Username = Ui.Ask("  Användarnamn: ");
        Password = Ui.Ask("  Lösenord:     ");
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
}

