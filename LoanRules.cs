namespace Bankapp;

public static class LoanRules
{
    // Adminens standardränta, 4 %
    public static decimal DefaultRate { get; set; } = 0.04m;

    public static decimal GetMaxLoanAmount(decimal accountBalance)
    {
        return accountBalance * 2m;
    }

    public static decimal GetAnnualInterestRate(
        decimal accountBalance,
        decimal defaultRate)
    {
        const decimal rateStep = 0.05m;

        int tier = accountBalance switch
        {
            >= 200_000m => 0,
            >= 100_000m => 1,
            > 50_000m => 2,
            _ => 3
        };

        return Math.Max(defaultRate, 0.04m) + tier * rateStep;
    }
}




