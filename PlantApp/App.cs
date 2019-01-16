using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlantApp.Domain;

namespace PlantApp
{
    partial class App
    {
        public string passWord;
        public string userName;
        public string email;

        DataAccess _dataAccess = new DataAccess();
        public User loggedOnUser = new User();

        internal void Run()
        {
            Console.Title = "PlantBook";
            Login();
            string user1 = loggedOnUser.UserName;

        }

        private void Login()
        {
            Header("PlantBook");
            WriteLine("Inloggning");
            WriteLine("a) Logga in i PlantBook");
            WriteLine("b) Skapa nytt användarkonto.");
            ConsoleKey command = Console.ReadKey(true).Key;

            if(command == ConsoleKey.A)
            {
                UserLoggIn();
            }
            if(command == ConsoleKey.B)
            {
                CreateAccount();
                MainMenu();
            }
            else
            {
                WriteLine("Fel val. Välj a eller b.");
                Login();
            }
                

        }

        private void CreateAccount()
        {
            while (true)
            {
                Write("Ange ett användarnamn: ");
                userName = Console.ReadLine();
                WriteLine("");
                loggedOnUser.UserName = userName;
                bool testUserName = _dataAccess.TestOfUserName(loggedOnUser);

                if (testUserName == false)
                {
                    Write("Ange ett lösenord: ");
                    passWord = "";
                    while (true)
                    {

                        var key = System.Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Enter)
                            break;                        
                        passWord += key.KeyChar;
                        Console.Write("*");
                    }
                    WriteLine("");
                    Write("Ange en e-post: ");
                    email = Console.ReadLine(); // lägg till validering av e-post.
                    WriteLine("Välj hur bostads exponeras mot solen");
                    List<Location> apparmentTypes = _dataAccess.GetAppartmentTypes();

                    foreach (var location in apparmentTypes)
                    {
                        WriteLine(location.LocationId.ToString().PadLeft(5) + location.LocationIn.PadLeft(20));
                    }

                    Write("Exponering: ");
                    loggedOnUser.UserLocationId = int.Parse(Console.ReadLine());
                    WriteLine("I vilken klimatzon bor du?");
                    List<Zone> zoneTypes = _dataAccess.GetZoneTypes();

                    foreach (var zone in zoneTypes)
                    {
                        WriteLine(zone.OriginId.ToString().PadLeft(5) + zone.ZoneIn.PadLeft(20));
                    }

                    Write("Boendezon: ");
                    loggedOnUser.ZoneId = int.Parse(Console.ReadLine());
                    loggedOnUser.PassWord = passWord;
                    loggedOnUser.Email = email;
                    break;
                }
                else
                {
                    WriteLine("Användarnamnet är upptaget, välj ett nytt.");
                }
            }
            _dataAccess.CreateNewAccount(loggedOnUser);
            loggedOnUser = _dataAccess.GetUserData(userName);
            WelcomeToPlantBook(loggedOnUser);
            
        }

        private void UserLoggIn()
        {
            while (true)
            {
                passWord = "";
                WriteLine("Logga in i applikationen");
                Write("Användarnamn: ");
                userName = Console.ReadLine();
                Write("Lösenord: ");
                while (true)
                {

                    var key = System.Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                        break;
                    passWord += key.KeyChar;
                    Console.Write("*");
                }
                loggedOnUser.UserName = userName;
                loggedOnUser.PassWord = passWord;
                bool userValid = _dataAccess.CheckIfUserIsValid(loggedOnUser);
                if (userValid == false)
                {
                    WriteLine("Användaren finns inte eller så är användarnamn eller lösenord felaktiga.");
                    WriteLine("För att få information om lösenord kontakta admin.");
                    WriteLine("Vill du fösöka logga in igen tryck I annars valfri tangent.");
                    ConsoleKey loggInCommand = Console.ReadKey(true).Key;
                    if (loggInCommand != ConsoleKey.I)
                    {
                        break;
                    }
                }
                if (userValid == true)
                {
                    loggedOnUser = _dataAccess.GetUserData(userName);
                    WelcomeToPlantBook(loggedOnUser);
                    MainMenu();
                    break;
                }
            }
        }

        private void WelcomeToPlantBook(User loggedOnUser)
        {
            Header("PlantBook");
            WriteLine("");
            WriteLine($"Hej {loggedOnUser.UserName}!");
            WriteLine("Du har loggat in i PlantBook.");
            WriteLine("Tryck på valfri tangent för att fortsätta.");
            Console.ReadKey();
        }

        private void MainMenu()
        {
            Header("HuvudMeny");
            WriteLine("a) Visa plantorna i databasen\n" +
                "b) Visa användarplantor\n" +
                "c) Friordssök på plantor\n" +
                "d) Avsluta");


            ConsoleKey key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.A)
                ShowPlantsMenu();

            if (key == ConsoleKey.B)
                SeeUserPlantsMenu();

            if (key == ConsoleKey.C)
                SearchPlantOnWord();

            if (key == ConsoleKey.D)
                Environment.Exit(0);


            else
            {
                WriteLine("Nu blev det fel!");
                Console.ReadKey();
                MainMenu();
            }
        }

        private void SearchPlantOnWord()
        {
            Header("Sök i PlantBook");
            WriteLine("Sök med valfritt ord i växtdatabasen");
            Write("Sökord: ");
            string searchWord = Console.ReadLine();
            List<Plant> searchPlantList = _dataAccess.SearchWithWord(searchWord);

            foreach (var plant in searchPlantList)
            {
                WriteLine(plant.PlantId.ToString().PadRight(5) + plant.Name.PadRight(30));
            }

            Write("Välj växt du vill se: ");
            List<Plant> singelPlant = _dataAccess.GetSinglePlant();
            PrintSinglePlantAndMenu(singelPlant);
            
        }


        public void SeeUserPlantsMenu()
        {
            Header("Användarväxter");
            WriteLine("a) Visa mina växter\n" +
                "b) Se andras växter\n" +
                "c) Användaruppgifter\n" +
                "d) Lägg till planta\n" +
                "e) Gå tillbaka");


            ConsoleKey key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.A)
            {
                ShowPlantsOnUser();
            }
            if (key == ConsoleKey.B)
            {
                ShowAllUserPlants();
            }
            if (key == ConsoleKey.C)
            {
                ShowUserInformation();
            }

            if (key == ConsoleKey.D)
            {
                AddUserPlant();
            }

            if (key == ConsoleKey.E)
            {
                MainMenu();
            }

            else
            {

                Console.ReadKey();
                MainMenu();
            }

        }

        private void ShowPlantsMenu()
        {
            Header("Visa växter");
            WriteLine("a) Visa alla plantor sorterat på namn\n" +
                "b) Visa efter kategori\n" +
                "c) Lägg till planta\n" +
                "d) Gå tillbaka");


            ConsoleKey key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.A)
            {
                ShowAllPlantsOnName();
            }

            if (key == ConsoleKey.B)
            {
                ShowOnCategory();
            }

            if (key == ConsoleKey.C)
            {
                AddPlant();
            }

            if (key == ConsoleKey.D)
            {
                MainMenu();
            }

            else
            {
                WriteLine("Nu blev det fel!");
                Console.ReadKey();
                MainMenu();
            }
            
        }       

        public void WriteLine(string text)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(text);
            Console.ResetColor();

        }
        public void Write(string text)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(text);
            Console.ResetColor();

        }
        public void Header(string text)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text.ToUpper());
            Console.WriteLine();
            Console.ResetColor();
        }
        public void PrintGreenText(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text.ToUpper());
            Console.WriteLine();
            Console.ResetColor();
        }

        public void PrintSinglePlantAndMenu(List<Plant> singlePlant)
        {
            Header("Info on plant");
            PrintGreenText("Plantans ID".PadRight(30) + "Plantans Namn".PadRight(30) + "Latinska namnet".PadRight(30) + "Vattnas varje x:e dag".PadRight(30) + "Info".PadRight(30));

            foreach (Plant bp in singlePlant)
            {
                Console.WriteLine(bp.PlantId.ToString().PadRight(30) + bp.Name.PadRight(30) + bp.LatinName.PadRight(30) + bp.WaterFrekuenseInDays.ToString().PadRight(30) + bp.GeneralInfo.PadRight(30));
            }
            Console.WriteLine("");


            var firstElement = singlePlant.First().Name;

            PrintGreenText("Vad vill du göra med " + firstElement + "en?");
            WriteLine("a) Google efter växten");
            WriteLine("b) Lägg till en kommentar");
            WriteLine("c) Uppdatera information om växt");
            WriteLine("d) Ta bort växt");
            WriteLine("e) Visa kommentarer");
            WriteLine("f) Gå till huvudmenyn");
            while (true)
            {
                ConsoleKey input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.A)
                {
                    GoogleThePlantPlease(singlePlant);
                    break;
                }
                if (input == ConsoleKey.B)
                {
                    AddACommentToPlant(singlePlant);
                    break;
                }
                if (input == ConsoleKey.C)
                {
                    UpDatePlantInfo(singlePlant[0].PlantId);
                    break;
                }

                if (input == ConsoleKey.D)
                {
                    DeletePlant(singlePlant);
                    break;
                }
                if (input == ConsoleKey.E)
                {
                    ShowComment(singlePlant);
                    break;
                }
                if (input == ConsoleKey.F)
                {
                    MainMenu();
                }
            }
        }

        private void DeletePlant(List<Plant> singlePlant)
        {
            List<Plant> plant = _dataAccess.GetAllPlantSorted();
            var sortedList = plant.OrderBy(x => x.Name).ToList();
            Header("Ta bort växt");
            PrintGreenText("Plantans Id".PadRight(30) + "Plantans namn".PadRight(5));
            foreach (Plant bp in sortedList)
            {
                WriteLine(bp.PlantId.ToString().PadRight(30) + bp.Name.PadRight(5));
            }
            WriteLine("");
            Write("Ange växt som ska tas bort: ");
            bool existPlant = int.TryParse(Console.ReadLine(), out int plantId);
            _dataAccess.DeletePlantInTable(plantId);
            Header("PlantBook");
            WriteLine($"Växt är bortagen.");
            Console.ReadKey();
        }
    }
}
