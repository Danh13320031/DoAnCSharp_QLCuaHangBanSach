using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace QLCuaHangBanSach
{
    internal class AuthorDAO
    {
        public string connectionString = @"Data Source=DESKTOP-UQ06DIF;Initial Catalog=SQLQuery3.sql;Integrated Security=True";

        public List<AuthorModel> getAllAuthor()
        {
            List<AuthorModel> listAuthorModel = new List<AuthorModel>();
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {

                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT * FROM TacGia", sqlConnection);

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AuthorModel Author = new AuthorModel();
                        Author.MaTacGia = (string)reader.GetString(0);
                        Author.TenTacGia = (string)reader.GetString(1);
                        Author.LienLac = (string)reader.GetString(2);

                        listAuthorModel.Add(Author);
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
            return listAuthorModel;
        }
    }
}
