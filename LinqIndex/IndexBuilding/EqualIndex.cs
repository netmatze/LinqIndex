using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqIndex.IndexBuilding
{
    public class EqualIndex<TKey, TElement> : IGroupingIndex<TKey, TElement> where TKey : IComparable<TKey>
    {
        public Dictionary<TKey, Grouping<TKey, TElement>> dictionary = new Dictionary<TKey, Grouping<TKey, TElement>>();

        internal void Add(TKey key, TElement element)
        {
            if (!dictionary.ContainsKey(key))
            {
                List<TElement> innerList = new List<TElement>();
                innerList.Add(element);
                Grouping<TKey, TElement> grouping = new Grouping<TKey, TElement>(key, innerList);
                dictionary.Add(key, grouping);
            }
            else
            {
                Grouping<TKey, TElement> grouping = dictionary[key];
                grouping.Add(element);
            }
        }

        internal void Delete(TKey key)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
            }
        }

        public TElement this[TKey key]
        {
            get
            {
                if (dictionary.ContainsKey(key))
                {
                    return dictionary[key].First();
                }
                return default(TElement);
            }
        }

        public TElement Get(object key)
        {           
            if (dictionary.ContainsKey((TKey)key))
            {
                return dictionary[(TKey)key].First();
            }
            return default(TElement);         
        }

        public IEnumerable<TElement> GroupKey(TKey key)
        {            
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            List<TElement> list = new List<TElement>();
            return list;
        }

        public IEnumerable<TElement> GroupKey(object key)
        {
            if (dictionary.ContainsKey((TKey)key))
            {
                return dictionary[(TKey)key];
            }
            List<TElement> list = new List<TElement>();
            return list;
        }

        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            foreach (var item in dictionary)
            {
                yield return (IGrouping<TKey,TElement>) item.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }       

        #region IIndex<TKey,TElement> Member

        public bool Contains(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        public bool Contains(object key)
        {
            return dictionary.ContainsKey((TKey)key);
        }

        #endregion          
    }
}
