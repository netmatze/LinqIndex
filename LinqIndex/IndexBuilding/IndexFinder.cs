using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LinqIndex.IndexBuilding
{
    public static class IndexFinder
    {
        public static TSource FindInIndex<TKey, TSource>(IndexDefinition<TSource> source,
            Expression<Func<TSource, TKey>> outerKeySelector, TKey innerKey, IEqualityComparer<TKey> equalityComparer)
        {
            Func<TSource, TKey> func = outerKeySelector.Compile();
            LambdaExpression lambdaExpression = (LambdaExpression)outerKeySelector;
            if (lambdaExpression.Body is MemberExpression)
            {
                MemberExpression memberExpression = (MemberExpression)lambdaExpression.Body;
                var member = memberExpression.Member.Name;
                object indexDefinition = null;
                indexDefinition = source.Find(member, IndexType.EqualIndex);
                if (indexDefinition == null)
                {
                    foreach (var innerItem in source.Source)
                    {
                        if (equalityComparer != null)
                        {
                            if (equalityComparer.Equals(func(innerItem), innerKey))
                            {
                                return innerItem;
                            }
                        }
                        else
                        {
                            if (func(innerItem).Equals(innerKey))
                            {
                                return innerItem;
                            }
                        }
                    }
                }
                else
                {
                    dynamic indexSource =
                        indexDefinition.GetType().GetProperty("IndexCollection").GetValue(indexDefinition, null);
                    Type typeKey = indexDefinition.GetType().GetGenericArguments()[1];
                    if (equalityComparer != null)
                    {
                        if (indexSource.Contains(innerKey, equalityComparer))
                        {
                            var item = indexSource.Get(innerKey);
                            return item;
                        }
                    }
                    else
                    {
                        if (indexSource.Contains(innerKey))
                        {
                            var item = indexSource.Get(innerKey);
                            return item;
                        }
                    }
                }
            }
            return default(TSource);
        }        
    }
}
