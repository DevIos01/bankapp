using System;
using System.Collections.Generic;
using System.Text;

namespace Bankapp;

public class Loan
{
    public decimal LoanAmount { get; set; }
    public decimal AccountBalanceAtApproval { get; set; }
    public decimal AnnualInterestRate { get; set; }
    public int LoanDurationYears { get; set; }
    public long PayoutAccountNumber { get; set; }

    public decimal InterestCost => 
        LoanAmount * AnnualInterestRate * LoanDurationYears;

    public decimal TotalCost =>
        LoanAmount + InterestCost;


    public static void RequestLoan(
       User loggedInUser,
       List<User> allUsers,
       decimal defaultRate)
    {
        // Räkna alla konton omräknat till SEK, minus det som redan är lånat.
        decimal totalLoans = loggedInUser.Loans.Sum(loan => loan.LoanAmount);
        decimal accountBalance = loggedInUser.Accounts
            .Sum(account => ExchangeRateManager.Convert(account.Balance, account.Currency, CurrencyType.Sek))
            - totalLoans;

        decimal maxLoan = Math.Max(0m, LoanRules.GetMaxLoanAmount(accountBalance) - totalLoans);

        Console.WriteLine($"Egna pengar i banken: {accountBalance:N2} kr");
        Console.WriteLine($"Du kan låna högst: {maxLoan:N2} kr");

        if (!decimal.TryParse(Ui.Ask("Lånebelopp: "), out decimal loanAmount) ||
            loanAmount <= 0 ||
            loanAmount > maxLoan)
        {
            Ui.Error("Ogiltigt belopp eller beloppet överstiger maxgränsen.");
            Ui.Pause();
            return;
        }

        if (!int.TryParse(Ui.Ask("Löptid i hela år: "), out int durationYears) ||
            durationYears <= 0)
        {
            Ui.Error("Ange en giltig löptid.");
            Ui.Pause();
            return;
        }
        decimal rate = LoanRules.GetAnnualInterestRate(accountBalance, defaultRate);
        decimal interestCost = loanAmount * rate * durationYears;
        decimal totalCost = loanAmount + interestCost;

Console.WriteLine($"Årsränta: {rate*100m}%");
        Console.WriteLine($"Beräknad ränta för hela löptiden: {interestCost} kr");
        Console.WriteLine($"Totalt att betala enligt enkel ränta: {totalCost} kr");

        // Välj ett konto i SEK att betala ut lånet till.
        var payoutAccounts = loggedInUser.Accounts
            .Where(account => account.Currency == CurrencyType.Sek)
            .ToList();

        if (payoutAccounts.Count == 0)
        {
            Ui.Error("Du behöver ett SEK-konto för att ta emot lånet.");
            Ui.Pause();
            return;
        }

        for (int i = 0; i < payoutAccounts.Count; i++)
        {
            Console.WriteLine(
                $"[{i + 1}] Konto {payoutAccounts[i].AccountNumber} – {payoutAccounts[i].Balance:N2} kr");
        }

        if (!int.TryParse(Ui.Ask("Välj konto: "), out int selection) ||
            selection < 1 ||
            selection > payoutAccounts.Count)
        {
            Ui.Error("Ogiltigt kontoval.");
            Ui.Pause();
            return;
        }

        Account payoutAccount = payoutAccounts[selection - 1];

        if (!Ui.Ask("Bekräfta lånet? (j/n): ")
            .Equals("j", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Lånet avbröts.");
            Ui.Pause();
            return;
        }

        var loan = new Loan
        {
            LoanAmount = loanAmount,
            AccountBalanceAtApproval = accountBalance,
            AnnualInterestRate = rate,
            LoanDurationYears = durationYears,
            PayoutAccountNumber = payoutAccount.AccountNumber
        };

        loggedInUser.Loans.Add(loan);
        payoutAccount.Deposit(loanAmount);
        payoutAccount.AddTransaction(new Transaction
        {
            Type = "Lån",
            From = "Banken",
            To = $"{payoutAccount.AccountName} ({payoutAccount.AccountNumber})",
            ToAccountNumber = payoutAccount.AccountNumber,
            Amount = loanAmount,
            Status = "Genomförd"
        });
        UserStore.Save(allUsers);

        Ui.Success("Lånet har betalats ut till ditt konto.");
        Ui.Pause();
    }

    public static void ShowLoans(User loggedInUser) //show loan amount, interest, duration and  total cost
    {
        Console.Clear();
        Console.WriteLine("=== Mina lån ===");

        if (loggedInUser.Loans.Count == 0)
        {
            Console.WriteLine("Du har inga lån.");
            Ui.Pause();
            return;
        }

        for (int i = 0; i < loggedInUser.Loans.Count; i++)
        {
            Loan loan = loggedInUser.Loans[i];

            Console.WriteLine($"\nLån {i + 1}");
            Console.WriteLine($"Belopp: {loan.LoanAmount} kr");
            Console.WriteLine($"Årsränta: {loan.AnnualInterestRate * 10m}%");
            Console.WriteLine($"Löptid: {loan.LoanDurationYears} år");
            Console.WriteLine($"Ränta totalt: {loan.InterestCost} kr");
            Console.WriteLine($"Totalt att betala: {loan.TotalCost} kr");
            Console.WriteLine($"Utbetalt till konto: {loan.PayoutAccountNumber}");
        }

        Ui.Pause();
    }
}