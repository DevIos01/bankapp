namespace Bankapp;

public class User
{
    public string FullName = "";
    public long AccountID;
    public string Email = "";
    public string Phone = "";
    public string Username = "";
    public string Password = "";
    public List<Account> accounts = new List<Account>();
}