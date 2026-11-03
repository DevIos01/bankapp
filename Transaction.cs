using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Bankapp;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Type { get; set; } = "";
    public string From { get; set; } = "";
    public string To { get; set; } = "";
    public long? FromAccountNumber { get; set; }
    public long? ToAccountNumber { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = "";
}
public static class TransactionStatus
{
    public const string Pending = "Väntande";
    public const string Done = "Genomförd";
    public const string Failed = "Misslyckad";
}
public static class TransactionService
{
    internal static readonly object Sync = new();
    private static Timer? _timer;
    public static TimeSpan WaitTime { get; set; } = ReadInterval();

    private static TimeSpan ReadInterval()
    {
        string? env = Environment.GetEnvironmentVariable("TRANSFER_WAIT_TIME");
        return double.TryParse(env, out double minutes) && minutes > 0
            ? TimeSpan.FromMinutes(minutes)
            : TimeSpan.FromMinutes(1);
    }
    public static bool Deposit(User user, List<User> allUsers, Account destination, decimal amount)
    {
        lock (Sync)
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
                Status = success ? TransactionStatus.Done : TransactionStatus.Failed
            });

            UserStore.Save(allUsers);
            return success;
        }

    }

    public static bool Transfer(User user, List<User> allUsers, Account source, Account destination, decimal amount)
    {
        lock (Sync)
        {
            bool accepted =
                user.Accounts.Contains(source) &&
                user.Accounts.Contains(destination) &&
                source != destination &&
                source.Currency == destination.Currency &&
                amount > 0 &&
                source.Balance >= amount;

            Transaction transaction = new Transaction
            {
                Type = "Överföring",
                From = $"{source.AccountName} ({source.AccountNumber})",
                To = $"{destination.AccountName} ({destination.AccountNumber})",
                FromAccountNumber = source.AccountNumber,
                ToAccountNumber = destination.AccountNumber,
                Amount = amount,
                Status = accepted ? TransactionStatus.Pending : TransactionStatus.Failed
            };

            source.AddTransaction(transaction);

            if (destination != source)
                destination.AddTransaction(transaction);

            UserStore.Save(allUsers);
            return accepted;
        }
    }

    public static int PendingTransfer(List<User> allUsers)
    {
        lock (Sync)
        {
            DateTime cutOff = DateTime.UtcNow - WaitTime;
            List<Account> accounts = allUsers.SelectMany(u => u.Accounts).ToList();

            List<Transaction> due = accounts
                .SelectMany(a => a.Transactions)
                .Where(t => t.Type == "Överföring"
                        && t.Status == TransactionStatus.Pending
                        && t.Timestamp <= cutOff)
                .GroupBy(t => t.Id)
                .Select(g => g.First())
                .OrderBy(t => t.Timestamp)
                .ToList();

            foreach (Transaction t in due)
            {
                Account? source = accounts.FirstOrDefault(a => a.AccountNumber == t.FromAccountNumber);
                Account? destination = accounts.FirstOrDefault(a => a.AccountNumber == t.ToAccountNumber);

                bool ok =
                    source != null &&
                    destination != null &&
                    source.Currency == destination.Currency &&
                    source.Withdraw(t.Amount);

                if (ok)
                    destination!.Deposit(t.Amount);

                string status = ok ? TransactionStatus.Done : TransactionStatus.Failed;

                foreach (Transaction copy in accounts
                                .SelectMany(a => a.Transactions)
                                .Where(x => x.Id == t.Id))
                {
                    copy.Status = status;
                }

            }

            if (due.Count > 0)
                UserStore.Save(allUsers);

            return due.Count;

        }
    }

    public static void StartTimer(List<User> allUsers)
    {
        _timer?.Dispose();
        _timer = new Timer(_ =>
        {
            try { PendingTransfer(allUsers); }
            catch (Exception ex) { Console.Error.WriteLine($"Timer fel: {ex.Message}"); }
        }, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
    }

}