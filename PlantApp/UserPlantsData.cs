using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using PlantApp.Domain;


namespace PlantApp
{
    partial class DataAccess
    {
        public List<UserPlant> ShowAllUserPlantsList()
        {
            List<UserPlant> list = new List<UserPlant>();

            var sql = @"
SELECT UserPlantId, Plant.Name, Location.Location, UserPlants.WaterFrekuenseInDays, Soil.SoilType, Nutrition.NutritionType, BoughtDate, Comment, UserPlants.UserId, [User].UserName
FROM UserPlants
INNER JOIN Plant ON UserPlants.PlantId=Plant.PlantId
INNER JOIN Location ON UserPlants.LocationId=Location.LocationId
INNER JOIN Soil ON UserPlants.SoildId=Soil.SoilId
INNER JOIN Nutrition ON UserPlants.NutritionId=Nutrition.NutritionId
INNER JOIN [User] ON UserPlants.UserId=[User].UserId
";
            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var up = new UserPlant
                    {
                        UserPlantId = reader.GetSqlInt32(0).Value,
                        Name = reader.GetSqlString(1).Value,
                        Location = reader.GetSqlString(2).Value,
                        WaterFrequence = reader.GetSqlInt32(3).Value,
                        Soil = reader.GetSqlString(4).Value,
                        Nutrition = reader.GetSqlString(5).Value,
                        Bought = reader.GetSqlDateTime(6).Value,
                        UserInfo = reader.GetSqlString(7).Value,
                        UserId = reader.GetSqlInt32(8).Value,
                        UserName = reader.GetSqlString(9).Value,
                    };
                    list.Add(up);

                }
                return list;
            }

        }
        public List<UserPlant> ShowAllPlantsOnUser(int userId)
        {
            List<UserPlant> list = new List<UserPlant>();

            var sql = @"
SELECT UserPlantId, Plant.Name, Location.Location, UserPlants.WaterFrekuenseInDays, Soil.SoilType, Nutrition.NutritionType, BoughtDate, Comment, UserPlants.UserId, [User].UserName, LastWater
FROM UserPlants
INNER JOIN Plant ON UserPlants.PlantId=Plant.PlantId
INNER JOIN Location ON UserPlants.LocationId=Location.LocationId
INNER JOIN Soil ON UserPlants.SoildId=Soil.SoilId
INNER JOIN Nutrition ON UserPlants.NutritionId=Nutrition.NutritionId
INNER JOIN [User] ON UserPlants.UserId=[User].UserId
WHERE UserPlants.UserId=@UserId
";
            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("UserId", userId));

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var up = new UserPlant
                    {
                        UserPlantId = reader.GetSqlInt32(0).Value,
                        Name = reader.GetSqlString(1).Value,
                        Location = reader.GetSqlString(2).Value,
                        WaterFrequence = reader.GetSqlInt32(3).Value,
                        Soil = reader.GetSqlString(4).Value,
                        Nutrition = reader.GetSqlString(5).Value,
                        Bought = reader.GetSqlDateTime(6).Value,
                        UserInfo = reader.GetSqlString(7).Value,
                        UserId = reader.GetSqlInt32(8).Value,
                        UserName = reader.GetSqlString(9).Value,
                        LastWatered = reader.GetSqlDateTime(10).Value,
                    };
                    list.Add(up);

                }
                return list;
            }
        }

        public UserPlant FindPlantOnId(int UserPlantId)
        {
            var sql = @"
SELECT Plant.Name, Location.Location, UserPlants.WaterFrekuenseInDays, Soil.SoilType, Nutrition.NutritionType, BoughtDate, Comment, UserPlants.UserId, [User].UserName, LastWater
FROM UserPlants
INNER JOIN Plant ON UserPlants.PlantId=Plant.PlantId
INNER JOIN Location ON UserPlants.LocationId=Location.LocationId
INNER JOIN Soil ON UserPlants.SoildId=Soil.SoilId
INNER JOIN Nutrition ON UserPlants.NutritionId=Nutrition.NutritionId
INNER JOIN [User] ON UserPlants.UserId=[User].UserId
WHERE UserPlants.UserPlantId=@UserPlantId
";
            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("UserPlantId", UserPlantId));

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var up = new UserPlant
                    {
                        UserPlantId = reader.GetSqlInt32(0).Value,
                        Name = reader.GetSqlString(1).Value,
                        Location = reader.GetSqlString(2).Value,
                        WaterFrequence = reader.GetSqlInt32(3).Value,
                        Soil = reader.GetSqlString(4).Value,
                        Nutrition = reader.GetSqlString(5).Value,
                        Bought = reader.GetSqlDateTime(6).Value,
                        UserInfo = reader.GetSqlString(7).Value,
                        UserId = reader.GetSqlInt32(8).Value,
                        UserName = reader.GetSqlString(9).Value,
                        LastWatered = reader.GetSqlDateTime(10).Value,
                    };
                    return up;
                }
                else
                    return null;
            }
        }
        public void DeleteUserPlant(int UserPlantId)
        {
            var sql = @"DELETE FROM UserPlants WHERE UserPlantId=@UserPlantId";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("UserPlantId", UserPlantId));
                command.ExecuteNonQuery();
            }

        }


        public void UpdateUserPlant(UserPlant newUserPlant)
        {
            var sql = @"UPDATE UserPlants 
                    SET WaterFrekuenseInDays=@WaterFrekuenseInDays, Comment=@Comment, LastWater=@LastWater 
                    WHERE UserPlantId=@UserPlantId";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("UserPlantId", newUserPlant.UserPlantId));
                command.Parameters.Add(new SqlParameter("WaterFrekuenseInDays", newUserPlant.WaterFrequence));
                command.Parameters.Add(new SqlParameter("Comment", newUserPlant.UserInfo));
                command.Parameters.Add(new SqlParameter("LastWater", newUserPlant.LastWatered));

                command.ExecuteNonQuery();

            }
        }

        public void CreateNewUserPlant(Plant plant1, int UserId, DateTime bought, string comment)
        {
            
            var sql = @"INSERT INTO UserPlants(PlantId, LocationId, WaterFrekuenseInDays, SoildId, NutritionId, BoughtDate, Comment, UserId, LastWater) 
                        VALUES(@PlantId, @LocationId, @WaterFrekuenseInDays, @SoildId, @NutritionId, @BoughtDate, @Comment, @UserId, @LastWater)";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("PlantId", plant1.PlantId));
                command.Parameters.Add(new SqlParameter("LocationId", plant1.LocationId));
                command.Parameters.Add(new SqlParameter("WaterFrekuenseInDays", plant1.WaterFrekuenseInDays));
                command.Parameters.Add(new SqlParameter("SoildId", plant1.SoilId));
                command.Parameters.Add(new SqlParameter("NutritionId", plant1.NutritionId));
                command.Parameters.Add(new SqlParameter("BoughtDate", bought));
                command.Parameters.Add(new SqlParameter("Comment", comment));
                command.Parameters.Add(new SqlParameter("UserId", UserId));
                command.Parameters.Add(new SqlParameter("LastWater", DateTime.Now));


                command.ExecuteNonQuery();

            }


        }
    }
}




