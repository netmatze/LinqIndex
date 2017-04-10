using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace LinqIndex.IndexBuilding
{
    public static class ToIndexExtention
    {        
        public static IIndex<TKey, TSource> ToIndex<TSource, TKey>(
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

        public static IIndex<TKey, TSource> ToEqaulIndex<TSource, TKey>(
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

        public static IGroupingIndex<TKey, TSource> ToEqaulGroupingIndex<TSource, TKey>(
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

        //public static IIndex<TKey, TSource> ToAvlIndex<TSource, TKey>(
        //    this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : IComparable<TKey>
        //{
        //    Index<TKey, TSource> index = new Index<TKey, TSource>();
        //    foreach (var item in source)
        //    {
        //        TKey key = keySelector(item);
        //        index.Add(key, item);
        //    }
        //    return index;
        //}

        public static IIndex<TKey, TSource> ToEqualIndex<TSource, TKey>(
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

        //public static IEnumerable<TSource> Where<TKey, TSource>(this IIndex<TKey, TSource> source,
        //    Func<IGrouping<TKey, TSource>, bool> predicate)
        //{
        //    foreach (IGrouping<TKey, TSource> item in source)
        //    {
        //        if (predicate(item))
        //        {
        //            foreach (var innerItem in item)
        //            {
        //                yield return innerItem;
        //            }
        //        }
        //    }
        //}

        //public static IIndex<TKey, TSource> WhereIndex<TKey, TSource>(this IIndex<TKey, TSource> source,
        //    Func<IGrouping<TKey, TSource>, bool> predicate) where TKey : IComparable<TKey>
        //{
        //    Index<TKey, TSource> index = new Index<TKey, TSource>();
        //    foreach (IGrouping<TKey, TSource> item in source)
        //    {
        //        if (!predicate(item))
        //        {
        //            index.Delete(item.Key);
        //        }
        //    }
        //    return index;
        //}

        private static IEnumerable<TSource> And<TSource>(IIndex<int, TSource> source,
            BinaryExpression binaryExpression)
        {            
            BinaryExpression leftBinaryExpression = null;
            BinaryExpression rightBinaryExpression = null;
            if (binaryExpression.Left.NodeType == ExpressionType.LessThan ||
                binaryExpression.Left.NodeType == ExpressionType.GreaterThan)
            {
                leftBinaryExpression = (BinaryExpression)binaryExpression.Left;
            }
            if (binaryExpression.Right.NodeType == ExpressionType.LessThan ||
                binaryExpression.Right.NodeType == ExpressionType.GreaterThan)
            {
                rightBinaryExpression = (BinaryExpression)binaryExpression.Right;
            }            
            return AndExecute(source, leftBinaryExpression, rightBinaryExpression);
        }

        private static IEnumerable<TSource> Or<TSource>(IIndex<int, TSource> source,
            BinaryExpression binaryExpression)
        {
            BinaryExpression leftBinaryExpression = null;
            BinaryExpression rightBinaryExpression = null;
            if (binaryExpression.Left.NodeType == ExpressionType.LessThan ||
                binaryExpression.Left.NodeType == ExpressionType.GreaterThan)
            {
                leftBinaryExpression = (BinaryExpression)binaryExpression.Left;
            }
            if (binaryExpression.Right.NodeType == ExpressionType.LessThan ||
                binaryExpression.Right.NodeType == ExpressionType.GreaterThan)
            {
                rightBinaryExpression = (BinaryExpression)binaryExpression.Right;
            }
            return OrExecute(source, leftBinaryExpression, rightBinaryExpression);
        }

        private static IEnumerable<TSource> OrExecute<TSource>(IIndex<int, TSource> source,
            BinaryExpression leftbinaryExpression, BinaryExpression rightbinaryExpression)
        {
            Index<int, TSource> index = (Index<int, TSource>)source;
            LambdaExpression leftinnerLambdaExpression = null;
            LambdaExpression rightinnerLambdaExpression = null;
            HashSet<TSource> values = new HashSet<TSource>();
            int leftstartValue = 0;
            int rightstartValue = 0;
            if (leftbinaryExpression.NodeType == ExpressionType.LessThan)
            {
                leftinnerLambdaExpression = BuildLessThanLambda(leftbinaryExpression);
            }
            else if (leftbinaryExpression.NodeType == ExpressionType.GreaterThan)
            {
                leftinnerLambdaExpression = BuildGreatherThanLambda(leftbinaryExpression);
                leftstartValue = BuildGreatherThanStartValue(leftbinaryExpression);
            }
            if (rightbinaryExpression.NodeType == ExpressionType.LessThan)
            {
                rightinnerLambdaExpression = BuildLessThanLambda(rightbinaryExpression);
            }
            else if (rightbinaryExpression.NodeType == ExpressionType.GreaterThan)
            {
                rightinnerLambdaExpression = BuildGreatherThanLambda(rightbinaryExpression);
                rightstartValue = BuildGreatherThanStartValue(rightbinaryExpression);
            }
            if (leftbinaryExpression.NodeType == ExpressionType.LessThan)
            {
                int min = index.Min();
                int counter = min;
                Delegate del = leftinnerLambdaExpression.Compile();
                while ((bool)del.DynamicInvoke(counter))
                {
                    var item = source[counter];
                    if (item != null && !item.Equals(default(TSource)))
                    {
                        if (!values.Contains(item))
                            values.Add(item);
                    }
                    counter++;
                }
            }
            if (leftbinaryExpression.NodeType == ExpressionType.GreaterThan)
            {
                int counter = leftstartValue + 1;
                int max = index.Max();
                Delegate del = leftinnerLambdaExpression.Compile();
                while ((bool)del.DynamicInvoke(counter))
                {
                    var item = source[counter];
                    if (item != null && !item.Equals(default(TSource)))
                    {
                        if (!values.Contains(item))
                            values.Add(item);    
                    }
                    if (counter >= max)
                    {
                        break;
                    }
                    counter++;
                }
            }
            if (rightbinaryExpression.NodeType == ExpressionType.LessThan)
            {
                int min = index.Min();
                int counter = min;
                Delegate del = rightinnerLambdaExpression.Compile();
                while ((bool)del.DynamicInvoke(counter))
                {
                    var item = source[counter];
                    if (item != null && !item.Equals(default(TSource)))
                    {
                        if (!values.Contains(item))
                            values.Add(item);
                    }
                    counter++;
                }
            }
            if (rightbinaryExpression.NodeType == ExpressionType.GreaterThan)
            {
                int counter = rightstartValue + 1;
                int max = index.Max();
                Delegate del = rightinnerLambdaExpression.Compile();
                while ((bool)del.DynamicInvoke(counter))
                {
                    var item = source[counter];
                    if (item != null && !item.Equals(default(TSource)))
                    {
                        if (!values.Contains(item))
                            values.Add(item);
                    }
                    if (counter >= max)
                    {
                        break;
                    }
                    counter++;
                }
            }
            return values;
        }

        private static IEnumerable<TSource> AndExecute<TSource>(IIndex<int, TSource> source,
            BinaryExpression leftbinaryExpression, BinaryExpression rightbinaryExpression)
        {
            Index<int, TSource> index = (Index<int, TSource>)source;
            LambdaExpression leftinnerLambdaExpression = null;
            LambdaExpression rightinnerLambdaExpression = null;
            if (leftbinaryExpression.NodeType == ExpressionType.LessThan)
            {
                leftinnerLambdaExpression = BuildLessThanLambda(leftbinaryExpression);
            }
            else if (leftbinaryExpression.NodeType == ExpressionType.GreaterThan)
            {
                leftinnerLambdaExpression = BuildGreatherThanLambda(leftbinaryExpression);
            }
            if (rightbinaryExpression.NodeType == ExpressionType.LessThan)
            {
                rightinnerLambdaExpression = BuildLessThanLambda(rightbinaryExpression);
            }
            else if (rightbinaryExpression.NodeType == ExpressionType.GreaterThan)
            {
                rightinnerLambdaExpression = BuildGreatherThanLambda(rightbinaryExpression);
            }
            if (leftbinaryExpression.NodeType == ExpressionType.LessThan)
            {               
                int min = index.Min();
                int counter = min;
                Delegate del = leftinnerLambdaExpression.Compile();
                while ((bool)del.DynamicInvoke(counter))
                {
                    var item = source[counter];
                    if (item != null && !item.Equals(default(TSource)))
                    {
                        Delegate delInner = rightinnerLambdaExpression.Compile();
                        if((bool)delInner.DynamicInvoke(item))
                        {
                            yield return item;
                        }                            
                    }
                    counter++;
                }
            }            
            if (leftbinaryExpression.NodeType == ExpressionType.GreaterThan)
            {
                int counter = 0;               
                int max = index.Max();
                Delegate del = rightinnerLambdaExpression.Compile();
                while ((bool)del.DynamicInvoke(counter))
                {
                    var item = source[counter];
                    if (item != null && !item.Equals(default(TSource)))
                    {                        
                        Delegate delInner = leftinnerLambdaExpression.Compile();
                        if ((bool)delInner.DynamicInvoke(counter))
                        {
                            yield return item;                         
                        }
                    }
                    if (counter >= max)
                    {
                        break;
                    }
                    counter++;
                }
            }
        }

        private static LambdaExpression BuildLessThanLambda(BinaryExpression binaryExpression)
        {
            ConstantExpression constantExpression = null;
            MemberExpression memberExpression = null;
            if (binaryExpression.Left.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = (MemberExpression)binaryExpression.Left;
            }
            if (binaryExpression.Right.NodeType == ExpressionType.Constant)
            {
                constantExpression = (ConstantExpression)binaryExpression.Right;
            }
            ParameterExpression param = Expression.Parameter(typeof(int), "param");
            LambdaExpression innerLambdaExpression = Expression.Lambda<Func<int, bool>>(
                Expression.LessThan(param, constantExpression), param);
            return innerLambdaExpression;
        }

        private static LambdaExpression BuildGreatherThanLambda(BinaryExpression binaryExpression)
        {
            ConstantExpression constantExpression = null;
            MemberExpression memberExpression = null;
            if (binaryExpression.Left.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = (MemberExpression)binaryExpression.Left;
            }
            if (binaryExpression.Right.NodeType == ExpressionType.Constant)
            {
                constantExpression = (ConstantExpression)binaryExpression.Right;
            }
            ParameterExpression param = Expression.Parameter(typeof(int), "param");
            LambdaExpression innerLambdaExpression = Expression.Lambda<Func<int, bool>>(
                Expression.GreaterThan(param, constantExpression), param);
            return innerLambdaExpression;
        }

        private static int BuildGreatherThanStartValue(BinaryExpression binaryExpression)
        {
            ConstantExpression constantExpression = null;
            MemberExpression memberExpression = null;            
            if (binaryExpression.Right.NodeType == ExpressionType.Constant)
            {
                constantExpression = (ConstantExpression)binaryExpression.Right;
            }
            return (int)constantExpression.Value;
        }

        private static IEnumerable<TSource> LessThan<TSource>(IIndex<int, TSource> source,
            BinaryExpression binaryExpression)
        {
            Index<int, TSource> index = (Index<int, TSource>)source;
            int min = index.Min();
            ConstantExpression constantExpression = null;
            MemberExpression memberExpression = null;
            if (binaryExpression.Left.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = (MemberExpression)binaryExpression.Left;
            }
            if (binaryExpression.Right.NodeType == ExpressionType.Constant)
            {
                constantExpression = (ConstantExpression)binaryExpression.Right;
            }
            int counter = min;
            ParameterExpression param = Expression.Parameter(typeof(int), "param");
            LambdaExpression innerLambdaExpression = Expression.Lambda<Func<int, bool>>(
                Expression.LessThan(param, constantExpression), param);
            Delegate del = innerLambdaExpression.Compile();
            while ((bool)del.DynamicInvoke(counter))
            {
                var item = source[counter];
                if (item != null && !item.Equals(default(TSource)))
                {
                    yield return item;
                }
                counter++;
            }
        }

        private static IEnumerable<TSource> GreaterThan<TSource>(IIndex<int, TSource> source,
            BinaryExpression binaryExpression)
        {
            Index<int, TSource> index = (Index<int, TSource>) source;
            int max = index.Max();
            ConstantExpression constantExpression = null;
            MemberExpression memberExpression = null;
            if (binaryExpression.Left.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = (MemberExpression)binaryExpression.Left;
            }
            if (binaryExpression.Right.NodeType == ExpressionType.Constant)
            {
                constantExpression = (ConstantExpression)binaryExpression.Right;
            }
            int counter = (int) constantExpression.Value + 1;
            ParameterExpression param = Expression.Parameter(typeof(int), "param");
            LambdaExpression innerLambdaExpression = Expression.Lambda<Func<int, bool>>(
                Expression.GreaterThan(param, constantExpression), param);
            Delegate del = innerLambdaExpression.Compile();
            while ((bool)del.DynamicInvoke(counter))
            {
                var item = source[counter];
                if (item != null && !item.Equals(default(TSource)))
                {
                    yield return item;
                }
                if(counter >= max)
                {
                    break;
                }
                counter++;
            }
        }

        private static IEnumerable<TSource> Equal<TSource>(IIndex<int, TSource> source,
            BinaryExpression binaryExpression)
        {
            if (source is Index<int, TSource>)
            {
                Index<int, TSource> index = (Index<int, TSource>)source;
                int max = index.Max();
                ConstantExpression constantExpression = null;                
                if (binaryExpression.Right.NodeType == ExpressionType.Constant)
                {
                    constantExpression = (ConstantExpression)binaryExpression.Right;
                }

                if (source.Contains((int)constantExpression.Value))
                {
                    var item = source[(int)constantExpression.Value];
                    yield return item;
                }                
            }
            else if (source is EqualIndex<int, TSource>)
            {
                EqualIndex<int, TSource> index = (EqualIndex<int, TSource>)source;               
                ConstantExpression constantExpression = null;                
                if (binaryExpression.Right.NodeType == ExpressionType.Constant)
                {
                    constantExpression = (ConstantExpression)binaryExpression.Right;
                }                
                if (source.Contains((int)constantExpression.Value))
                {
                    var item = source[(int)constantExpression.Value];
                    yield return item;
                }
            }
        }

        public static IEnumerable<TSource> Where<TSource>(this IIndex<int, TSource> source,
            Expression<Func<IGrouping<int, TSource>, bool>> predicate) //where TKey : IComparable<TKey>
        {            
            if (predicate.NodeType == ExpressionType.Lambda)
            {
                LambdaExpression lambdaExpression = (LambdaExpression)predicate;
                if (lambdaExpression.Body.NodeType == ExpressionType.AndAlso)
                {
                    return And(source, (BinaryExpression)lambdaExpression.Body);
                }
                else if(lambdaExpression.Body.NodeType == ExpressionType.OrElse)
                {
                    return Or(source, (BinaryExpression)lambdaExpression.Body);
                }
                if (lambdaExpression.Body.NodeType == ExpressionType.LessThan)
                {
                    return LessThan(source, (BinaryExpression)lambdaExpression.Body);
                }
                if (lambdaExpression.Body.NodeType == ExpressionType.GreaterThan)
                {
                    return GreaterThan(source, (BinaryExpression)lambdaExpression.Body);
                }
                if (lambdaExpression.Body.NodeType == ExpressionType.Equal)
                {
                    return Equal(source, (BinaryExpression)lambdaExpression.Body);
                }
            }
            return null;
        }

        public static IIndex<TKey, TSource> WhereIndex<TKey, TSource>(this IIndex<TKey, TSource> source,
            Expression<Func<IGrouping<TKey, TSource>, bool>> predicate) where TKey : IComparable<TKey>
        {
            Index<TKey, TSource> index = new Index<TKey, TSource>();
            if (predicate.NodeType == ExpressionType.Lambda)
            {
                LambdaExpression lambdaExpression = (LambdaExpression)predicate;
                if (lambdaExpression.Body.NodeType == ExpressionType.LessThan)
                {
                    BinaryExpression binaryExpression = (BinaryExpression)lambdaExpression.Body;
                    if (binaryExpression.Left.NodeType == ExpressionType.MemberAccess)
                    {
                        MemberExpression memberExpression = (MemberExpression)binaryExpression.Left;
                    }
                    if (binaryExpression.Right.NodeType == ExpressionType.Constant)
                    {
                        ConstantExpression constantExpression = (ConstantExpression)binaryExpression.Right;
                    }                    
                }
            }            
            //foreach (IGrouping<TKey, TSource> item in source)
            //{                
            //    if (!predicate.Compile()(item))
            //    {
            //        index.Delete(item.Key);
            //    }
            //}
            return index;
        }

        //public static IEnumerable<TSource> Where<TKey, TSource>(this IIndex<TKey, TSource> source,
        //    TKey key, Func<TSource, bool> predicate)
        //{
        //    foreach (var item in source)
        //    {
        //        if (item.Key.Equals(key))
        //        {
        //            foreach (var innerItem in item)
        //            {
        //                if (predicate(innerItem))
        //                {
        //                    yield return innerItem;
        //                }
        //            }
        //        }
        //    }
        //}

        //public static IEnumerable<TSource> Where<TKey, TSource>(this IIndex<TKey, TSource> source,
        //    Func<IGrouping<TKey, TSource>, TKey> keySelector, Func<TSource, bool> predicate)
        //{
        //    foreach (var item in source)
        //    {
        //        if (item.Key.Equals(keySelector(item)))
        //        {
        //            foreach (var innerItem in item)
        //            {
        //                if (predicate(innerItem))
        //                {
        //                    yield return innerItem;
        //                }
        //            }
        //        }
        //    }
        //}

        //public static IEnumerable<TSource> Join<TKey, TSource>(this IIndex<TKey, TSource> source,
        //    IEnumerable<TSource> inner, Func<TSource, TKey> innerKeySelector)
        //{
        //    foreach (var innerItem in inner)
        //    {
        //        TSource value = source[innerKeySelector(innerItem)];
        //        if (!value.Equals(default(TSource)))
        //        {
        //            yield return value;
        //        }
        //    }
        //}

        //public static IEnumerable<TResult> Join<TKey, TSource, TInner, TResult>(this IEnumerable<TSource> source,
        //   IEnumerable<TInner> inner, Func<TSource, TKey> innerKeySelector, Func<TInner, TKey> outerKeySelector,
        //    Func<TSource, TInner, TResult> resultSelector)
        //{
        //    foreach (var sourceItem in source)
        //    {
        //        TKey sourceKey = innerKeySelector(sourceItem);
        //        foreach (var innerItem in inner)
        //        {
        //            TKey innerKey = outerKeySelector(innerItem);
        //            if (sourceKey.Equals(innerKey))
        //            {
        //                yield return resultSelector(sourceItem, innerItem);
        //            }
        //        }
        //    }
        //}

        public static IEnumerable<TResult> JoinIndex<TKey, TSource, TInner, TResult>(this IEnumerable<TSource> source,
           IEnumerable<TInner> inner, Func<TSource, TKey> innerKeySelector, Func<TInner, TKey> outerKeySelector,
            Func<TSource, TInner, TResult> resultSelector)
        {
            var lookup = inner.ToLookup(outerKeySelector);
            foreach (var sourceItem in source)
            {
                TKey sourceKey = innerKeySelector(sourceItem);                
                var result = lookup[sourceKey];
                foreach (var innerItem in result)
                {
                    yield return resultSelector(sourceItem, innerItem);                 
                }
            }
        }

        public static IIndex<TKey, TSource> JoinIndex<TKey, TSource>(this IIndex<TKey, TSource> source,
            IEnumerable<TSource> inner, Func<TSource, TKey> innerKeySelector) where TKey : IComparable<TKey>
        {
            Index<TKey, TSource> index = new Index<TKey, TSource>();
            foreach (var innerItem in inner)
            {
                TSource value = source[innerKeySelector(innerItem)];
                TKey key = innerKeySelector(innerItem);
                if (value != null)
                {
                    index.Add(key, value);
                }
            }
            return index;
        }

        public static IEnumerable<TResult> JoinIndex<TKey, TSource, TInner, TResult>(this IIndex<TKey, TSource> source,
            IEnumerable<TInner> inner, Func<TInner, TKey> innerKeySelector,
            Func<TSource, TInner, TResult> resultSelector) where TKey : IComparable<TKey>
        {
            foreach (TInner innerItem in inner)
            {
                var value = source[innerKeySelector(innerItem)];
                if (value != null)
                {
                    yield return resultSelector(value, innerItem);
                }
            }            
        }

        public static IEnumerable<TResult> GroupJoinIndex<TKey, TSource, TInner, TResult>(
            this IEnumerable<TSource> source, IGroupingIndex<TKey, TInner> inner, Func<TSource, TKey> sourceKeySelector,
            Func<TSource, IEnumerable<TInner>, TResult> resultSelector) where TKey : IComparable<TKey>
        {
            foreach (TSource innerItem in source)
            {
                var value = inner.GroupKey(sourceKeySelector(innerItem));
                yield return resultSelector(innerItem, value);
            }
        }
    }    
}
