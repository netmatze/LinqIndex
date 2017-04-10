using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace LinqIndex.IndexBuilding
{
    public sealed class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        private readonly TKey key;
        private List<TElement> elements;

        internal Grouping(TKey key, IEnumerable<TElement> elements)
        {
            this.key = key;
            this.elements = new List<TElement>(elements);
        }

        public TKey Key { get { return key; } }

        public void Add(TElement element)
        {
            elements.Add(element);
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
