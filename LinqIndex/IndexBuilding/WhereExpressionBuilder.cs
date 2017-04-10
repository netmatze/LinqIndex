using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LinqIndex.IndexBuilding
{
    public static class WhereExpressionBuilder
    {
        public static IEnumerable<TSource> And<TSource>(IndexDefinition<TSource> source,
            BinaryExpression binaryExpression, Func<TSource, bool> predicate)
        {
            BinaryExpression leftBinaryExpression = null;
            BinaryExpression rightBinaryExpression = null;
            IEnumerable<TSource> firstResult = null;
            IEnumerable<TSource> secondResult = null;
            IndexDefinition<TSource> tempSource = source;
            if (binaryExpression.Left.NodeType == ExpressionType.LessThan ||
                binaryExpression.Left.NodeType == ExpressionType.LessThanOrEqual ||
                binaryExpression.Left.NodeType == ExpressionType.GreaterThan ||
                binaryExpression.Left.NodeType == ExpressionType.GreaterThanOrEqual ||
                binaryExpression.Left.NodeType == ExpressionType.Equal)
            {
                leftBinaryExpression = (BinaryExpression)binaryExpression.Left;
                firstResult = FirstAddExecute(source, leftBinaryExpression, predicate, false).ToList();
            }
            else if (binaryExpression.Left.NodeType == ExpressionType.AndAlso)
            {
                leftBinaryExpression = (BinaryExpression)binaryExpression.Left;
                firstResult = And(source, leftBinaryExpression, predicate);
            }
            else if (binaryExpression.Left.NodeType == ExpressionType.OrElse)
            {
                leftBinaryExpression = (BinaryExpression)binaryExpression.Left;
                firstResult = Or(source, leftBinaryExpression, predicate);
            }
            if (binaryExpression.Right.NodeType == ExpressionType.LessThan ||
                binaryExpression.Right.NodeType == ExpressionType.LessThanOrEqual ||
                binaryExpression.Right.NodeType == ExpressionType.GreaterThan ||
                binaryExpression.Right.NodeType == ExpressionType.GreaterThanOrEqual ||
                binaryExpression.Right.NodeType == ExpressionType.Equal)
            {
                rightBinaryExpression = (BinaryExpression)binaryExpression.Right;
                secondResult = SecondAddExecute(source, firstResult, rightBinaryExpression, predicate, true).ToList();
            }
            else if (binaryExpression.Right.NodeType == ExpressionType.AndAlso)
            {
                rightBinaryExpression = (BinaryExpression)binaryExpression.Right;
                secondResult = And(source, rightBinaryExpression, predicate);
            }
            else if (binaryExpression.Right.NodeType == ExpressionType.OrElse)
            {
                rightBinaryExpression = (BinaryExpression)binaryExpression.Right;
                secondResult = Or(source, rightBinaryExpression, predicate);
            }
            List<TSource> list = firstResult.ToList();
            foreach (var item in secondResult)
            {
                yield return item;
            }
        }

        public static IEnumerable<TSource> SecondAddExecute<TSource>(
            IndexDefinition<TSource> source, IEnumerable<TSource> firstResult,
            BinaryExpression binaryExpression, Func<TSource, bool> predicate,
            bool recalculateIndex = false)
        {
            IEnumerable<TSource> result = null;
            if (binaryExpression.NodeType == ExpressionType.LessThan)
            {
                result = LessThanOrLessThanOrEqaul(source, firstResult, binaryExpression,
                    predicate, LogicalEnum.LessThan, recalculateIndex);
            }
            else if (binaryExpression.NodeType == ExpressionType.LessThanOrEqual)
            {
                result = LessThanOrLessThanOrEqaul(source, firstResult, binaryExpression,
                    predicate, LogicalEnum.LessThanOrEqual, recalculateIndex);
            }
            else if (binaryExpression.NodeType == ExpressionType.GreaterThan)
            {
                result = GreaterThanOrGreaterThanOrEqual(source, firstResult, binaryExpression,
                    predicate, LogicalEnum.GreaterThan, recalculateIndex);
            }
            else if (binaryExpression.NodeType == ExpressionType.GreaterThanOrEqual)
            {
                result = GreaterThanOrGreaterThanOrEqual(source, firstResult, binaryExpression,
                    predicate, LogicalEnum.GreaterThanOrEqual, recalculateIndex);
            }
            else if (binaryExpression.NodeType == ExpressionType.Equal)
            {
                result = Equal(source, firstResult, binaryExpression,
                    predicate, recalculateIndex);
            }
            foreach (var item in result)
            {
                yield return item;
            }
        }

        public static IEnumerable<TSource> FirstAddExecute<TSource>(
            IndexDefinition<TSource> source, BinaryExpression binaryExpression,
            Func<TSource, bool> predicate, bool recalculateIndex = false)
        {
            IEnumerable<TSource> result = null;
            if (binaryExpression.NodeType == ExpressionType.LessThan)
            {
                result = LessThanOrLessThanOrEqaul(source, binaryExpression,
                    predicate, LogicalEnum.LessThan, recalculateIndex);
            }
            else if (binaryExpression.NodeType == ExpressionType.LessThanOrEqual)
            {
                result = LessThanOrLessThanOrEqaul(source, binaryExpression,
                    predicate, LogicalEnum.LessThanOrEqual, recalculateIndex);
            }
            else if (binaryExpression.NodeType == ExpressionType.GreaterThan)
            {
                result = GreaterThanOrGreaterThanOrEqual(source, binaryExpression,
                    predicate, LogicalEnum.GreaterThan, recalculateIndex);
            }
            else if (binaryExpression.NodeType == ExpressionType.GreaterThanOrEqual)
            {
                result = GreaterThanOrGreaterThanOrEqual(source, binaryExpression,
                    predicate, LogicalEnum.GreaterThanOrEqual, recalculateIndex);
            }
            else if (binaryExpression.NodeType == ExpressionType.Equal)
            {
                result = Equal(source, binaryExpression, predicate, recalculateIndex);
            }
            foreach (var item in result)
            {
                yield return item;
            }
        }

        public static IEnumerable<TSource> Or<TSource>(
            IndexDefinition<TSource> source, BinaryExpression binaryExpression,
            Func<TSource, bool> predicate)
        {
            BinaryExpression leftBinaryExpression = null;
            BinaryExpression rightBinaryExpression = null;
            IEnumerable<TSource> firstResult = null;
            IEnumerable<TSource> secondResult = null;
            IndexDefinition<TSource> tempSource = source;
            if (binaryExpression.Left.NodeType == ExpressionType.LessThan ||
                binaryExpression.Left.NodeType == ExpressionType.LessThanOrEqual ||
                binaryExpression.Left.NodeType == ExpressionType.GreaterThan ||
                binaryExpression.Left.NodeType == ExpressionType.GreaterThanOrEqual ||
                binaryExpression.Left.NodeType == ExpressionType.Equal)
            {
                leftBinaryExpression = (BinaryExpression)binaryExpression.Left;
                firstResult = FirstAddExecute(source, leftBinaryExpression, predicate, false).ToList();
            }
            else if (binaryExpression.Left.NodeType == ExpressionType.AndAlso)
            {
                leftBinaryExpression = (BinaryExpression)binaryExpression.Left;
                firstResult = And(source, leftBinaryExpression, predicate);
            }
            else if (binaryExpression.Left.NodeType == ExpressionType.OrElse)
            {
                leftBinaryExpression = (BinaryExpression)binaryExpression.Left;
                firstResult = Or(source, leftBinaryExpression, predicate);
            }
            if (binaryExpression.Right.NodeType == ExpressionType.LessThan ||
                binaryExpression.Right.NodeType == ExpressionType.LessThanOrEqual ||
                binaryExpression.Right.NodeType == ExpressionType.GreaterThan ||
                binaryExpression.Right.NodeType == ExpressionType.GreaterThanOrEqual ||
                binaryExpression.Right.NodeType == ExpressionType.Equal)
            {
                rightBinaryExpression = (BinaryExpression)binaryExpression.Right;
                secondResult = FirstAddExecute(source, rightBinaryExpression, predicate, false).ToList();
            }
            else if (binaryExpression.Right.NodeType == ExpressionType.AndAlso)
            {
                rightBinaryExpression = (BinaryExpression)binaryExpression.Right;
                secondResult = And(source, rightBinaryExpression, predicate);
            }
            else if (binaryExpression.Right.NodeType == ExpressionType.OrElse)
            {
                rightBinaryExpression = (BinaryExpression)binaryExpression.Right;
                secondResult = Or(source, rightBinaryExpression, predicate);
            }
            List<TSource> list = firstResult.ToList();
            HashSet<TSource> hashSet = new HashSet<TSource>(list);
            foreach (var item in secondResult)
            {
                if (!hashSet.Contains(item))
                {
                    hashSet.Add(item);
                }
            }
            foreach (var item in hashSet)
            {
                yield return item;
            }
        }

        private static int BuildGreatherThanStartValue(BinaryExpression binaryExpression)
        {
            ConstantExpression constantExpression = null;
            if (binaryExpression.Right.NodeType == ExpressionType.Constant)
            {
                constantExpression = (ConstantExpression)binaryExpression.Right;
            }
            return (int)constantExpression.Value;
        }

        public static IEnumerable<TSource> LessThanOrLessThanOrEqaul<TSource>(
            IndexDefinition<TSource> source, BinaryExpression binaryExpression,
            Func<TSource, bool> predicate, LogicalEnum logicalEnum,
            bool recalculateIndex = false)
        {
            IndexDefinition<TSource> index = (IndexDefinition<TSource>)source;
            ConstantExpression constantExpression = null;
            MemberExpression memberExpression = null;
            object indexDefinition = null;
            if (binaryExpression.Left.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = (MemberExpression)binaryExpression.Left;
                var member = memberExpression.Member.Name;
                indexDefinition = index.Find(member, IndexType.UnequalIndex, recalculateIndex);
            }
            else if (binaryExpression.Left.NodeType == ExpressionType.Constant)
            {
                constantExpression = (ConstantExpression)binaryExpression.Left;
            }
            if (binaryExpression.Right.NodeType == ExpressionType.Constant)
            {
                constantExpression = (ConstantExpression)binaryExpression.Right;
            }
            else if (binaryExpression.Right.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = (MemberExpression)binaryExpression.Right;
                var member = memberExpression.Member.Name;
                indexDefinition = index.Find(member, IndexType.UnequalIndex, recalculateIndex);
            }
            if (indexDefinition == null)
            {
                foreach (var innerItem in index.Source.Where(predicate))
                {
                    yield return innerItem;
                }
            }
            else
            {
                dynamic indexSource =
                    indexDefinition.GetType().GetProperty("IndexCollection").GetValue(indexDefinition, null);
                Type typeKey = indexDefinition.GetType().GetGenericArguments()[1];
                dynamic items = null;
                if (logicalEnum == LogicalEnum.LessThanOrEqual)
                    items = indexSource.SmallerThanOrEqualEnumerator(Converting(constantExpression.Value, constantExpression.Type));
                else
                    items = indexSource.SmallerThanEnumerator(Converting(constantExpression.Value, constantExpression.Type));
                foreach (var item in items)
                {
                    foreach (var inneritem in item)
                    {
                        yield return inneritem;
                    }
                }
            }
        }

        public static dynamic Converting(object source, Type dest)
        {
            return Convert.ChangeType(source, dest);
        }

        public static IEnumerable<TSource> LessThanOrLessThanOrEqaul<TSource>(
            IndexDefinition<TSource> source, IEnumerable<TSource> firstResult,
            BinaryExpression binaryExpression, Func<TSource, bool> predicate,
            LogicalEnum logicalEnum, bool recalculateIndex = false)
        {
            IndexDefinition<TSource> index = (IndexDefinition<TSource>)source;
            index = index.Copy();
            index.Source = firstResult;
            ConstantExpression constantExpression = null;
            MemberExpression memberExpression = null;
            object indexDefinition = null;
            if (binaryExpression.Left.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = (MemberExpression)binaryExpression.Left;
                var member = memberExpression.Member.Name;
                indexDefinition = index.Find(member, IndexType.UnequalIndex, recalculateIndex);
            }
            else if (binaryExpression.Left.NodeType == ExpressionType.Constant)
            {
                constantExpression = (ConstantExpression)binaryExpression.Left;
            }
            if (binaryExpression.Right.NodeType == ExpressionType.Constant)
            {
                constantExpression = (ConstantExpression)binaryExpression.Right;
            }
            else if (binaryExpression.Right.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = (MemberExpression)binaryExpression.Right;
                var member = memberExpression.Member.Name;
                indexDefinition = index.Find(member, IndexType.UnequalIndex, recalculateIndex);
            }
            if (indexDefinition == null)
            {
                foreach (var innerItem in index.Source.Where(predicate))
                {
                    yield return innerItem;
                }
            }
            else
            {
                dynamic indexSource =
                    indexDefinition.GetType().GetProperty("IndexCollection").GetValue(indexDefinition, null);
                Type typeKey = indexDefinition.GetType().GetGenericArguments()[1];
                //int min = indexSource.Min();
                //int counter = min;
                ParameterExpression param = Expression.Parameter(typeof(int), "param");
                dynamic items = null;
                if (logicalEnum == LogicalEnum.LessThanOrEqual)
                    items = indexSource.SmallerThanOrEqualEnumerator(Converting(constantExpression.Value, constantExpression.Type));
                else
                    items = indexSource.SmallerThanEnumerator(Converting(constantExpression.Value, constantExpression.Type));
                foreach (var item in items)
                {
                    foreach (var inneritem in item)
                    {
                        yield return inneritem;
                    }
                }
            }
        }

        public static IEnumerable<TSource> GreaterThanOrGreaterThanOrEqual<TSource>(
            IndexDefinition<TSource> source,
            BinaryExpression binaryExpression, Func<TSource, bool> predicate,
            LogicalEnum logicalEnum, bool recalculateIndex = false)
        {
            IndexDefinition<TSource> index = (IndexDefinition<TSource>)source;
            ConstantExpression constantExpression = null;
            MemberExpression memberExpression = null;
            object indexDefinition = null;
            if (binaryExpression.Left.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = (MemberExpression)binaryExpression.Left;
                var member = memberExpression.Member.Name;
                indexDefinition = index.Find(member, IndexType.UnequalIndex, recalculateIndex);
            }
            else if (binaryExpression.Left.NodeType == ExpressionType.Constant)
            {
                constantExpression = (ConstantExpression)binaryExpression.Left;
            }
            if (binaryExpression.Right.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = (MemberExpression)binaryExpression.Right;
                var member = memberExpression.Member.Name;
                indexDefinition = index.Find(member, IndexType.UnequalIndex, recalculateIndex);
            }
            else if (binaryExpression.Right.NodeType == ExpressionType.Constant)
            {
                constantExpression = (ConstantExpression)binaryExpression.Right;
            }
            if (indexDefinition == null || constantExpression.Type != typeof(int))
            {
                foreach (var innerItem in index.Source.Where(predicate))
                {
                    yield return innerItem;
                }
            }
            else
            {
                dynamic indexSource =
                    indexDefinition.GetType().GetProperty("IndexCollection").GetValue(indexDefinition, null);
                Type typeKey = indexDefinition.GetType().GetGenericArguments()[1];
                dynamic items = null;
                if (logicalEnum == LogicalEnum.GreaterThanOrEqual)
                    items = indexSource.BiggerThanOrEqualEnumerator(Converting(constantExpression.Value, constantExpression.Type));
                else
                    items = indexSource.BiggerThanEnumerator(Converting(constantExpression.Value, constantExpression.Type));
                foreach (var item in items)
                {
                    foreach (var inneritem in item)
                    {
                        yield return inneritem;
                    }
                }
            }
        }

        public static IEnumerable<TSource> GreaterThanOrGreaterThanOrEqual<TSource>(
            IndexDefinition<TSource> source,
            IEnumerable<TSource> firstResult, BinaryExpression binaryExpression,
            Func<TSource, bool> predicate, LogicalEnum logicalEnum,
            bool recalculateIndex = false)
        {
            IndexDefinition<TSource> index = (IndexDefinition<TSource>)source;
            index = index.Copy();
            index.Source = firstResult;
            ConstantExpression constantExpression = null;
            MemberExpression memberExpression = null;
            object indexDefinition = null;
            if (binaryExpression.Left.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = (MemberExpression)binaryExpression.Left;
                var member = memberExpression.Member.Name;
                indexDefinition = index.Find(member, IndexType.UnequalIndex, recalculateIndex);
            }
            else if (binaryExpression.Left.NodeType == ExpressionType.Constant)
            {
                constantExpression = (ConstantExpression)binaryExpression.Left;
            }
            if (binaryExpression.Right.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = (MemberExpression)binaryExpression.Right;
                var member = memberExpression.Member.Name;
                indexDefinition = index.Find(member, IndexType.UnequalIndex, recalculateIndex);
            }
            else if (binaryExpression.Right.NodeType == ExpressionType.Constant)
            {
                constantExpression = (ConstantExpression)binaryExpression.Right;
            }
            if (indexDefinition == null || constantExpression.Type != typeof(int))
            {
                foreach (var innerItem in index.Source.Where(predicate))
                {
                    yield return innerItem;
                }
            }
            else
            {
                dynamic indexSource =
                    indexDefinition.GetType().GetProperty("IndexCollection").GetValue(indexDefinition, null);
                Type typeKey = indexDefinition.GetType().GetGenericArguments()[1];
                dynamic items = null;
                if (logicalEnum == LogicalEnum.GreaterThanOrEqual)
                    items = indexSource.BiggerThanOrEqualEnumerator(Converting(constantExpression.Value, constantExpression.Type));
                else
                    items = indexSource.BiggerThanEnumerator(Converting(constantExpression.Value, constantExpression.Type));
                foreach (var item in items)
                {
                    foreach (var inneritem in item)
                    {
                        yield return inneritem;
                    }
                }                
            }
        }

        public static IEnumerable<TSource> Equal<TSource>(
            IndexDefinition<TSource> source, BinaryExpression binaryExpression,
            Func<TSource, bool> predicate, bool recalculateIndex = false)
        {
            IndexDefinition<TSource> index = (IndexDefinition<TSource>)source;
            ConstantExpression constantExpression = null;
            MemberExpression memberExpression = null;
            object indexDefinition = null;
            if (binaryExpression.Left.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = (MemberExpression)binaryExpression.Left;
                var member = memberExpression.Member.Name;
                indexDefinition = index.Find(member, IndexType.EqualIndex);
            }
            if (binaryExpression.Right.NodeType == ExpressionType.Constant)
            {
                constantExpression = (ConstantExpression)binaryExpression.Right;
            }
            if (indexDefinition == null)
            {
                foreach (var innerItem in index.Source.Where(predicate))
                {
                    yield return innerItem;
                }
            }
            else
            {
                dynamic indexSource =
                    indexDefinition.GetType().GetProperty("IndexCollection").GetValue(indexDefinition, null);
                Type typeKey = indexDefinition.GetType().GetGenericArguments()[1];
                if (indexSource.Contains(constantExpression.Value))
                {
                    var items = indexSource.GroupKey(constantExpression.Value);
                    foreach (var item in items)
                    {
                        yield return item;
                    }
                }
            }
        }

        public static IEnumerable<TSource> Equal<TSource>(
            IndexDefinition<TSource> source, IEnumerable<TSource> firstResult,
            BinaryExpression binaryExpression, Func<TSource, bool> predicate,
            bool recalculateIndex = false)
        {
            IndexDefinition<TSource> index = (IndexDefinition<TSource>)source;
            index = index.Copy();
            index.Source = firstResult;
            ConstantExpression constantExpression = null;
            MemberExpression memberExpression = null;
            object indexDefinition = null;
            if (binaryExpression.Left.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = (MemberExpression)binaryExpression.Left;
                var member = memberExpression.Member.Name;
                indexDefinition = index.Find(member, IndexType.EqualIndex, true);
            }
            if (binaryExpression.Right.NodeType == ExpressionType.Constant)
            {
                constantExpression = (ConstantExpression)binaryExpression.Right;
            }
            if (indexDefinition == null)
            {
                foreach (var innerItem in index.Source.Where(predicate))
                {
                    yield return innerItem;
                }
            }
            else
            {
                dynamic indexSource =
                    indexDefinition.GetType().GetProperty("IndexCollection").GetValue(indexDefinition, null);
                Type typeKey = indexDefinition.GetType().GetGenericArguments()[1];
                if (indexSource.Contains(constantExpression.Value))
                {
                    var item = indexSource.Get(constantExpression.Value);
                    yield return item;
                }
            }
        }
    }
}
