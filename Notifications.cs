using System;
using System.Collections.Generic;
using System.Text;

namespace Bankapp
{
    public enum NotificationChannel
    {
        Email,
        Sms
    }
    public static class Notifications
    {
        public static decimal LargeAmountLimit { get; set; } = 10000m;

        private static readonly string FilePath =
            Path.Combine(AppContext.BaseDirectory, "notifications.txt");

        private static readonly object FileLock = new();

        public static void NotifyUser(User owner, Transaction transaction, string currency)
        {
            if (transaction.Amount <= LargeAmountLimit)
                return;

            bool sms = owner.NotifyVia == NotificationChannel.Sms;
            string channel = sms ? "SMS" : "E-post";
            string recipient = sms ? owner.Phone : owner.Email;

            if (string.IsNullOrWhiteSpace(recipient))
                return;

            string message =
                 $"Stor transaktion: {transaction.Type} på {transaction.Amount:N2} {currency} " +
            $"({transaction.From} → {transaction.To}) har genomförts.";

            Console.WriteLine();
            Ui.Success($"[{channel} till {recipient}] {message}");

            string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {channel} till {recipient} | {message}";
            lock (FileLock)
            {
                File.AppendAllText(FilePath, line + Environment.NewLine);
            }
        }
    }
}
