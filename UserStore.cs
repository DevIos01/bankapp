using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Bankapp
{
    public static class UserStore
    {
        private static readonly string FilePath =
            Path.Combine(AppContext.BaseDirectory, "users.json");

        private static readonly JsonSerializerOptions Options = new()
        {
            IncludeFields = true,
            WriteIndented = true
        };

        public static List<User> Load()
        {
            if (!File.Exists(FilePath))
            {
                return new List<User>();
            }

            try
            {
                string json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<User>>(json, Options) ?? new List<User>();
            }
            catch (JsonException)
            {
                Console.WriteLine("users.json kunde inte läsas, startar med tom lista.");
                return new List<User>();
            }
        }
        public static void Save(List<User> users)
        {
            // the transfer timer also saves, so one save at a time,
            // and write a temp file first so users.json is never left half written
            lock (TransactionService.Sync)
            {
                string json = JsonSerializer.Serialize(users, Options);
                File.WriteAllText(FilePath + ".tmp", json);
                File.Move(FilePath + ".tmp", FilePath, true);
            }
        }
    }

}