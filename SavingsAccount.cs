namespace Bankapp;

public class SavingsAccount : Account
{
    public decimal InterestRate = 0.03m;

    public decimal YearlyInterest(decimal amount)
    {
        return amount * InterestRate;
    }
}
