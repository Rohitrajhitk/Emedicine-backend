using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using Azure;

namespace EMedicineBE.Models

{
    public class DAL
    {
        public Response register(Users users,SqlConnection connection)
        {
            try
            {
                Response response = new Response();
                SqlCommand cmd = new SqlCommand("sp_register", connection);
                var query = "select * from users where email=@email";
                //var query2 = "select * from users where email="+users.Email;
                // SqlCommand cmd = new SqlCommand(query, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FirstName", users.FirstName);
                cmd.Parameters.AddWithValue("@LastName", users.LastName);
                cmd.Parameters.AddWithValue("@Password", users.Password);
                cmd.Parameters.AddWithValue("@Email", users.Email);
                cmd.Parameters.AddWithValue("@Fund", 0);
                cmd.Parameters.AddWithValue("@Type", "Users");
           
                // cmd.Parameters.AddWithValue("@Type","Pending");
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "User registered successfully";
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "User registration failed";
                }
                return response;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public Response login(Users users, SqlConnection connection)
        {
            SqlDataAdapter da = new SqlDataAdapter("sp_login", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;      
            da.SelectCommand.Parameters.AddWithValue("@Password",users.Password);
            da.SelectCommand.Parameters.AddWithValue("@Email", users.Email);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Response response = new Response();
            Users user = new Users();
            if (dt.Rows.Count > 0)
            {
                user.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                user.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                user.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                user.Type = Convert.ToString(dt.Rows[0]["Type"]);
                response.StatusCode = 200;
                response.StatusMessage = "users is valid";
                response.user = user;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "users is invalid";
                response.user = null;
            }
            return response;
        }

        public Response viewUser(Users users, SqlConnection connection)
        {
            SqlDataAdapter da = new SqlDataAdapter("p_viewUser", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@ID", users.ID);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Response response = new Response();
            Users user = new Users();
            if (dt.Rows.Count > 0)
            {
                user.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                user.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                user.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                user.Type = Convert.ToString(dt.Rows[0]["Type"]);
                user.Fund = Convert.ToDecimal(dt.Rows[0]["Fund"]);
                user.CreatedOn = Convert.ToDateTime(dt.Rows[0]["CreatedOn"]);
                user.Password = Convert.ToString(dt.Rows[0]["Password"]);
                response.StatusCode = 200;
                response.StatusMessage = "users exits";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "users does not exists";
                response.user = user;
            }
            return response;
        }

        public Response updateProfile(Users users,SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_updateProfile",connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FirstName", users.FirstName);
            cmd.Parameters.AddWithValue("@LastName", users.LastName);
            cmd.Parameters.AddWithValue("@Password", users.Password);
            cmd.Parameters.AddWithValue("@Email", users.Email);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.StatusCode=200;
                response.StatusMessage = "Record updated successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Some error occur try after some time";
            }

            return response;
        }
        public Response addToCart(Cart carts,SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_addToCart",connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", carts.UserId);
            cmd.Parameters.AddWithValue("@UnitPrice", carts.UnitPrce);
            cmd.Parameters.AddWithValue("@Discount", carts.Discount);
            cmd.Parameters.AddWithValue("@Quantity", carts.Quantity);
            cmd.Parameters.AddWithValue("@TotlalPrice", carts.TotlalPrice);
            cmd.Parameters.AddWithValue("@MedicineId", carts.MedicineId);
            connection.Open();
            int i  = cmd.ExecuteNonQuery(); 
            connection.Close();
            if (i > 0)
            {
                response.StatusCode=200;
                response.StatusMessage = "Item added successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Item not added to the cart";
            }
            return response;
        }

        public Response placeOrder(Users users,SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_placeOrder", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", users.ID);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Order placed  successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Order does not placed ";
            }
            return response;
        }

        public Response orderList (Users users,SqlConnection connection) 
        {
            Response response = new Response();
            List<Orders> listOrder = new List<Orders>();
            SqlDataAdapter da = new SqlDataAdapter("sp_OrderList", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@Type", users.Type);
            da.SelectCommand.Parameters.AddWithValue("@ID",users.ID);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if(dt.Rows.Count > 0)
            {
                for(int i = 0;i<dt.Rows.Count;i++)
                {
                    Orders order = new Orders();
                    order.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    order.OrderNo = Convert.ToString(dt.Rows[i]["OrderNo"]);
                    order.OrderTotal = Convert.ToDecimal(dt.Rows[i]["OrderTotal"]);
                    order.OrderStatus = Convert.ToString(dt.Rows[i]["OrderStatus"]);
                    listOrder.Add(order);
                }
                if(listOrder.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Order detail fetched";
                    response.listOrders = listOrder;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "Order detail not fetched";
                    response.listOrders = null;
                }
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Order detail not fetched";
                response.listOrders = null;
            }
            return response;
        }

        public Response addUpdateMedicine(Medicines medicines,SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_addUpdateMedicine", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name",medicines.Name);
            cmd.Parameters.AddWithValue("@Manufacture", medicines.Manufacturer);
            cmd.Parameters.AddWithValue("@UnitPrce", medicines.UnitPrce);
            cmd.Parameters.AddWithValue("@Discount", medicines.Discount);
            cmd.Parameters.AddWithValue("@Quantity", medicines.Quantity);
            cmd.Parameters.AddWithValue("@ExpDate", medicines.ExpDate);
            cmd.Parameters.AddWithValue("@ImageUrl", medicines.ImageUrl);
            cmd.Parameters.AddWithValue("@Status", medicines.Status);
            cmd.Parameters.AddWithValue("@Type", medicines.Type);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Medicine inseted successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Medicine did not save. try again!";
            }
            return response;
        }
        public Response userList(SqlConnection connection)
        {
            Response response = new Response();
            List<Users> listUsers = new List<Users>();
            SqlDataAdapter da = new SqlDataAdapter("sp_UserList", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Users user = new Users();
                    user.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    user.FirstName = Convert.ToString(dt.Rows[i]["FirstName"]);
                    user.LastName = Convert.ToString(dt.Rows[i]["LastName"]);
                    user.Password = Convert.ToString(dt.Rows[i]["Password"]);
                    user.Email = Convert.ToString(dt.Rows[i]["Email"]);
                    user.Fund = Convert.ToDecimal(dt.Rows[i]["Fund"]);
                    user.Status = Convert.ToInt32(dt.Rows[i]["Status"]);
                    user.CreatedOn = Convert.ToDateTime(dt.Rows[i]["CreatedOn"]);
                    listUsers.Add(user);
                }
                if (listUsers.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "users detail fetched";
                    response.listUsers = listUsers;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "users detail not fetched";
                    response.listUsers= null;
                }
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "users detail not fetched";
                response.listUsers = null;
            }
            return response;
        }
    }
}
