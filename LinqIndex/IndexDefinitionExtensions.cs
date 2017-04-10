using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using LinqIndex.IndexBuilding;

namespace LinqIndex
{
    public static class IndexDefinitionExtensions
    {
        public static IEnumerable<TResult> Join<TKey, TSource, TInner, TResult>(this IndexDefinition<TSource> source,
            IEnumerable<TInner> inner, Func<TInner, TKey> innerKeySelector,
            Expression<Func<TSource, TKey>> outerKeySelector,
            Func<TSource, TInner, TResult> resultSelector)
        {
            foreach (TInner innerItem in inner)
            {
                TKey key = innerKeySelector(innerItem);
                var value = IndexFinder.FindInIndex(source, outerKeySelector, key, null);                
                if (value != null)
                {
                    yield return resultSelector(value, innerItem);
                }
            }
        }

        public static IEnumerable<TResult> Join<TKey, TSource, TInner, TResult>(this IndexDefinition<TSource> source,
            IEnumerable<TInner> inner, Func<TInner, TKey> innerKeySelector,
            Expression<Func<TSource, TKey>> outerKeySelector,
            Func<TSource, TInner, TResult> resultSelector, 
            IEqualityComparer<TKey> equalityComparer)
        {
            foreach (TInner innerItem in inner)
            {
                TKey key = innerKeySelector(innerItem);
                var value = IndexFinder.FindInIndex(source, outerKeySelector, key, equalityComparer);
                if (value != null)
                {
                    yield return resultSelector(value, innerItem);
                }
            }
        }

        public static IEnumerable<TResult> GroupJoin<TKey, TSource, TInner, TResult>(this IndexDefinition<TSource> source,
            IEnumerable<TInner> inner, Func<TInner, TKey> innerKeySelector,
            Expression<Func<TSource, TKey>> outerKeySelector,
            Func<TSource, IEnumerable<TInner>, TResult> resultSelector)
        {
            Dictionary<TKey, Tuple<TSource, List<TInner>>> dictionary = new Dictionary<TKey, Tuple<TSource, List<TInner>>>();
            foreach (TInner innerItem in inner)
            {
                TKey key = innerKeySelector(innerItem);
                var value = IndexFinder.FindInIndex(source, outerKeySelector, key, null);
                if (value != null)
                {
                    if (dictionary.ContainsKey(key))
                    {
                        Tuple<TSource, List<TInner>> tuple = dictionary[key];
                        tuple.Item2.Add(innerItem);
                    }
                    else
                    {
                        List<TInner> list = new List<TInner>();
                        list.Add(innerItem);
                        dictionary.Add(key, new Tuple<TSource, List<TInner>>(value, list));
                    }
                }
            }
            foreach (KeyValuePair<TKey, Tuple<TSource, List<TInner>>> key in dictionary)
            {
                yield return resultSelector(key.Value.Item1, key.Value.Item2);
            }
        }

        public static IEnumerable<TResult> GroupJoin<TKey, TSource, TInner, TResult>(this IndexDefinition<TSource> source,
            IEnumerable<TInner> inner, Func<TInner, TKey> innerKeySelector,
            Expression<Func<TSource, TKey>> outerKeySelector,
            Func<TSource, IEnumerable<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey> equalityComparer)
        {
            Dictionary<TKey, Tuple<TSource, List<TInner>>> dictionary = new Dictionary<TKey, Tuple<TSource, List<TInner>>>();
            foreach (TInner innerItem in inner)
            {
                TKey key = innerKeySelector(innerItem);
                var value = IndexFinder.FindInIndex(source, outerKeySelector, key, equalityComparer);
                if (value != null)
                {
                    if (dictionary.ContainsKey(key))
                    {
                        Tuple<TSource, List<TInner>> tuple = dictionary[key];
                        tuple.Item2.Add(innerItem);
                    }
                    else
                    {
                        List<TInner> list = new List<TInner>();
                        list.Add(innerItem);
                        dictionary.Add(key, new Tuple<TSource, List<TInner>>(value, list));
                    }
                }
            }
            foreach (KeyValuePair<TKey, Tuple<TSource, List<TInner>>> key in dictionary)
            {
                yield return resultSelector(key.Value.Item1, key.Value.Item2);
            }
        }

        public static IEnumerable<TSource> Where<TSource>(this IndexDefinition<TSource> source,
            Expression<Func<TSource, bool>> predicate)
        {
            Func<TSource, bool> func = predicate.Compile();
            if (predicate.NodeType == ExpressionType.Lambda)
            {                
                LambdaExpression lambdaExpression = (LambdaExpression)predicate;
                if (lambdaExpression.Body.NodeType == ExpressionType.AndAlso)
                {
                    return WhereExpressionBuilder.And(source, (BinaryExpression)lambdaExpression.Body, func);
                }
                else if (lambdaExpression.Body.NodeType == ExpressionType.OrElse)
                {
                    return WhereExpressionBuilder.Or(source, (BinaryExpression)lambdaExpression.Body, func);
                }
                else if (lambdaExpression.Body.NodeType == ExpressionType.LessThan)
                {
                    return WhereExpressionBuilder.LessThanOrLessThanOrEqaul(source, (BinaryExpression)lambdaExpression.Body, func, LogicalEnum.LessThan);
                }
                else if (lambdaExpression.Body.NodeType == ExpressionType.LessThanOrEqual)
                {
                    return WhereExpressionBuilder.LessThanOrLessThanOrEqaul(source, (BinaryExpression)lambdaExpression.Body, func, LogicalEnum.LessThanOrEqual);
                }
                else if (lambdaExpression.Body.NodeType == ExpressionType.GreaterThan)
                {
                    return WhereExpressionBuilder.GreaterThanOrGreaterThanOrEqual(source, (BinaryExpression)lambdaExpression.Body, func, LogicalEnum.GreaterThan);
                }
                else if (lambdaExpression.Body.NodeType == ExpressionType.GreaterThanOrEqual)
                {
                    return WhereExpressionBuilder.GreaterThanOrGreaterThanOrEqual(source, (BinaryExpression)lambdaExpression.Body, func, LogicalEnum.GreaterThanOrEqual);
                }
                else if (lambdaExpression.Body.NodeType == ExpressionType.Equal)
                {
                    return WhereExpressionBuilder.Equal(source, (BinaryExpression)lambdaExpression.Body, func);
                }
                else
                {
                    return source.Source.Where(func);
                }
            }
            return source.Source.Where(func);
        }     
    }
}

