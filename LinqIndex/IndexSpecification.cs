using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using LinqIndex.IndexBuilding;

namespace LinqIndex
{       
    public static class IndexSpecification
    {
        public static IndexDefinition<TSource> CreateIndexKey<TSource, TKey>(
            this IEnumerable<TSource> source, Expression<Func<TSource, TKey>> keySelector) where TKey : IComparable<TKey>
        {
            IndexDefinition<TSource> indexDefinition = new IndexDefinition<TSource>() { Source = source };
            IndexPartDefinition<TSource, TKey> index = 
                new IndexPartDefinition<TSource, TKey>() { KeySelector = keySelector, KeyType = typeof(TKey) };
            indexDefinition.Add(index);     
            return indexDefinition;
        }

        public static IndexDefinition<TSource> AddIndexKey<TSource, TKey>(
            this IndexDefinition<TSource> indexDefinition, Expression<Func<TSource, TKey>> keySelector) where TKey : IComparable<TKey>
        {
            IndexPartDefinition<TSource, TKey> index = 
                new IndexPartDefinition<TSource, TKey>() { KeySelector = keySelector, KeyType = typeof(TKey) };
            indexDefinition.Add(index);
            return indexDefinition;
        }

        public static IIndex<TKey, TSource> IndexSpecificationToIndex<TSource, TKey>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : IComparable<TKey>
        {
            EqualIndex<TKey, TSource> index = new EqualIndex<TKey, TSource>();
            foreach (var item in source)
            {
                TKey key = keySelector(item);
                index.Add(key, item);
            }
            return index;
        }

        public static IIndex<TKey, TSource> UnequalIndexSpecificationToIndex<TSource, TKey>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : IComparable<TKey>
        {
            Index<TKey, TSource> index = new Index<TKey, TSource>();
            foreach (var item in source)
            {
                TKey key = keySelector(item);
                index.Add(key, item);
            }
            return index;
        }
    }

    //public class Customer
    //{
    //    public string CustomerID { get; set; }
    //    public string LastName { get; set; }
    //    public int Number { get; set; }
    //    public double DoubleNumber { get; set; }
    //    public int Age { get; set; }

    //    public static List<Customer> SampleData()
    //    {
    //        List<Customer> customers = new List<Customer>();
    //        for (int i = 1; i <= 2000; i++) //1000010
    //        {
    //            customers.Add(new Customer { CustomerID = "GOT" + i, LastName = "Gottshall", Number = i, Age = i, DoubleNumber = i });
    //            //customers.Add(new Customer { CustomerID = "VAL" + i, LastName = "Valdes", Number = i, Age = 2, DoubleNumber = i });
    //            //customers.Add(new Customer { CustomerID = "GAU" + i, LastName = "Gauwain", Number = i, Age = 3, DoubleNumber = i });
    //            //customers.Add(new Customer { CustomerID = "DEA" + i, LastName = "Deane", Number = i, Age = 4, DoubleNumber = i });
    //            //customers.Add(new Customer { CustomerID = "ZEE" + i, LastName = "Zeeman", Number = i, Age = 5, DoubleNumber = i });                    
    //        }
    //        return customers;
    //    }
    //}
}