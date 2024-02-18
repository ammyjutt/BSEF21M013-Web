using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ass_02
{
    internal class Order
    {

        private int orderID;
        private int price;
        private int productID;
        private String cust_name;
        private String cust_cnic;
        private String cust_phone;
        private String cust_address;
        private char size;


        // Constraints are also defined on database in back-end


        private bool is_valid_name(String name)
        {
            // Check if the name is null or empty
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            // Check if the name contains any invalid characters
            foreach (char c in name)
            {
                if (!char.IsLetter(c))
                {
                    return false;
                }
            }

            // Name is valid
            return true;
        }

        private bool is_valid_cnic(String cnic)
        {
            // Check if the CNIC is null or empty
            if (string.IsNullOrEmpty(cnic))
            {
                return false;
            }

            // Check if the CNIC has the correct format
            if (cnic.Length != 13)
            {
                return false;
            }

            // Check if the CNIC contains only digits
            foreach (char c in cnic)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            // CNIC is valid
            return true;
        }

        private bool is_valid_phone(String phone)
        {
            // Check if the phone number is null or empty
            if (string.IsNullOrEmpty(phone))
            {
                return false;
            }

            // Check if the phone number has the correct format
            if (phone.Length != 11)
            {
                return false;
            }

            // Check if the phone number contains only digits
            foreach (char c in phone)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            // Phone number is valid
            return true;
        }


        public int OrderID
        {
            get
            {
                return orderID;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("OrderID should be non-negative integer!");

                orderID = value;

            }
        }

        public int Price
        {
            get
            {
                return price;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Price is Negative !");
                price = value;
            }
        }

        public int ProductId
        {
            get
            {
                return productID;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("ProductID should be non-negative integer!");
                productID = value;
            }
        } 

        public String CustomerCNIC
        {
            get
            {
                return cust_cnic;
            }
            set
            {
                if (!is_valid_cnic(value))
                    throw new ArgumentException("Invalid CNIC!");
                cust_cnic = value;
            }
        
        }

        public String CustomerName { 

            get
            {
                return cust_name;
            }
            set
            {
                if (!is_valid_name(value))
                    throw new ArgumentException("Invalid Name!");
                cust_name = value;
            }
        
        
        }

        public String CustomerAddress
        {
            get
            {
                return cust_address;
            }
            set
            {
                if(string.IsNullOrEmpty(value))
                    throw new ArgumentException("Invalid Address!");
                cust_address = value;
            }

        }

        public String CustomerPhone
        {
            get
            {
                return cust_phone;
            }
            set
            {
                if (!is_valid_phone(value))
                    throw new ArgumentException("Invalid Phone Number!");
                cust_phone = value;
            }

        }

        public char Size
        {
            get
            {
                return size;
            }
            set
            {
                if (value != 'S' && value != 'M' && value != 'L')
                    throw new ArgumentException("Invalid Size!");
                size = value;
            }

        } 

        //  constructor 

        public Order(int orderID, int price, string customerCNIC, string customerName, string customerAddress, string customerPhone, int productId, char size)
        {
            OrderID = orderID;
            Price = price;
            CustomerCNIC = customerCNIC;
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            CustomerPhone = customerPhone;
            ProductId = productId;
            this.Size = size;
        }

        public override string ToString()
        {
            return "OrderID: " + OrderID + " Price: " + Price + " CustomerCNIC: " + CustomerCNIC + " CustomerName: " + CustomerName + " CustomerAddress: " + CustomerAddress + " CustomerPhone: " + CustomerPhone + " ProductId: " + ProductId + " Size: " + Size;
        }

    }
}
