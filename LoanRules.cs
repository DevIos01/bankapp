namespace Bankapp;

public static class LoanRules
{
    //  minimum interest 4 %
    public static decimal DefaultRate { get; set; } = 0.04m;

    public static decimal ShowRate(decimal defaultRate)
    {
        return defaultRate * 10m;
    }
    public static decimal GetMaxLoanAmount(decimal accountBalance)
    {
        return accountBalance * 5m; // accountbalance x 2 = max loan
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




