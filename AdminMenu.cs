using System;
using System.Collections.Generic;
using System.Text;

namespace Bankapp;

public static class AdminMenu
{
    public static void Show(List<User> allUsers)
    {
        while (true)
        {
            Console.WriteLine("[1] Skapa användare");
            Console.WriteLine("[2] Visa låsta användare / lås upp");
            Console.WriteLine("[0] Logga ut");
            Console.Write("Val: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    //CreateUser(allUsers);
                    break;

                case "2":
                   // UnlockUser(allUsers);
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
