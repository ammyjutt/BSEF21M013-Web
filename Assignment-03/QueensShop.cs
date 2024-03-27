using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;




namespace QueenShop
{
    internal class QueensShop
    {

        private string connStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=QueensDBTwo;Integrated Security=True;";

        private void fillRow(DataRow row)
        {
            try
            {
                Console.WriteLine("Enter Product Code: ");
                row["ProductCode"] = Console.ReadLine();

                Console.WriteLine("Enter Product Size (M/S/L): ");
                row["ProductSize"] = Convert.ToChar(Console.ReadLine());

                Console.WriteLine("Enter Customer Address: ");
                row["CustomerAddress"] = Console.ReadLine();

                Console.WriteLine("Enter Customer Contact: ");
                row["CustomerContact"] = Console.ReadLine();


                Console.WriteLine("Enter Product Quantity: ");
                row["ProductQuantity"] = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter Price: ");
                row["Price"] = Convert.ToDouble(Console.ReadLine());


                Console.WriteLine("Enter Customer Name: ");
                row["CustomerName"] = Console.ReadLine();

                Console.WriteLine("Enter Product Name: ");
                row["ProductName"] = Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        private void DisplayOrder(DataTable OrderTable)
        {
            try
            {
                int id = 0;
                Console.WriteLine("Enter OrderID to Display: ");
                id = Convert.ToInt32(Console.ReadLine());
                DataRow row = OrderTable.Rows.Find(id);
                if (row != null)
                {
                    string orderDetails = string.Format("ID: {0}, Code: {1}, Size: {2}, Address: {3}, Contact: {4}, Quantity: {5}, Price: {6}, CustomerName: {7} , ProdName: {8}",
                    row["OrderID"],
                    row["ProductCode"],
                    row["ProductSize"],
                    row["CustomerAddress"],
                    row["CustomerContact"],
                    row["ProductQuantity"],
                    row["Price"],
                    row["CustomerName"],
                    row["ProductName"]);
                    Console.WriteLine(orderDetails);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void UpdateOrder(DataTable OrderTable)
        {
            try
            {
                int id = 0;
                Console.WriteLine("Enter OrderID to Update: ");
                id = Convert.ToInt32(Console.ReadLine());
                DataRow row = OrderTable.Rows.Find(id);
                fillRow(row);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void InsertOrder(DataTable OrderTable)
        {
            try
            {
                DataRow newRow = OrderTable.NewRow();
                Console.WriteLine("Enter OrderID: ");
                newRow["OrderID"] = Convert.ToInt32(Console.ReadLine());
                fillRow(newRow);
                OrderTable.Rows.Add(newRow);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void DeleteOrder(DataTable OrderTable)
        {
            try
            {
                int id = 0;
                Console.WriteLine("Enter OrderID to Delete: ");
                id = Convert.ToInt32(Console.ReadLine());
                DataRow row = OrderTable.Rows.Find(id);
                if (row != null)
                {
                    row.Delete();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void Menu()
        {

            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();


            // insert 
            SqlCommand SelectCmd = new SqlCommand("SELECT * FROM [Order];", conn);
            da.SelectCommand = SelectCmd;


            // update 
            SqlCommand UpdateCmd = new SqlCommand("UPDATE [Order] SET ProductCode = @ProductCode, ProductSize = @ProductSize, CustomerAddress = @CustomerAddress, CustomerContact = @CustomerContact, ProductQuantity = @ProductQuantity, Price = @Price, CustomerName = @CustomerName, ProductName = @ProductName WHERE OrderID = @OrderID;", conn);
            UpdateCmd.Parameters.Add("@OrderID", SqlDbType.Int, 0, "OrderID");
            UpdateCmd.Parameters.Add("CustomerName", SqlDbType.NVarChar, 50, "CustomerName");
            UpdateCmd.Parameters.Add("CustomerAddress", SqlDbType.NVarChar, 50, "CustomerAddress");
            UpdateCmd.Parameters.Add("CustomerContact", SqlDbType.NChar, 12, "CustomerContact");
            UpdateCmd.Parameters.Add("ProductCode", SqlDbType.NVarChar, 50, "ProductCode");
            UpdateCmd.Parameters.Add("ProductSize", SqlDbType.NChar, 1, "ProductSize");
            UpdateCmd.Parameters.Add("ProductQuantity", SqlDbType.Int, 0, "ProductQuantity");
            UpdateCmd.Parameters.Add("Price", SqlDbType.Int, 0, "Price");
            UpdateCmd.Parameters.Add("ProductName", SqlDbType.NVarChar, 50, "ProductName");
            da.UpdateCommand = UpdateCmd;



            // insert 
            SqlCommand InsertCmd = new SqlCommand("INSERT into [Order] Values(@OrderID,@CustomerName,@CustomerAddress,@CustomerContact,@ProductCode,@ProductSize,@ProductQuantity,@Price,@ProductName);", conn);
            InsertCmd.Parameters.Add("@OrderID", SqlDbType.Int, 0, "OrderID");
            InsertCmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar, 50, "CustomerName");
            InsertCmd.Parameters.Add("@CustomerAddress", SqlDbType.NVarChar, 50, "CustomerAddress");
            InsertCmd.Parameters.Add("@CustomerContact", SqlDbType.NChar, 12, "CustomerContact");
            InsertCmd.Parameters.Add("@ProductCode", SqlDbType.NVarChar, 50, "ProductCode");
            InsertCmd.Parameters.Add("@ProductSize", SqlDbType.NChar, 1, "ProductSize");
            InsertCmd.Parameters.Add("@ProductQuantity", SqlDbType.Int, 0, "ProductQuantity");
            InsertCmd.Parameters.Add("@Price", SqlDbType.Int, 0, "Price");
            InsertCmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 50, "ProductName");
            da.InsertCommand = InsertCmd;




            // delete
            da.DeleteCommand = new SqlCommand("delete from [Order] WHERE OrderID = @arg", conn);
            da.DeleteCommand.Parameters.Add("arg", SqlDbType.Int,0,"OrderID");



            da.Fill(ds, "OrderTable");
            DataTable OrderTable = ds.Tables["OrderTable"];
            OrderTable.PrimaryKey = new DataColumn[] { OrderTable.Columns["OrderID"] };






            bool loop = true;

            // Display menu options
            while (loop)
            {
                try
                {
                    // press any key to continue
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("Menu:");
                    Console.WriteLine("1. Display Order");
                    Console.WriteLine("2. Update Order");
                    Console.WriteLine("3. Insert Order");
                    Console.WriteLine("4. Delete Order");
                    Console.WriteLine("5. Exit");

                    Console.WriteLine("Enter your choice: ");
                    int choice = Convert.ToInt32(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            DisplayOrder(OrderTable);
                            break;
                        case 2:
                            UpdateOrder(OrderTable);
                            break;
                        case 3:
                            InsertOrder(OrderTable);
                            break;
                        case 4:
                            DeleteOrder(OrderTable);
                            break;
                        case 5:
                            loop = false;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    continue;
                }
            }

            da.Update(ds, "OrderTable");
            conn.Close();

        }
    }

}