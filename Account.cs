namespace Bankapp;

public class Account
{
    public long AccountNumber;
    public CurrencyType Currency = CurrencyType.Sek;
    private double _balance = 0;
    public Guid AccountID;

    public double Balance()
    {
        return _balance;
        
    }




}
