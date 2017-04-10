using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinqIndex;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using LinqIndex;
using LinqIndex.IndexBuilding;

namespace LinqIndexTest
{
    [TestClass]
    public class LinqIndexTests
    {
        private readonly int lowerLimit = 1;
        private readonly int highterLimit = 10000;

        [TestMethod]
        public void CreateFastTree()
        {
            FastTree<int, int> fastTree = new FastTree<int, int>();
            var items = Enumerable.Range(lowerLimit, highterLimit);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();            
            foreach (var item in items)
            {
                fastTree.Insert(item, item);
            }
            stopWatch.Stop();
            Debug.WriteLine("Create index: " + stopWatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void CreateIndex()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var items = Enumerable.Range(lowerLimit, highterLimit);
            var index = items.ToIndex(integer => integer);
            stopWatch.Stop();
            Debug.WriteLine("Index generation: " + stopWatch.ElapsedMilliseconds);
            var counter = 0;
            foreach (var item in index)
            {
                var innerItem = item;
                counter++;
            }
            Assert.AreEqual(items.Count(), counter);
        }

        [TestMethod]
        public void CreateEqualIndex()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var items = Enumerable.Range(lowerLimit, highterLimit);
            var index = items.ToEqualIndex(integer => integer);
            stopWatch.Stop();
            Debug.WriteLine("EqualIndex generation: " + stopWatch.ElapsedMilliseconds);
            var counter = 0;
            foreach (var item in index)
            {
                var innerItem = item;
                counter++;
            }
            Assert.AreEqual(items.Count(), counter);
        }

        [TestMethod]
        public void EqualWhereWithoutIndex()
        {
            var items = Enumerable.Range(lowerLimit, highterLimit);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            items.Where(i => i > 1000000).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Where without index: " + stopWatch.ElapsedTicks);            
        }

        [TestMethod]
        public void EqualWhereIndex()
        {
            var items = Enumerable.Range(lowerLimit, highterLimit);
            var index = items.ToIndex(integer => integer);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var list = index.Where(i => i.Key > 1000000).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Where with index: " + stopWatch.ElapsedTicks);
        }

        [TestMethod]
        public void EqualWhereEqualIndex()
        {
            var items = Enumerable.Range(lowerLimit, highterLimit);
            var index = items.ToEqaulIndex(integer => integer);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var list = index.Where(i => i.Key > 1000000).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Where with equal index: " + stopWatch.ElapsedTicks);
        }

        [TestMethod]
        public void UnEqualWhereWithoutIndex()
        {
            var items = Enumerable.Range(lowerLimit, highterLimit);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = items.Where(i => 
            {                 
                return (i > 10000000);
            }).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Where without index: " + stopWatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void UnEqualWhereIndex()
        {
            var items = Enumerable.Range(lowerLimit, highterLimit);
            var index = items.ToIndex(integer => integer);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var list = index.Where(i => i.Key > 10000000).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Where with index: " + stopWatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void UnEqualWhereEqualIndex()
        {
            var items = Enumerable.Range(lowerLimit, highterLimit);
            var index = items.ToEqaulIndex(integer => integer);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var list = index.Where(i => i.Key > 999).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Where with equal index: " + stopWatch.ElapsedTicks);
        }

        [TestMethod]
        public void JoinWithoutIndex()
        {
            var items1 = Enumerable.Range(lowerLimit, highterLimit);
            var items2 = Enumerable.Range(lowerLimit, highterLimit);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var list = items1.Join(items2, i => i, j => j, (a, b) => new { A = a, B = b }).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Join without index: " + stopWatch.ElapsedTicks);
        }

        [TestMethod]
        public void JoinEqualIndex()
        {
            var items1 = Enumerable.Range(lowerLimit, highterLimit);
            var items2 = Enumerable.Range(lowerLimit, highterLimit);
            var index = items1.ToEqaulIndex(integer => integer);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var list = index.JoinIndex(items2, i => i, (a, b) => { return new { A = a, B = b }; }).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Join with equal index: " + stopWatch.ElapsedTicks);
        }

        [TestMethod]
        public void OuterJoinWithoutIndex()
        {
            var customers = Customer.SampleData();
            var orders = Order.SampleData();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = customers.GroupJoin(orders, 
                c => c.CustomerID, 
                o => o.CustomerID, 
                (c, o) => new { LastName = c.LastName, Orders = o.Count() }).ToList();
            stopWatch.Stop();
            Debug.WriteLine("GroupJoin without index: " + stopWatch.ElapsedTicks);
        }

        [TestMethod]
        public void OuterJoinWithLookup()
        {
            var customers = Customer.SampleData();
            var orders = Order.SampleData();
            var orders_lookup = orders.ToLookup(o => o.CustomerID);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = customers.
                Select(c => new { LastName = c.LastName, Orders = orders_lookup[c.CustomerID].Count() }).ToList();                
            stopWatch.Stop();
            Debug.WriteLine("Select with lookup: " + stopWatch.ElapsedTicks);
        }

        [TestMethod]
        public void OuterJoinEqualIndex()
        {
            IEnumerable<Customer> customers = Customer.SampleData();
            IEnumerable<Order> orders = Order.SampleData();
            var index = orders.ToEqaulGroupingIndex(o => o.CustomerID);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = customers.GroupJoinIndex(index, c => c.CustomerID,
                (c, o) => new { LastName = c.LastName, Orders = o.Count() }).ToList();
            stopWatch.Stop();
            Debug.WriteLine("GroupJoin with equal group index: " + stopWatch.ElapsedTicks);
        }

        [TestMethod]
        public void CreateTree()
        {
            //var items = Enumerable.Range(lowerLimit, highterLimit);
            //IEnumerable<Customer> customers = Customer.SampleData();
            //IndexDefinition<Customer> indexPart = customers.CreateIndexKey(c => c.Number)
            //    .AddIndexKey(c => c.LastName);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();            
            //var list = indexPart.Where(i => i.Number > 100).ToList();
            //stopWatch.Stop();
            //Debug.WriteLine("Where with equal index first time " + stopWatch.ElapsedMilliseconds);
            //stopWatch.Reset();
            List<Customer> list = new List<Customer>();
            list.Add(new Customer() { Number = 4, LastName = "name4" });   
            list.Add(new Customer() { Number = 6, LastName = "name6" });                     
            list.Add(new Customer() { Number = 8, LastName = "name8" });
            list.Add(new Customer() { Number = 9, LastName = "name9" });
            list.Add(new Customer() { Number = 10, LastName = "name10" });
            list.Add(new Customer() { Number = 7, LastName = "name7" });
            //list.Add(new Customer() { Number = 5, LastName = "name5" });
            //list.Add(new Customer() { Number = 3, LastName = "name3" });
            Tree<int, Customer> tree = new Tree<int, Customer>();
            foreach (var item in list)
            {
                tree.Insert(item.Number, item);
            }
            stopWatch.Stop();
            Debug.WriteLine("Create Tree: " + stopWatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void UnEqualWhereWithoutIndexCustomer()
        {
            var customers = Customer.SampleData();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = customers.Where(i =>
            {
                return (i.Number > 199999);
            }).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Where without index: " + stopWatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void UnEqualWhereIndexCustomer()
        {
            var customers = Customer.SampleData();
            var index = customers.ToIndex(integer => integer.Number);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var list = index.Where(i => i.Key > 199999).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Where with index: " + stopWatch.ElapsedMilliseconds);
        }

        //[TestMethod]
        //public void EqualIndexSpecification()
        //{
        //    IEnumerable<Customer> customers = Customer.SampleData();
        //    IndexDefinition<Customer> indexPart = customers.CreateIndexKey(c => c.Number)
        //        .AddIndexKey(c => c.LastName);                
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();
        //    var list = customers.Where(i => i.Number > 1000000 && i.LastName == "Gottshall").ToList();
        //    stopWatch.Stop();
        //    Debug.WriteLine("Where without index first time " + stopWatch.ElapsedMilliseconds);
        //    stopWatch.Reset();
        //    stopWatch.Start();
        //    list = customers.Where(i => i.Number > 1000000 && i.LastName == "Gottshall").ToList();
        //    stopWatch.Stop();
        //    Debug.WriteLine("Where without index second time " + stopWatch.ElapsedMilliseconds);
        //    stopWatch.Reset();
        //    stopWatch.Start();
        //    list = indexPart.Where(i => i.Number > 1000000 && i.LastName == "Gottshall").ToList();
        //    stopWatch.Stop();
        //    Debug.WriteLine("Where with equal index first time " + stopWatch.ElapsedMilliseconds);
        //    stopWatch.Reset();
        //    stopWatch.Start();
        //    list = indexPart.Where(i => i.Number > 1000000 && i.LastName == "Gottshall").ToList();
        //    stopWatch.Stop();
        //    Debug.WriteLine("Where with equal index second time " + stopWatch.ElapsedMilliseconds);
        //}

        [TestMethod]
        public void EqualIndexSpecification()
        {
            IEnumerable<Customer> customers = Customer.SampleData();
            IndexDefinition<Customer> indexPart = customers.CreateIndexKey(c => c.Number)
                .AddIndexKey(c => c.Age);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var list = customers.Where(i => i.Age <= 200 && i.Number > 2).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Where without index first time " + stopWatch.ElapsedMilliseconds);
            stopWatch.Reset();
            stopWatch.Start();
            list = customers.Where(i => i.Age <= 200 && i.Number > 2).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Where without index second time " + stopWatch.ElapsedMilliseconds);
            stopWatch.Reset();
            stopWatch.Start();
            list = indexPart.Where(i => i.Age <= 20000 && i.Number > 100).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Where with equal index first time " + stopWatch.ElapsedMilliseconds);
            stopWatch.Reset();
            stopWatch.Start();
            list = indexPart.Where(i => i.Age <= 20000 && i.Number > 100).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Where with equal index second time " + stopWatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void UnequalIndexSpecification()
        {
            IEnumerable<Customer> customers = Customer.SampleData();
            IndexDefinition<Customer> indexPart = customers.CreateIndexKey(c => c.Number);                            
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var list = customers.Where(c => c.Number > 1000000).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Where for unequal condition without index first time " + stopWatch.ElapsedMilliseconds);
            stopWatch.Reset();
            stopWatch.Start();
            list = customers.Where(c => c.Number > 1000000).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Where for unequal condition without index second time " + stopWatch.ElapsedMilliseconds);
            stopWatch.Reset();
            stopWatch.Start();
            list = indexPart.Where(c => c.Number > 1000000).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Where for unequal condition with unequal index first time " + stopWatch.ElapsedMilliseconds);
            stopWatch.Reset();
            stopWatch.Start();
            list = indexPart.Where(c => c.Number > 1000000).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Where for unequal condition with unequal index second time " + stopWatch.ElapsedMilliseconds);
            //list = customers.Where(c => c.Number > 1000000).ToList();
            ////list = customers.Where(c => c.Number > 1000000).ToList();
            ////list = customers.Where(c => c.Number > 1000000).ToList();
            ////list = customers.Where(c => c.Number > 1000000).ToList();
            ////list = customers.Where(c => c.Number > 1000000).ToList();
            ////list = customers.Where(c => c.Number > 1000000).ToList();
            ////list = customers.Where(c => c.Number > 1000000).ToList();
            ////list = customers.Where(c => c.Number > 1000000).ToList();
            //stopWatch.Stop();
            //Debug.WriteLine("Where without index: " + stopWatch.ElapsedMilliseconds);
            //Stopwatch stopWatch1 = new Stopwatch();
            //stopWatch1.Start();
            //list = indexPart.Where(c => c.Number > 1000000).ToList();
            ////list = indexPart.Where(c => c.Number > 1000000).ToList();
            ////list = indexPart.Where(c => c.Number > 1000000).ToList();
            ////list = indexPart.Where(c => c.Number > 1000000).ToList();
            ////list = indexPart.Where(c => c.Number > 1000000).ToList();
            ////list = indexPart.Where(c => c.Number > 1000000).ToList();
            ////list = indexPart.Where(c => c.Number > 1000000).ToList();
            ////list = indexPart.Where(c => c.Number > 1000000).ToList();                  
            //stopWatch1.Stop();
            //Debug.WriteLine("Where with unequal index: " + stopWatch1.ElapsedMilliseconds);
        }

        [TestMethod]
        public void WithoutIndexSpecification()
        {
            IEnumerable<Customer> customers = Customer.SampleData();           
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var list = customers.Where(i => i.Number > 10000).ToList();
            list = customers.Where(i => i.Number == 100000).ToList();
            list = customers.Where(i => i.Number == 20000).ToList();
            list = customers.Where(i => i.Number == 200000).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Where without index: " + stopWatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void EqaulIndexJoinSpecification()
        {
            IEnumerable<Customer> customers = Customer.SampleData();
            var indexPart = customers.CreateIndexKey(c => c.CustomerID).
                AddIndexKey(c => c.Number);            
            IEnumerable<int> range = Enumerable.Range(1, 10);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = indexPart.Join(range, i => i, c => c.Number, 
                (c, i) => string.Format("{0}: {1}, {2}",i, c.CustomerID,c.Number)).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Where with equal index: " + stopWatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void EqualIndexJoinSpecification()
        {
            var customers = Customer.SampleData();
            var indexPart = customers.CreateIndexKey(c => c.CustomerID).
                AddIndexKey(c => c.Number);
            var orders = Order.SampleData();
            var result1 = indexPart.Join(orders,
                c => c.CustomerID,
                o => o.CustomerID,
                (c, o) => new { LastName = c.LastName, Orders = o.OrderNumber }).ToList();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = customers.Join(orders,
                c => c.CustomerID,
                o => o.CustomerID,
                (c, o) => new { LastName = c.LastName, Orders = o.OrderNumber }).ToList();
            stopWatch.Stop();
            Debug.WriteLine("Join without index: " + stopWatch.ElapsedMilliseconds);
            Stopwatch stopWatch1 = new Stopwatch();
            stopWatch1.Start();
            result = indexPart.Join(orders,
                c => c.CustomerID,
                o => o.CustomerID,
                (c, o) => new { LastName = c.LastName, Orders = o.OrderNumber }).ToList();
            stopWatch1.Stop();
            Debug.WriteLine("Join with index: " + stopWatch1.ElapsedMilliseconds);            
        }

        [TestMethod]
        public void EqualIndexGroupJoinSpecification()
        {
            var customers = Customer.SampleData();
            var indexPart = customers.CreateIndexKey(c => c.CustomerID).
                AddIndexKey(c => c.Number);
            var orders = Order.SampleData();
            var result1 = indexPart.GroupJoin(orders,
                c => c.CustomerID,
                o => o.CustomerID,
                (c, o) => new { LastName = c.LastName, Orders = o.Count() }).ToList();                        
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = customers.GroupJoin(orders,
                c => c.CustomerID,
                o => o.CustomerID,
                (c, o) => new { LastName = c.LastName, Orders = o.Count() }).ToList();
            stopWatch.Stop();
            Debug.WriteLine("GroupJoin without index: " + stopWatch.ElapsedTicks);
            Stopwatch stopWatch1 = new Stopwatch();
            stopWatch1.Start();
            result = indexPart.GroupJoin(orders,
                c => c.CustomerID,
                o => o.CustomerID,
                (c, o) => new { LastName = c.LastName, Orders = o.Count() }).ToList();            
            stopWatch1.Stop();
            Debug.WriteLine("GroupJoin with index: " + stopWatch1.ElapsedTicks);
            Stopwatch stopWatch2 = new Stopwatch();
            var orders_lookup = orders.ToLookup(o => o.CustomerID);
            stopWatch2.Start();           
            result = customers.Select(c =>
                new { LastName = c.LastName, Orders = orders_lookup[c.CustomerID].Count() }).ToList();
            stopWatch2.Stop();
            Debug.WriteLine("GroupJoin with ToLookup: " + stopWatch2.ElapsedMilliseconds);
        }

        [TestMethod]
        public void WithoutIndexGroupJoinSpecification()
        {
            var customers = Customer.SampleData();           
            var orders = Order.SampleData();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();            
            var result1 = customers.GroupJoin(orders,
                c => c.CustomerID,
                o => o.CustomerID,
                (c, o) => new { LastName = c.LastName, Orders = o.Count() }).ToList();            
            stopWatch.Stop();
            Debug.WriteLine("GroupJoin without index: " + stopWatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void WithLookupGroupJoinSpecification()
        {
            var customers = Customer.SampleData();
            var orders = Order.SampleData();            
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var orders_lookup = orders.ToLookup(o => o.CustomerID);
            var result1 = customers.Select(c => 
                new { LastName = c.LastName, Orders = orders_lookup[c.CustomerID].Count() }).ToList();                
            stopWatch.Stop();
            Debug.WriteLine("GroupJoin without index: " + stopWatch.ElapsedMilliseconds);
        }        
    }
}