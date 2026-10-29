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
                Ui.Header("MINA SIDOR", $"Inloggad som {loggedInUser.Username}");
                Ui.Option("1", "Ändra namn");
                Ui.Option("2", "Ändra e-post");
                Ui.Option("3", "Ändra telefon");
                Ui.Option("0", "Logga ut");
                string choice = Ui.Choice();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine();
                        Console.WriteLine($"  Nuvarande: {loggedInUser.FullName}");
                        loggedInUser.FullName = Ui.Ask("  Nytt namn: ");
                        UserStore.Save(allUser);
                        Ui.Success("Sparat!");
                        Ui.Pause();
                        break;
                    case "2":
                        Console.WriteLine();
                        Console.WriteLine($"  Nuvarande: {loggedInUser.Email}");
                        loggedInUser.Email = Ui.AskEmail("  Ny e-post: ");
                        UserStore.Save(allUser);
                        Ui.Success("Sparat!");
                        Ui.Pause();
                        break;
                    case "3":
                        Console.WriteLine();
                        Console.WriteLine($"  Nuvarande: {loggedInUser.Phone}");
                        loggedInUser.Phone = Ui.Ask("  Ny telefon: ");
                        UserStore.Save(allUser);
                        Ui.Success("Sparat!");
                        Ui.Pause();
                        break;
                    case "0":
                        return;
                    default:
                        Ui.Error("Fel val, försök igen.");
                        Ui.Pause();
                        break;

                }

            }

        }
    }
}
