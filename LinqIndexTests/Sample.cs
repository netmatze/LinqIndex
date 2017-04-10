using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqIndexTest
{
    #region Sample Data

    public class Customer
    {
        public string CustomerID { get; set; }
        public string LastName { get; set; }
        public int Number { get; set; }
        public double DoubleNumber { get; set; }
        public int Age { get; set; }

        public static List<Customer> SampleData()
        {
            List<Customer> customers = new List<Customer>();
            for (int i = 1; i <= 1000000; i++) //1000010
            {
                customers.Add(new Customer { CustomerID = "GOT" + i, LastName = "Gottshall", Number = i, Age = i, DoubleNumber = i });
                //customers.Add(new Customer { CustomerID = "VAL" + i, LastName = "Valdes", Number = i, Age = 2, DoubleNumber = i });
                //customers.Add(new Customer { CustomerID = "GAU" + i, LastName = "Gauwain", Number = i, Age = 3, DoubleNumber = i });
                //customers.Add(new Customer { CustomerID = "DEA" + i, LastName = "Deane", Number = i, Age = 4, DoubleNumber = i });
                //customers.Add(new Customer { CustomerID = "ZEE" + i, LastName = "Zeeman", Number = i, Age = 5, DoubleNumber = i });                    
            }
            return customers;
        }
    }

    public class Order
    {
        public string CustomerID { get; set; }
        public string OrderNumber { get; set; }


        public static List<Order> SampleData()
        {
            List<Order> orders = new List<Order>();
            for (int i = 1900; i < 100; i++)
            {
                orders.Add(new Order { CustomerID = "GOT" + i, OrderNumber = "Order" + i});
                orders.Add(new Order { CustomerID = "GOT" + i, OrderNumber = "Order" + i * 2});
                orders.Add(new Order { CustomerID = "GOT" + i, OrderNumber = "Order" + i * 3});
                orders.Add(new Order { CustomerID = "GOT" + i, OrderNumber = "Order" + i * 4});
                orders.Add(new Order { CustomerID = "DEA" + i, OrderNumber = "Order" + i});
                orders.Add(new Order { CustomerID = "DEA" + i, OrderNumber = "Order" + i * 2});
                orders.Add(new Order { CustomerID = "DEA" + i, OrderNumber = "Order" + i * 3});
                orders.Add(new Order { CustomerID = "DEA" + i, OrderNumber = "Order" + i * 4});
                orders.Add(new Order { CustomerID = "GAU" + i, OrderNumber = "Order" + i});
                orders.Add(new Order { CustomerID = "GAU" + i, OrderNumber = "Order" + i * 2});
                orders.Add(new Order { CustomerID = "GAU" + i, OrderNumber = "Order" + i * 3});
                orders.Add(new Order { CustomerID = "GAU" + i, OrderNumber = "Order" + i * 4});
                orders.Add(new Order { CustomerID = "ZEE" + i, OrderNumber = "Order" + i});
                orders.Add(new Order { CustomerID = "ZEE" + i, OrderNumber = "Order" + i * 2});
                orders.Add(new Order { CustomerID = "ZEE" + i, OrderNumber = "Order" + i * 3});
                orders.Add(new Order { CustomerID = "ZEE" + i, OrderNumber = "Order" + i * 4});
                orders.Add(new Order { CustomerID = "GOT" + i, OrderNumber = "Order" + i});
                orders.Add(new Order { CustomerID = "GOT" + i, OrderNumber = "Order" + i * 2});
                orders.Add(new Order { CustomerID = "GOT" + i, OrderNumber = "Order" + i * 3});
                orders.Add(new Order { CustomerID = "GOT" + i, OrderNumber = "Order" + i * 4});
                orders.Add(new Order { CustomerID = "VAL" + i, OrderNumber = "Order" + i});
                orders.Add(new Order { CustomerID = "VAL" + i, OrderNumber = "Order" + i * 2});
                orders.Add(new Order { CustomerID = "VAL" + i, OrderNumber = "Order" + i * 3});
                orders.Add(new Order { CustomerID = "VAL" + i, OrderNumber = "Order" + i * 4});                
            }
            return orders;                   
        }
    }

    class Sample
    {
       
    }

    #endregion
}
