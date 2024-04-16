using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QLCuaHangBanSach
{
    internal class CategoryDAO
    {
        public string connectionString = @"Data Source=DESKTOP-UQ06DIF;Initial Catalog=SQLQuery3.sql;Integrated Security=True";

        public List<CategoryModel> getAllCategory()
        {
            List<CategoryModel> listCategoryModel = new List<CategoryModel>();
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM TheLoai", sqlConnection);

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CategoryModel Category = new CategoryModel();
                        Category.MaTheLoai = (string)reader.GetString(0);
                        Category.TenTheLoai = (string)reader.GetString(1);

                        listCategoryModel.Add(Category);
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
            return listCategoryModel;
        }
    }
}
