using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QLCuaHangBanSach.DAO
{
    internal class UserDAO
    {
        public string connectionString = @"Data Source=DESKTOP-UQ06DIF;Initial Catalog=SQLQuery3.sql;Integrated Security=True";

        public List<UserModel> getAllUser()
        {
            List<UserModel> listUserModel = new List<UserModel>();
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM users", sqlConnection);

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserModel User = new UserModel();
                        User.username = (string)reader.GetString(0);
                        User.password = (string)reader.GetString(1);

                        listUserModel.Add(User);
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Connect Fail: " + error);
            }
            finally
            {
                sqlConnection.Close();
            }
            return listUserModel;
        }
    }
}
