using System;
using System.Collections.Generic;
using System.Text;

namespace PlantApp.Domain
{
    class User
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Email { get; set; }
        public int UserLevelId { get; set; }
        public int ZoneId { get; set; }
        public int UserLocationId { get; set; }
        public int UserId { get; set; }
    }
}
