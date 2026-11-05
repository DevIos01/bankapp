using System;
using System.Collections.Generic;
using System.Text;

namespace Bankapp;

public class Transaction
{
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Type { get; set; } = "";
    public string From { get; set; } = "";
    public string To { get; set; } = "";
    public long? FromAccountNumber { get; set; }
    public long? ToAccountNumber { get; set; }
    public decimal Amount { get; set; }
    public string? Exchange { get; set; } // e.g. "10,00 USD → 100,00 SEK" when the currencies differ
    public string Status { get; set; } = "";
}

public static class TransactionService
{
    public static bool Deposit(
        User user,
        List<User> allUsers,
        Account destination,
        decimal amount)
    {
        bool success = user.Accounts.Contains(destination) && destination.Deposit(amount);

        destination.AddTransaction(new Transaction
        {
            Type = "Insättning",
            From = "Extern insättning",
            To = $"{destination.AccountName} ({destination.AccountNumber})",
            FromAccountNumber = null,
            ToAccountNumber = destination.AccountNumber,
            Amount = amount,
            Status = success ? "Genomförd" : "Misslyckad"
        });

        UserStore.Save(allUsers);
        return success;
    }

    public static bool Transfer(
        User user,
        List<User> allUsers,
        Account source,
        Account destination,
        decimal amount)
    {
        decimal receivedAmount =
            ExchangeRateManager.Convert(amount, source.Currency, destination.Currency);

        bool success =
            user.Accounts.Contains(source) &&
            user.Accounts.Contains(destination) &&
            source != destination &&
            amount > 0 &&
            receivedAmount > 0 &&
            source.Balance >= amount;

        if (success)
        {
            source.Withdraw(amount);
            destination.Deposit(receivedAmount);
        }

        Transaction transaction = new Transaction
        {
            Type = "Överföring",
            From = $"{source.AccountName} ({source.AccountNumber})",
            To = $"{destination.AccountName} ({destination.AccountNumber})",
            FromAccountNumber = source.AccountNumber,
            ToAccountNumber = destination.AccountNumber,
            Amount = amount,
            Exchange = source.Currency == destination.Currency
                ? null
                : $"{amount:N2} {source.Currency.ToString().ToUpper()} → " +
                  $"{receivedAmount:N2} {destination.Currency.ToString().ToUpper()}",
            Status = success ? "Genomförd" : "Misslyckad"
        };

        source.AddTransaction(transaction);

        if (destination != source)
            destination.AddTransaction(transaction);

        UserStore.Save(allUsers);
        return success;
    }

    public static Account? FindAccountByNumber(List<User> allUsers, long accountNumber)
    {
        foreach (User u in allUsers)
        {
            foreach (Account a in u.Accounts)
            {
                if (a.AccountNumber == accountNumber)
                    return a;
            }
            
        }
        return null;
    }
    public static bool TransferToOther(
    User user, List<User> allUsers,
    Account source, Account destination, decimal amount)
    {
        decimal receivedAmount =
            ExchangeRateManager.Convert(amount, source.Currency, destination.Currency);

        bool success =
            user.Accounts.Contains(source) &&
            source != destination &&
            amount > 0 &&
            receivedAmount > 0 &&
            source.Balance >= amount;

        if (success)
        {
            source.Withdraw(amount);
            destination.Deposit(receivedAmount);
        }

        Transaction transaction = new Transaction
        {
            Type = "Överföring",
            From = $"{source.AccountName} ({source.AccountNumber})",
            To = $"{destination.AccountName} ({destination.AccountNumber})",
            FromAccountNumber = source.AccountNumber,
            ToAccountNumber = destination.AccountNumber,
            Amount = amount,
            Exchange = source.Currency == destination.Currency
                ? null
                : $"{amount:N2} {source.Currency.ToString().ToUpper()} → " +
                  $"{receivedAmount:N2} {destination.Currency.ToString().ToUpper()}",
            Status = success ? "Genomförd" : "Misslyckad"
        };

        source.AddTransaction(transaction);
        destination.AddTransaction(transaction);

        UserStore.Save(allUsers);
        return success;
    }
}