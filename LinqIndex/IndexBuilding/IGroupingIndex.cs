using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqIndex.IndexBuilding
{
    public interface IGroupingIndex<TKey, TElement> : IIndex<TKey, TElement>
    {
        IEnumerable<TElement> GroupKey(TKey key);
    }
}
