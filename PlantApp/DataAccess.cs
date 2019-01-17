using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using PlantApp.Domain;

namespace PlantApp
{
    partial class DataAccess
    {
        private const string conString = "Server=(localdb)\\mssqllocaldb; Database=Plants";
        //private const string conString = "Server=tcp:academygbg.database.windows.net,1433;Initial Catalog=PlantBook;Persist Security Info=False;User ID=Tobias;Password=Password1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

        public List<Plant> GetAllPlantSorted()
        {
            var sql = @"SELECT PlantId, Name FROM Plant";
            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                var list = new List<Plant>();
                while (reader.Read())
                {
                    var tempplant = new Plant      //TODO: Möjligen en otydlig variabelnamn
                    {
                        PlantId = reader.GetSqlInt32(0).Value,
                        Name = reader.GetSqlString(1).Value,
                    };
                    list.Add(tempplant);
                }
                return list;
            }
        }

        internal bool CheckIfUserIsValid(User loggedOnUser)
        {
            bool userExist = false;
            var sql = "SELECT COUNT(*) FROM [User] WHERE UserName = @userName AND PassWord = @passWord";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("userName", loggedOnUser.UserName));
                command.Parameters.Add(new SqlParameter("passWord", loggedOnUser.PassWord));
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int countTag = reader.GetSqlInt32(0).Value;
                    if (countTag == 1)
                        userExist = true;
                }
            }
            return userExist;
        }
        
        internal List<Zone> GetZoneTypes()
        {
            List<Zone> types = new List<Zone>();

            var sql = @"SELECT * FROM Origin";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Zone zone = new Zone();
                    {
                        zone.OriginId = reader.GetSqlInt32(0).Value;
                        zone.ZoneIn = reader.GetSqlString(1).Value;
                    }
                    types.Add(zone);
                }
            }
            return types;
        }

        public List<PlantComment> ShowComment(Plant onlyOne) //TODO onlyOne används ej i metoden och behövs därför inte skickas med
        {
            var sql = @"select comment, [user].UserName from Comment 
                       inner join [User] on Comment.UserID=[user].UserId
                       where Comment.PlantId=PlantId";

            List<PlantComment> types = new List<PlantComment>();


            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    PlantComment comment = new PlantComment();
                    {
                        comment.CommentFromUser = reader.GetSqlString(0).Value;
                        comment.UserComment = reader.GetSqlString(1).Value;
                    }   
                    types.Add(comment);
                }
            }
            return types;

        }

        internal void AddComment(Plant onlyOne, string comment, User loggedOnUser)
        {
            DateTime dateNow = DateTime.Now;
            var sql = @"INSERT INTO Comment (Comment, UserID, Time, Likes, CommentTypeId, PlantId) 
                       Values (@comment, @UserID, @dateNow, 0, 1, @PlantId)";
            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("comment", comment));
                command.Parameters.Add(new SqlParameter("dateNow", dateNow));
                command.Parameters.Add(new SqlParameter("UserID", loggedOnUser.UserId));
                command.Parameters.Add(new SqlParameter("PlantId", onlyOne.PlantId));
                command.ExecuteNonQuery();
            }
        }

        internal List<Location> GetAppartmentTypes()
        {
            List<Location> types = new List<Location>();

            var sql = @"SELECT * FROM Location";

            using (SqlConnection connection = new SqlConnection(conString))
            using(SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Location location = new Location();
                    {
                        location.LocationId = reader.GetSqlInt32(0).Value;
                        location.LocationIn = reader.GetSqlString(1).Value;
                    }
                    types.Add(location);
                }
            }
            return types;
        }

        internal void UpdateName(string newName, string columnName, int plantId)
        {
            var sql = $@"UPDATE Plant SET {columnName} = @newName WHERE PlantId = @plantId";

            using (SqlConnection connection = new SqlConnection(conString))
            using(SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("newName", newName));
                command.Parameters.Add(new SqlParameter("plantId", plantId));
                command.ExecuteNonQuery();
            }
        }

        internal List<Plant> GetSinglePlant(int plantId)
        {
            var sql = @"SELECT * FROM Plant where PlantId=@input";
            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command2 = new SqlCommand(sql, connection))
            {
                connection.Open();
                command2.Parameters.Add(new SqlParameter("input", plantId));
                SqlDataReader reader = command2.ExecuteReader();
                var list = new List<Plant>();
                while (reader.Read())
                {
                    var bp = new Plant
                    {
                        Name = reader.GetSqlString(0).Value.ToString(),
                        PlantId = reader.GetSqlInt32(1).Value,
                        LatinName = reader.GetSqlString(2).Value,
                        LocationId = reader.GetSqlInt32(3).Value,
                        WaterFrekuenseInDays = reader.GetSqlInt32(4).Value,
                        PlantTypeId = reader.GetSqlInt32(5).Value,
                        ScentId = reader.GetSqlInt32(6).Value,
                        SoilId = reader.GetSqlInt32(7).Value,
                        NutritionId = reader.GetSqlInt32(8).Value,
                        OriginId = reader.GetSqlInt32(9).Value,
                        PoisonId = reader.GetSqlInt32(10).Value,
                        GeneralInfo = reader.GetSqlString(11).Value
                    };
                    list.Add(bp);
                }
                return list;
            }
        }

        internal void CreateNewAccount(User loggedOnUser)
        {
            var sql = @"INSERT INTO [User] (UserName, PassWord, Email, UserLevelId, ZoneId, UserLocationId) VALUES (@userName, @passWord, @email, 1, @zoneId, @locationID)";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("userName", loggedOnUser.UserName));
                command.Parameters.Add(new SqlParameter("passWord", loggedOnUser.PassWord));
                command.Parameters.Add(new SqlParameter("email", loggedOnUser.Email));
                command.Parameters.Add(new SqlParameter("zoneId", loggedOnUser.ZoneId));
                command.Parameters.Add(new SqlParameter("locationId", loggedOnUser.UserLocationId));
                command.ExecuteNonQuery();   
            }
        }

        internal List<Plant> SearchWithWord(string searchWord)
        {
            List<Plant> plantList = new List<Plant>();
            var sql = @"SELECT PlantId, [Name] FROM Plant WHERE Name LIKE '%' + @search + '%'";

            using (SqlConnection connection = new SqlConnection(conString))
            using(SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("search", searchWord));
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Plant plant = new Plant()
                    {
                        PlantId = reader.GetSqlInt32(0).Value,
                        Name = reader.GetSqlString(1).Value,
                    };
                    plantList.Add(plant);
                }
            }
            return plantList;
        }

        internal void AddPlant(Plant added)
        {
            var sql = @"insert into Plant (Name, LatinName, WaterFrekuenseInDays, LocationId, PlantTypeId, ScentId, SoilId, NutritionId, OriginId, PoisonId, GeneralInfo) Values (@Name, @LatinName, @WaterFrekuenseInDays, 1, 1, 1, 1, 1, 1, 1, @GeneralInfo)";
            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("Name", added.Name));
                command.Parameters.Add(new SqlParameter("LatinName", added.LatinName));
                command.Parameters.Add(new SqlParameter("WaterFrekuenseInDays", added.WaterFrekuenseInDays));
                command.Parameters.Add(new SqlParameter("GeneralInfo", added.GeneralInfo));          
                command.ExecuteNonQuery();
            }
        }
        
        internal bool TestOfUserName(User loggedOnUser)
        {
            bool testUser = true;
            var sql = @"SELECT COUNT(*) FROM [User] WHERE UserName = @userName";

            using (SqlConnection connection = new SqlConnection(conString))
            using(SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("userName", loggedOnUser.UserName));
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    int countTag = reader.GetSqlInt32(0).Value;
                    if (countTag == 0)
                        testUser = false;
                }
            }
            return testUser;
        }

        internal User GetUserData(string userName)
        {
            User user = new User();

            var Sql = @"SELECT * FROM [User] WHERE UserName = @userName";

            using (SqlConnection connection = new SqlConnection(conString))
            using(SqlCommand command = new SqlCommand(Sql, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("userName", userName));
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    user.UserId = reader.GetSqlInt32(0).Value;
                    user.UserName = reader.GetSqlString(1).Value;
                    user.PassWord = reader.GetSqlString(2).Value;
                    user.UserLocationId = reader.GetSqlInt32(3).Value;
                    user.ZoneId = reader.GetSqlInt32(4).Value;
                    user.UserLevelId = reader.GetSqlInt32(5).Value;
                    user.Email = reader.GetSqlString(6).Value;
                }
            }
            return user;
        }

        //public Plant GetPlantByCategory(int input)
        //{
        //    var sql = @"SELECT PlantId, Name
        //                FROM Plant 
        //                WHERE PlantTypeId=@input";
        //    using (SqlConnection connection = new SqlConnection(conString))
        //    using (SqlCommand command = new SqlCommand(sql, connection))
        //    {
        //        connection.Open();

        public List<Plant> GetPlantByCategory(int input)
        {
            var sql = @"SELECT PlantId, Name, PlantType.PlantType
                        FROM Plant 
                        inner join PlantType on Plant.PlantTypeId=PlantType.PlantTypeId
                        WHERE plant.PlantTypeId=@InputId";
            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("InputId", input));
                SqlDataReader reader = command.ExecuteReader();

                var list = new List<Plant>();

                while (reader.Read())
                {
                    var bp = new Plant
                    {
                        PlantId = reader.GetSqlInt32(0).Value,
                        Name = reader.GetSqlString(1).Value,
                    };
                    list.Add(bp);
                }
                return list;
            }
        }

        internal List<PlantType> GetCategort()
        {
            var sql = @"SELECT PlantTypeId, PlantType FROM PlantType";
            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                var list = new List<PlantType>();
                while (reader.Read())
                {
                    var bp = new PlantType
                    {
                        PlantTypeId = reader.GetSqlInt32(0).Value,
                        PlantTypes = reader.GetSqlString(1).Value,

                    };
                    list.Add(bp);
                }
                return list;
            }
        }

        internal void DeletePlantInTable(int plantId)
        {
            var sql = @"DELETE FROM PlantToLocation WHERE PlantId = @plantId
                        DELETE FROM Comment WHERE PlantId = @plantId
                        DELETE FROM UserPlants WHERE PlantId = @plantId
                        DELETE FROM Plant WHERE PlantId = @plantId";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("plantId", plantId));
                command.ExecuteNonQuery();
            }
        }
    }
}
