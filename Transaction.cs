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
    public long ToAccountNumber { get; set; }
    public decimal Amount { get; set; }
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
        bool success = user.Accounts.Contains(destination) && amount > 0;

        if (success)
            destination.Credit(amount);

        user.Transactions.Add(new Transaction
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
        bool success =
            user.Accounts.Contains(source) &&
            user.Accounts.Contains(destination) &&
            source != destination &&
            source.Currency == destination.Currency &&
            amount > 0 &&
            source.Balance >= amount;

        if (success)
        {
            source.Debit(amount);
            destination.Credit(amount);
        }

        user.Transactions.Add(new Transaction
        {
            Type = "Överföring",
            From = $"{source.AccountName} ({source.AccountNumber})",
            To = $"{destination.AccountName} ({destination.AccountNumber})",
            FromAccountNumber = source.AccountNumber,
            ToAccountNumber = destination.AccountNumber,
            Amount = amount,
            Status = success ? "Genomförd" : "Misslyckad"
        });

        UserStore.Save(allUsers);
        return success;
    }
}