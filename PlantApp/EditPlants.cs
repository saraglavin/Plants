using PlantApp.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PlantApp
{
    partial class App
    {
        private void ShowAllPlantsOnName()
        {
            Header("Här är listan på alla plantor i databasen");
            List<Plant> plant = _dataAccess.GetAllPlantSorted();
            var sortedList = plant.OrderBy(x => x.Name).ToList();
            PrintGreenText("Plantans Id".PadRight(30) + "Plantans namn".PadRight(5));
            foreach (Plant bp in sortedList)
            {
                WriteLine(bp.PlantId.ToString().PadRight(30) + bp.Name.PadRight(5));
            }
            WriteLine("");
            PrintGreenText("Vad vill du göra?");
            WriteLine("a) Välj en planta att arbeta med");
            WriteLine("b) Gå till huvudmenyn");
            while (true)
            {
                ConsoleKey command = Console.ReadKey(true).Key;

                if (command == ConsoleKey.A)
                {
                    PickAPlant(sortedList);
                    break;
                }
                if (command == ConsoleKey.B)
                {
                    MainMenu();
                }
                else
                {
                    Console.WriteLine("Sorry, wrong input...");
                }
            }

            ShowPlantsMenu();
        }

        private void PickAPlant(List<Plant> sortedList)
        {
            Header("Vilken planta vill du jobba med? Välj ett Id");
            foreach (Plant bp in sortedList)
            {
                WriteLine(bp.PlantId.ToString().PadRight(30) + bp.Name.PadRight(5));
            }
            Write("Plantan som ska väljas: ");
            List<Plant> singePlant = _dataAccess.GetSinglePlant();
            PrintSinglePlantAndMenu(singePlant);
            
        }
        private void UpDatePlantInfo(int plantId)
        {
            Header("Uppdatera plantans information");
            WriteLine("a) Namn");
            WriteLine("b) Latinska namnet");
            WriteLine("c) Växttyp");
            WriteLine("d) Vattningsfrekvens");
            WriteLine("e) Doft");
            WriteLine("f) Rekommenderade placeringar");
            WriteLine("g) Jordtyp");
            WriteLine("h) Näring");
            WriteLine("i) Giftighet");
            WriteLine("j) Generel informstion");

            ConsoleKey key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.A)
                UpdatePlantName(plantId, "namn", "Name");

            if (key == ConsoleKey.B)
                    UpdatePlantName(plantId, "latinska namn", "LatinName");

            //if (key == ConsoleKey.C)
            //    UpdatePlantType();

            if (key == ConsoleKey.D)
                UpdatePlantName(plantId, "vattningsfrekvens", "WaterFrekuenseInDays");

            //if (key == ConsoleKey.E)
            //    UpdatePlantSent();

            //if (key == ConsoleKey.F)
            //    UpdatePlantLocation();

            //if (key == ConsoleKey.G)
            //    UpdatePlantSoil();

            //if (key == ConsoleKey.H)
            //    UpdatePlantNutrition();

            //if (key == ConsoleKey.I)
            //    UpdatePlantPoison();

            if (key == ConsoleKey.J)
                UpdatePlantName(plantId, "information", "GeneralInfo");

            else
            {
                WriteLine("Nu blev det fel!");
                Console.ReadKey();
                //MainMenu();
            }

        }

        private void UpdatePlantName(int plantId, string name, string columnName)
        {
            List<Plant> singlePlant = _dataAccess.GetSinglePlant(plantId);
            WriteLine($"Nuvarande {name} på växten är: {singlePlant[0].GetProperty(columnName)}.");
            Write("Vad vill du ändra till: ");
            string newName = Console.ReadLine();
            _dataAccess.UpdateName(newName, columnName, singlePlant[0].PlantId);
            List<Plant> updatedSinglePlant = _dataAccess.GetSinglePlant(plantId);
            PrintSinglePlantAndMenu(updatedSinglePlant); 
        }

        private void ShowComment(List<Plant> singePlant)
        {
            Header("Visar kommentarer för: " + singePlant[0].Name);

            Plant onlyOne = new Plant();
            onlyOne.Name = singePlant[0].Name;
            onlyOne.PlantId = singePlant[0].PlantId;
            onlyOne.LatinName = singePlant[0].LatinName;
            onlyOne.LocationId = singePlant[0].LocationId;
            onlyOne.WaterFrekuenseInDays = singePlant[0].WaterFrekuenseInDays;
            onlyOne.PlantTypeId = singePlant[0].PlantTypeId;
            onlyOne.ScentId = singePlant[0].ScentId;
            onlyOne.NutritionId = singePlant[0].NutritionId;
            onlyOne.OriginId = singePlant[0].OriginId;
            onlyOne.PoisonId = singePlant[0].PoisonId;
            onlyOne.GeneralInfo = singePlant[0].GeneralInfo;
            List<PlantComment> plantcomment = _dataAccess.ShowComment(onlyOne);
            PrintGreenText("Kommentar".PadRight(100) + "Användare".PadRight(100));
            foreach (PlantComment item in plantcomment)
            {
                Console.WriteLine(item.CommentFromUser.PadRight(100) + item.UserComment.PadRight(100));
            }
            Console.WriteLine("Tryck på enter för att komma till huvudmenyn");
            Console.ReadKey();
        }

        private void GoogleThePlantPlease(List<Plant> singePlant)
        {
            var firstElement = singePlant.First().Name;
            try
            {
                Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", "https://www.google.se/search?q=" + firstElement); // Ska visa bilder på växten
                Console.WriteLine("");               
            }

            catch (Exception)
            {
                Console.WriteLine("Tyvärr funkar detta endast i Chrome");
                Console.WriteLine("");
            }
            Console.WriteLine("Klicka enter för att komma till förstasidan");
            Console.ReadKey();
            MainMenu();
        }

        private void AddACommentToPlant(List<Plant> singePlant)
        {
            Header("Lägg till kommentar om " + singePlant[0].Name);
            string comment = Console.ReadLine();
            Plant onlyOne = new Plant();
            onlyOne.Name = singePlant[0].Name;
            onlyOne.PlantId = singePlant[0].PlantId;
            onlyOne.LatinName = singePlant[0].LatinName;
            onlyOne.LocationId = singePlant[0].LocationId;
            onlyOne.WaterFrekuenseInDays = singePlant[0].WaterFrekuenseInDays;
            onlyOne.PlantTypeId = singePlant[0].PlantTypeId;
            onlyOne.ScentId = singePlant[0].ScentId;
            onlyOne.NutritionId = singePlant[0].NutritionId;
            onlyOne.OriginId = singePlant[0].OriginId;
            onlyOne.PoisonId = singePlant[0].PoisonId;
            onlyOne.GeneralInfo = singePlant[0].GeneralInfo;

            _dataAccess.AddComment(onlyOne, comment, loggedOnUser);
            Console.WriteLine("Tack för ditt bidrag, tryck på enter för att komma till huvudmenyn");
            Console.ReadKey();
            MainMenu();
        }

        private void WorkWithPlant(string command)
        {
            Console.WriteLine("Fixa detta");
            Console.ReadLine();

        }

        private void ShowOnCategory()
        {
            Header("Välj en kategori");
            List <PlantType> category = _dataAccess.GetCategort();
            PrintGreenText("Kategori Id".PadRight(30) + "Kategori".PadRight(5));

            foreach (PlantType bp in category)
            {
                WriteLine(bp.PlantTypeId.ToString().PadRight(30) + bp.PlantTypes.PadRight(5));
            }
            WriteLine("");
            int input = int.Parse(Console.ReadLine());
            List<Plant> plantCategory = _dataAccess.GetPlantByCategory(input);
            Console.Clear();
            Header("Visar alla plantor i den kategorin");
            PrintGreenText("Plantans Id".PadRight(30) + "Plantans namn".PadRight(5));

            foreach (Plant bp in plantCategory)
            {
                WriteLine(bp.PlantId.ToString().PadRight(30) + bp.Name.PadRight(5));
            }
            WriteLine("");
            Console.ReadKey();
            ShowPlantsMenu();

        }
        private void AddPlant()
        {
            int waterDate;
            Header("Lägg till planta i databsen");
            List<Plant> addPlantList = new List<Plant>();

            WriteLine("Vad är plantans namn på Svenska?");
            string nameOnPlant = Console.ReadLine();
            WriteLine("Vad är plantans latinska namn?");
            string latinName = Console.ReadLine();
            Console.WriteLine("Hur ofta ska plantan vattnas, skriv antal dagar (ex '5')");            
            waterDate = int.Parse(Console.ReadLine());            
            Console.WriteLine("Skriv lite info om plantan");
            string info = Console.ReadLine();

            Plant added = new Plant();
            added.Name = nameOnPlant;
            added.LatinName = latinName;
            added.GeneralInfo = info;
            added.WaterFrekuenseInDays = waterDate;
            _dataAccess.AddPlant(added);
            MainMenu();
        }
    }
}
