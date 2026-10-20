namespace Bankapp;

public class Account
{
    public long AccountNumber;
    public CurrencyType Currency = CurrencyType.Sek;
    public decimal Balance { get; private set; }
    public Guid UserID;
}
