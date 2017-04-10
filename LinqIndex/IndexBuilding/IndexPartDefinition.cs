using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LinqIndex.IndexBuilding
{
    public class IndexPartDefinition<TSource, TKey> where TKey : IComparable<TKey>
    {
        public Expression<Func<TSource, TKey>> KeySelector { get; set; }
        public Type KeyType { get; set; }

        IIndex<TKey, TSource> indexCollection;

        public IIndex<TKey, TSource> IndexCollection 
        {
            get { return indexCollection; }
            set { indexCollection = value; }
        }        
        private IndexType indexType;

        public IIndex<TKey, TSource> BuildIndex(IEnumerable<TSource> source, IndexType indexType, bool recalculateIndex = false)
        {
            if (this.indexType == indexType)
            {
                if (IndexCollection == null || recalculateIndex)
                {
                    Func<TSource, TKey> keySelectorExecutable = KeySelector.Compile();
                    IIndex<TKey, TSource> indexCollection = null;
                    if (indexType == IndexType.EqualIndex)
                    {
                        indexCollection = source.IndexSpecificationToIndex<TSource, TKey>(keySelectorExecutable);
                    }
                    else
                    {
                        indexCollection = source.UnequalIndexSpecificationToIndex<TSource, TKey>(keySelectorExecutable);
                    }
                    IndexCollection = indexCollection;
                }
                return IndexCollection;
            }
            else
            {
                if (IndexCollection == null || recalculateIndex)
                {
                    this.indexType = indexType;
                    Func<TSource, TKey> keySelectorExecutable = KeySelector.Compile();
                    IIndex<TKey, TSource> indexCollection = null;
                    if (indexType == IndexType.EqualIndex)
                    {
                        indexCollection = source.IndexSpecificationToIndex<TSource, TKey>(keySelectorExecutable);
                    }
                    else
                    {
                        indexCollection = source.UnequalIndexSpecificationToIndex<TSource, TKey>(keySelectorExecutable);
                    }
                    IndexCollection = indexCollection;
                }
                return IndexCollection;
            }
        }
    }    
}
