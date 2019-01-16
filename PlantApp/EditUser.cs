using System;
using System.Collections.Generic;
using System.Text;
using PlantApp.Domain;


namespace PlantApp
{
   partial class App
    {
       private void ShowUserInformation()
        {
            Header("Användarinformation");
            
            WriteLine($"\nAnvändarnamn: {loggedOnUser.UserName}");
            WriteLine($"Email: {loggedOnUser.Email}");
            Console.WriteLine();

            WriteLine("Vill du..?\n" +
                "a) Ändra e-mail\n" +
                "b) Byta lösenord\n" +
                "c) Gå tillbaka\n");
            User editUser = loggedOnUser;


            ConsoleKey key2 = Console.ReadKey(true).Key;

            if (key2 == ConsoleKey.A)
            {
                WriteLine("Ange ny e-mailadress:");
                editUser.Email = Console.ReadLine();
                _dataAccess.EditUser(editUser);
                Console.Clear();
                WriteLine("Uppdaterat!");
                Console.ReadKey();
                SeeUserPlantsMenu();
            }

            if (key2 == ConsoleKey.B)
            {
                WriteLine("Ange nytt lösenord:");
                editUser.PassWord = Console.ReadLine();
                _dataAccess.EditUser(editUser);
                Console.Clear();
                WriteLine("Uppdaterat!");
                Console.ReadKey();
                SeeUserPlantsMenu();
            }

            if (key2 == ConsoleKey.C)
            {
                MainMenu();
            }

            Console.ReadKey();
            SeeUserPlantsMenu();
            
        } 
    }
}
