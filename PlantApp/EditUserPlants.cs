using System;
using System.Collections.Generic;
using System.Text;
using PlantApp.Domain;


namespace PlantApp
{
    partial class App
    {

       private void ShowAllUserPlants()
        {
            Header("Alla användarplantor");

           List<UserPlant> AllUserPlants = _dataAccess.ShowAllUserPlantsList();

            foreach (var plant in AllUserPlants)
            {
                WriteLine(plant.Name);
                WriteLine("Användare: " + plant.UserName);
                WriteLine("Inköpsdag: " + plant.Bought);
                WriteLine($"Info från {plant.UserName}: " +
                    $"{plant.UserInfo}");
                Console.WriteLine();
            }
            Console.ReadKey();
            SeeUserPlantsMenu();
        }

       private void ShowPlantsOnUser()
        {
            Header("Alla Dina Plantor");

            List<UserPlant> AllUserPlants = _dataAccess.ShowAllPlantsOnUser(loggedOnUser.UserId);

            foreach (var plant in AllUserPlants)
            {
                TimeSpan t = CalculateWaterDay(plant.LastWatered, plant.WaterFrequence);
                string daysTilWater = DisplayDaysTilWater(t);
                WriteLine(plant.Name);
                WriteLine("Användare: " + plant.UserName);
                WriteLine("Inköpsdag: " + plant.Bought);
                WriteLine("Vattnas var " + plant.WaterFrequence + " dag");
                WriteLine(daysTilWater);
                WriteLine($"Info från {plant.UserName}: " +
                    $"{plant.UserInfo}\n");
            }

            WriteLine("");
            WriteLine("a) Uppdatera Planta");
            WriteLine("b) Vattna Planta");
            WriteLine("c) Gå tillbaka");


            ConsoleKey key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.A)
            {
                UpdateUserPlant(AllUserPlants);
            }

            if (key == ConsoleKey.B)
            {
                WaterPlantQuestion(AllUserPlants);
            }


            if (key == ConsoleKey.C)
            {
                SeeUserPlantsMenu();
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


        private TimeSpan CalculateWaterDay(DateTime wateredDay, int Waterfrequence)
        {

            DateTime today = DateTime.Now;
            DateTime daytowater = wateredDay.AddDays(Waterfrequence);
            TimeSpan t = daytowater - today;

            return t;

        }

        private string DisplayDaysTilWater(TimeSpan t)
        {
            string countDown;

            if (t.Days >= 5)
            {

                countDown = $"{t.Days} dagar till vattning. Lungt!";
                return countDown;
            }

            if (t.Days < 2 && t.Days > 0)
            {
                countDown = $"{t.Days} dagar till vattning!";
                return countDown;
            }

            if (t.Days == 0)
            {
                countDown = $"Idag ska den vattnas.";
                return countDown;
            }

            if (t.Days < 0)
            {

                countDown = $"Åh, nej! Du skulle vattnat för {t.Days * -1} dagar sedan!";
                return countDown;
            }


            else
            {
                countDown = "Den har vattnats idag :)";
                return countDown;
            }
        }

        private void UpdateUserPlant(List<UserPlant> list)
        {
            Header("Uppdatera planta");
            foreach (var plant in list)
            {
                WriteLine("Id: " + plant.UserPlantId);
                WriteLine("Namn: " + plant.Name);
            }

            WriteLine("Vilken Planta vill du uppdatera?");
            int uppdPlant = int.Parse(Console.ReadLine());
            UserPlant PlantToUppdate = SortOutUserPlant(list, uppdPlant);

            Header("Uppdaterar " + PlantToUppdate.Name);

            UserPlant newUserPlant = PlantToUppdate;
            WriteLine("Vad vill du uppdatera?");
            WriteLine("a) Placering");
            WriteLine("b) Dagar mellan vattning");
            WriteLine("c) Uppdatera informationen");
            WriteLine("d) Vattna");
            WriteLine("e) Ta bort Planta");
            WriteLine("f) Tillbaka");

            ConsoleKey key2 = Console.ReadKey(true).Key;

            if (key2 == ConsoleKey.A)
            {
                WriteLine("Här kan man uppdatera väderstreck sen");
                WriteLine("Uppdaterat!");
                Console.ReadKey();
                SeeUserPlantsMenu();
            }


            if (key2 == ConsoleKey.B)
            {
                Console.WriteLine();
                WriteLine("Hur många dagar vill du ha mellan vattning?");

                newUserPlant.WaterFrequence = int.Parse(Console.ReadLine());

                _dataAccess.UpdateUserPlant(newUserPlant);
                Console.Clear();
                WriteLine("Uppdaterat!");
                Console.ReadKey();
                SeeUserPlantsMenu();
            }
            if (key2 == ConsoleKey.C)
            {
                Console.WriteLine();
                WriteLine("Ange ny information om plantan.");
                newUserPlant.UserInfo = Console.ReadLine();

                _dataAccess.UpdateUserPlant(newUserPlant);
                Console.Clear();
                WriteLine("Uppdaterat!");
                Console.ReadKey();
                SeeUserPlantsMenu();
            }

            if (key2 == ConsoleKey.D)
            {
                WaterPlant(newUserPlant);
            }


            if (key2 == ConsoleKey.E)
            {
                DeleteUserPlant(PlantToUppdate.UserPlantId);
            }
            if (key2 == ConsoleKey.F)
            {
                SeeUserPlantsMenu();
            }
            else
            {
                WriteLine("Nu blev det fel!");
                Console.ReadKey();
                MainMenu();
            }

        }

        private void WaterPlantQuestion(List<UserPlant> list)
        {
            Header("Vattna");
            Console.WriteLine();
            foreach (var plant in list)
            {
                WriteLine("Id: " + plant.UserPlantId);
                WriteLine("Namn: " + plant.Name);
            }

            Console.WriteLine();
            WriteLine("Vilken Planta vill du Vattna?");
            int uppdPlant = int.Parse(Console.ReadLine());
            UserPlant PlantToWater = SortOutUserPlant(list, uppdPlant);

            Console.Clear();
            Console.WriteLine();

            Console.BackgroundColor = ConsoleColor.DarkGreen;
            WriteLine("Vattnar blomman...");
            Console.ResetColor();
            Console.ReadKey();
            PlantToWater.LastWatered = DateTime.Now;

            _dataAccess.UpdateUserPlant(PlantToWater);

            Console.ReadKey();
            SeeUserPlantsMenu();


        }
        private void WaterPlant(UserPlant newUserPlant)
        {
            Console.Clear();
            Console.WriteLine();
            WriteLine("Vattnar blomman...");
            Console.ReadKey();
            newUserPlant.LastWatered = DateTime.Now;

            _dataAccess.UpdateUserPlant(newUserPlant);

            Console.ReadKey();
            SeeUserPlantsMenu();

        }


        private UserPlant SortOutUserPlant(List<UserPlant> list, int UserPlantId)
        {
            UserPlant newPlant = new UserPlant();
            foreach (var plant in list)
            {
                if (plant.UserPlantId == UserPlantId)
                {
                    newPlant = plant;
                }
                else
                {
                    WriteLine("Nu blev det fel");
                }
            }
            return newPlant;
        }

        private void AddUserPlant()
        {
            Header("Lägga till planta");
            WriteLine("Vilken planta vill du lägga till (ange id?");

           List<Plant> plantlist = _dataAccess.GetAllPlantSorted();

            foreach (var plant in plantlist)
            {
                WriteLine("Id: " + plant.PlantId + " " + plant.Name);
            }

            int plantToAdd = int.Parse(Console.ReadLine());
           List<Plant> plant1list = _dataAccess.GetSinglePlant(plantToAdd);

            Plant plant1 = new Plant();

            foreach (var plant in plant1list)
            {
                plant1 = plant;
            }

            Header($"Lägg till en {plant1.Name}");
            WriteLine("När köpte du plantan? (Ange i format MM/dd/yyyy) ");
            DateTime bought2 = DateTime.Parse(Console.ReadLine());
          //  DateTime bought2 = DateTime.Parse(bought1 + " 0:00");
            WriteLine("Lägg till en kommentar om din planta:");
            string comment = Console.ReadLine();

            _dataAccess.CreateNewUserPlant(plant1, loggedOnUser.UserId, bought2, comment);

            Console.Clear();

            WriteLine("Planta tillagd!");
            Console.ReadKey();
            SeeUserPlantsMenu();

        }

        private void DeleteUserPlant(int UserPlantId)
        {
            Header("Ta bort planta");
            WriteLine("Är du säker?");
            string yn = Console.ReadLine();


            if (yn.ToLower() == "ja")
            {

                _dataAccess.DeleteUserPlant(UserPlantId);

                WriteLine("Plantan är borttagen!");
                Console.ReadKey();
                SeeUserPlantsMenu();
            }

            else
            {
                WriteLine("Ingen planta har tagits bort");
                Console.ReadKey();
                SeeUserPlantsMenu();
            }
        }
    }
}
