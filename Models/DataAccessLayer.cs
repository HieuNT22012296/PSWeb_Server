using System;
using System.Data;
using Microsoft.Data.SqlClient;
namespace PSWeb_Server.Models
{
	// Các database logic để ở đây
	public class DataAccessLayer
	{
		public Response register(Users users, SqlConnection connection)
		{
			Response response = new Response();
			SqlCommand cmd = new SqlCommand("sp_register", connection);
			cmd.CommandType = System.Data.CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@FirstName", users.FirstName);
            cmd.Parameters.AddWithValue("@LastName", users.LastName);
            cmd.Parameters.AddWithValue("@Password", users.Password);
            cmd.Parameters.AddWithValue("@Email", users.Email);
            cmd.Parameters.AddWithValue("@Fund", 0);
            cmd.Parameters.AddWithValue("@Type", "Users");
			cmd.Parameters.AddWithValue("@Type", "Pending");
			connection.Open();
			int i = cmd.ExecuteNonQuery();
			connection.Close();
			if(i > 0)
			{
				response.StatusCode = 200;
				response.StatusMessage = "User registed successfully";
			}
			else
			{
				response.StatusCode = 100;
				response.StatusMessage = "User registation failed";
			}
			return response;
        }

		public Response Login(Users users, SqlConnection connection)
		{
			SqlDataAdapter da = new SqlDataAdapter("sp_login", connection);
			da.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
			da.SelectCommand.Parameters.AddWithValue("@Email", users.Email);
            da.SelectCommand.Parameters.AddWithValue("@Password", users.Password);
			DataTable dt = new DataTable();
			da.Fill(dt);
			Response response = new Response();
			if(dt.Rows.Count > 0)
			{
				response.StatusCode = 200;
				response.StatusMessage = "User is valid";
			}
			else
			{
                response.StatusCode = 100;
                response.StatusMessage = "User is invalid";
            }
            return response;
		}
	}
}

