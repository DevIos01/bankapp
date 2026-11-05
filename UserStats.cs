using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Bankapp
{
    public static class UserStats
    {
        public static void Show(List<User> allUsers)
        {
            lock (TransactionService.Sync)
            {
                List<Account> accounts = allUsers.SelectMany(u => u.Accounts).ToList();

                List<Transaction> all = accounts
                    .SelectMany(a => a.Transactions)
                    .GroupBy(t => t.Id)
                    .Select(g => g.First())
                    .ToList();

                List<Transaction> transfers = all.Where(t => t.Type == "Överföring").ToList();
                List<Transaction> doneTransfers = transfers
                    .Where(t => t.Status == TransactionStatus.Done)
                    .ToList();

                Ui.Header("STATISTIK");

                // Antal transaktioner, values lined up like MINA LÅN
                Console.WriteLine("  Transaktioner:");
                Console.WriteLine($"    Totalt:          {all.Count}");
                Console.WriteLine($"    Insättningar:    {all.Count(t => t.Type == "Insättning")}");
                Console.WriteLine($"    Lån:             {all.Count(t => t.Type == "Lån")}");
                Console.WriteLine($"    Överföringar:    {transfers.Count}");
                Console.WriteLine($"      Väntande:      {transfers.Count(t => t.Status == TransactionStatus.Pending)}");
                Console.WriteLine($"      Genomförda:    {doneTransfers.Count}");
                Console.WriteLine($"      Misslyckade:   {transfers.Count(t => t.Status == TransactionStatus.Failed)}");

                Console.WriteLine();
                Console.WriteLine("  Totalt överfört:");

                var totals = doneTransfers
                    .Select(t => new
                    {
                        t.Amount,
                        Source = accounts.FirstOrDefault(a => a.AccountNumber == t.FromAccountNumber)
                    })
                    .Where(x => x.Source != null)
                    .GroupBy(x => x.Source!.Currency)
                    .ToList();

                if (totals.Count == 0)
                {
                    Console.WriteLine("    Inga genomförda överföringar ännu.");
                }
                else
                {
                    foreach (var group in totals)
                    {
                        Console.WriteLine(
                            $"    {group.Key.ToString().ToUpper() + ":",-17}{group.Sum(x => x.Amount):N2} ({group.Count()} st)");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("  Mest använda konton:");

                var topAccounts = accounts
                    .Select(a => new
                    {
                        Account = a,
                        Count = a.Transactions.Count(t => t.Status != TransactionStatus.Failed)
                    })
                    .Where(x => x.Count > 0)
                    .OrderByDescending(x => x.Count)
                    .Take(5)
                    .ToList();

                if (topAccounts.Count == 0)
                {
                    Console.WriteLine("    Inga transaktioner ännu.");
                }
                else
                {
                    int rank = 1;
                    foreach (var x in topAccounts)
                    {
                        string owner = allUsers.FirstOrDefault(u => u.UserID == x.Account.UserID)?.Username ?? "okänd";
                        Console.WriteLine(
                            $"    {rank++}. {x.Account.AccountName,-15} {x.Account.AccountNumber,-10} {owner,-12} {x.Count} st");
                    }
                }
            }

            Ui.Pause();
        }
    }
}
