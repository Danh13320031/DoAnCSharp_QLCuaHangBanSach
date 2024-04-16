using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QLCuaHangBanSach
{
    internal class BookDAO
    {
        public string connectionString = @"Data Source=DESKTOP-UQ06DIF;Initial Catalog=SQLQuery3.sql;Integrated Security=True";

        public List<BookModel> getAllBook()
        {
            List<BookModel> listBookModel = new List<BookModel>();
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Sach", sqlConnection);

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BookModel Book = new BookModel();
                        Book.MaSach = (string)reader.GetString(0);
                        Book.TenSach = (string)reader.GetString(1);
                        Book.SoLuongTon = (int)reader.GetValue(2);
                        Book.MaTheLoai = (string)reader.GetString(3);
                        Book.MaNhaXuatBan = (string)reader.GetString(4);
                        Book.MaTacGia = (string)reader.GetString(5);

                        listBookModel.Add(Book);
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Connect Fail: " + error);
            } finally
            {
                sqlConnection.Close();
            }
            return listBookModel;
        }
    }
}
