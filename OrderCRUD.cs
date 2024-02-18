using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Collections;



namespace Ass_02
{
    internal class OrderCRUD
    {
        private static String connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=\"QueensDB\";Integrated Security=True;";

              
        private static void runQuery(String query)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Console.WriteLine("Query Successfully Executed !");
        }

        // Method to insert an order into the database
        static public void insertOrder(Order o)
        {
            try
            {
                String query = $"insert into [Order] (order_id, cust_cnic, cust_name, cust_phone, cust_address, product_id, price, size) Values({o.OrderID}, '{o.CustomerCNIC}', '{o.CustomerName}', '{o.CustomerPhone}', '{o.CustomerAddress}', {o.ProductId}, {o.Price}, '{o.Size}')";

                runQuery(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Method to retrieve all orders from the database
        static public void getAllOrders()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            String query = "select * from [Order]";

            SqlCommand cmd = new SqlCommand(query, conn);

            SqlDataReader reader = cmd.ExecuteReader();
            int i = 1;

            while (reader.Read())
            {
                Console.WriteLine();
                Console.WriteLine($"ID: {reader[0]}");
                Console.WriteLine($"CNIC: {reader[1]}");
                Console.WriteLine($"Name: {reader[2]}");
                Console.WriteLine($"Phone: {reader[3]}");
                Console.WriteLine($"Address: {reader[4]}");
                Console.WriteLine($"ProductID: {reader[5]}");
                Console.WriteLine($"Price: {reader[6]}");
                Console.WriteLine($"Size: {reader[7]}");
                Console.WriteLine();
            }

            reader.Close();
        }

        // Method to update the address of an order based on phone number
        static public void updateAddress(String address, String phone)
        {
            String query = "update [Order] set cust_address = '" + address + "' where cust_phone = '" + phone + "'";
            try
            {
                runQuery(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Method to delete an order from the database based on order ID
        static public void DeleteOrder(int orderId)
        {
            // can this query lead to injection? ig no coz it's integer
            String query = "delete from [Order] where order_id = " + orderId;

            try
            {
                runQuery(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Method to update the address of an order based on phone number (securely)
        static public void UpdateOrderAddress(String phone, String newAddress)
        {
            String query = "update [Order] set cust_address = @newAddress where cust_phone = @phone";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@newAddress", newAddress);
            cmd.Parameters.AddWithValue("@phone", phone);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Console.WriteLine("Success!");
        }
    }


}
