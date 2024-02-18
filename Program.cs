using Ass_02;
using System.Diagnostics;










//                                     MUHAMMAD AMIR     
//                                      BSE F21 M013








class Program
{
    static void Main(string[] args)
    {
        try
        {
            


            bool exit = false;
            while (!exit)
            {

                Console.Clear();

                // Create a menu for CRUD operations
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Insert Order");
                Console.WriteLine("2. Get All Orders");
                Console.WriteLine("3. Update Address");
                Console.WriteLine("4. Delete Order");
                Console.WriteLine("5. Secure Update Order");
                Console.WriteLine("6. Exit");



                Console.Write("Enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        // Insert Order
                        Console.Write("Enter order ID: ");
                        int orderID = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter customer CNIC: ");
                        string customerCNIC = Console.ReadLine();
                        Console.Write("Enter customer name: ");
                        string customerName = Console.ReadLine();
                        Console.Write("Enter customer phone: ");
                        string customerPhone = Console.ReadLine();
                        Console.Write("Enter customer address: ");
                        string customerAddress = Console.ReadLine();
                        Console.Write("Enter product ID: ");
                        int productID = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter price: ");
                        int price = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter size: ");
                        char size = Convert.ToChar(Console.ReadLine());
                        try
                        {
                            Order o = new Order(orderID, price, customerCNIC, customerName, customerAddress, customerPhone, productID, size);
                            OrderCRUD.insertOrder(o);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            
                        }                        
                        break;

                    case 2:
                        // Get All Orders
                        OrderCRUD.getAllOrders();
                        break;

                    case 3:
                        // Update Address
                        Console.Write("Enter customer phone: ");
                        string phone = Console.ReadLine();
                        Console.Write("Enter new address: ");
                        string address = Console.ReadLine();
                        OrderCRUD.updateAddress(address, phone);
                        break;

                    case 4:
                        // Delete Order
                        Console.Write("Enter order ID: ");
                        int deleteOrderID = Convert.ToInt32(Console.ReadLine());
                        OrderCRUD.DeleteOrder(deleteOrderID);
                        break;

                    case 5:
                        // Secure Update Order
                        Console.Write("Enter customer phone: ");
                        String custphone = Console.ReadLine();
                        Console.Write("Enter new address: ");
                        String custaddress = Console.ReadLine();
                        OrderCRUD.UpdateOrderAddress(custphone, custaddress);
                        break;

                    case 6:
                        // Exit
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;




                }
                Console.WriteLine("Press Any Key...");
                Console.ReadKey();

                
            }
        }
        catch (Exception ex)
        {
            //Console.WriteLine(ex.ToString());
            Console.WriteLine(ex.Message);
        }
    }
}




