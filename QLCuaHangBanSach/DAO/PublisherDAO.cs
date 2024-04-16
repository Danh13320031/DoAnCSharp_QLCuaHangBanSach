using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QLCuaHangBanSach
{
    internal class PublisherDAO
    {
        public string connectionString = @"Data Source=DESKTOP-UQ06DIF;Initial Catalog=SQLQuery3.sql;Integrated Security=True";

        public List<PublisherModel> getAllPublisher()
        {
            List<PublisherModel> listPublisherModel = new List<PublisherModel>();
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM NhaXuatBan", sqlConnection);

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PublisherModel Publisher = new PublisherModel();
                        Publisher.MaNhaXuatBan = (string)reader.GetString(0);
                        Publisher.TenNhaXuatBan = (string)reader.GetString(1);
                        Publisher.DiaChi = (string)reader.GetString(2);
                        Publisher.DienThoai = (string)reader.GetString(3);

                        listPublisherModel.Add(Publisher);
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
            return listPublisherModel;
        }
    }
}
