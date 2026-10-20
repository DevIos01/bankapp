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

            string json = File.ReadAllText(FilePath);

            return JsonSerializer.Deserialize<List<User>>(json, Options)
                   ?? new List<User>();
        }

        public static void Save(List<User> users)
        {
            string json = JsonSerializer.Serialize(users, Options);
            File.WriteAllText(FilePath, json);
        }
    }
}