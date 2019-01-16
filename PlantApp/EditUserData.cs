using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using PlantApp.Domain;

namespace PlantApp
{
    partial class DataAccess
    {

        public void EditUser(User x)
        {
            

            var sql = @"UPDATE [User]
                    SET PassWord=@PassWord, Email=@Email 
                    WHERE userId=@userId";

            using (SqlConnection connection = new SqlConnection(conString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("PassWord", x.PassWord));
                command.Parameters.Add(new SqlParameter("Email", x.Email));
                command.Parameters.Add(new SqlParameter("userId", x.UserId));

                command.ExecuteNonQuery();

            }

        }

    }
}
