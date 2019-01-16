using System;
using System.Collections.Generic;
using System.Text;

namespace PlantApp.Domain
{
    class UserPlant
    {
        public int UserPlantId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int WaterFrequence { get; set; }
        public string Soil { get; set; }
        public string Nutrition { get; set; }
        public DateTime Bought { get; set; }
        public string UserInfo { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime LastWatered { get; set; }


    }
}
