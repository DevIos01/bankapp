using System;
using System.Collections.Generic;
using System.Text;

namespace Bankapp
{
    public static class UserMenu
    {
        public static void EditKyc(User loggedInUser, List  <User> allUser)
            {
            while (true)
            {
                Console.WriteLine("──────────────────────────────");
                Console.WriteLine("  [1] Ändra Namn");
                Console.WriteLine("  [2] Ändra Epost");
                Console.WriteLine("  [0] Avsluta");
                Console.WriteLine("──────────────────────────────");
                Console.Write("Val: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        loggedInUser.FullName = Ui.Ask("Nytt namn: ");

                        break;
                    case "2":
                        loggedInUser.Email = Ui.Ask("Ny Epost");
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
