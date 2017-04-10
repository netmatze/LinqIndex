using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqIndex.IndexBuilding
{
    public interface IIndex<TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>
    {
        bool Contains(TKey key);
        TElement this[TKey key] { get; }
    }
}
