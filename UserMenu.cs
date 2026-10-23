using System;
using System.Collections.Generic;
using System.Text;

namespace Bankapp
{
    public static class UserMenu
    {
        public static void EditKyc(User loggedInUser, List<User> allUser)
            {
            while (true)
            {
                Console.WriteLine("──────────────────────────────");
                Console.WriteLine("  [1] Ändra Namn");
                Console.WriteLine("  [2] Ändra Epost");
                Console.WriteLine("  [3] Ändra Mobilnummer");

                Console.WriteLine("  [0] Avsluta");
                Console.WriteLine("──────────────────────────────");
                Console.Write("Val: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine($"Nuvarande uppgifter: {loggedInUser.FullName}");
                        loggedInUser.FullName = Ui.Ask("Nytt namn: ");
                        UserStore.Save(allUser);

                        break;
                    case "2":
                        Console.WriteLine($"Nuvarande uppgifter: {loggedInUser.Email}");
                        loggedInUser.Email = Ui.Ask("Ny Epost: ");
                        UserStore.Save(allUser);

                        break;
                    case "3":
                        Console.WriteLine($"Nuvarande uppgifter: {loggedInUser.FullName}");
                        loggedInUser.Phone = Ui.Ask("Nytt mobilnummer: ");
                        UserStore.Save(allUser);

                        break;
                    case "0":
                        return;
                    default:
                        Ui.Error("Fel val, försök igen.");
                        break;

                }

            }

        }
    }
}
