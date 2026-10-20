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

    public void CreateAccount(List<User> allUsers)
    {
        //  Console.WriteLine("Ange unikt användarnamn.");
        User newUser = new User(); //Skapa konto 
        newUser.Username = "user";
        newUser.Password = "1234";
        if (allUsers.Any(u => u.Username == "user"))
        {
            Console.WriteLine("Ange unikt användarnamn");
            return;
        }
        Kyc(newUser); //ny meny
        allUsers.Add(newUser);
        //--------skapa-konto-funktion som tar newUser.UserID--------
        Console.WriteLine("Konton har skapats.");

    }

    public void Kyc(User newUser)
    {
        newUser.FullName = "";
        newUser.Email = "";
        newUser.Phone = "";
    }

    public bool Login(List<User> allUsers)
    {
        if (allUsers.Any(u => u.Username == Username && u.Password == Password))
        {
            return true;
        }

        return false;
    }
}
