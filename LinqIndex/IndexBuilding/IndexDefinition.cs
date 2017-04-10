using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;

namespace LinqIndex.IndexBuilding
{
    public class IndexDefinition<TSource>
    {
        public List<object> indexes = new List<object>();
        public IEnumerable<TSource> Source { get; set; }

        public IndexDefinition<TSource> Copy()
        {
            return new IndexDefinition<TSource>() { indexes = this.indexes };
        }

        public void Add<TKey>(IndexPartDefinition<TSource, TKey> indexPartDefinition) where TKey : IComparable<TKey>
        {
            indexes.Add(indexPartDefinition);
        }

        private ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        public object Find(string member, IndexType indexType, bool recalculateIndex = false)
        {
            foreach (var item in indexes)
            {
                Expression expression = (Expression)item.GetType().GetProperty("KeySelector").GetValue(item, null);
                if (expression.NodeType == ExpressionType.Lambda)
                {
                    LambdaExpression lambdaExpression = (LambdaExpression)expression;
                    MemberExpression memberExpression = null;
                    memberExpression = (MemberExpression)lambdaExpression.Body;
                    if (memberExpression.NodeType == ExpressionType.MemberAccess)
                    {
                        var expressionMember = memberExpression.Member;
                        if (expressionMember.Name == member)
                        {
                            object resultItem = item.GetType().
                                InvokeMember("BuildIndex", System.Reflection.BindingFlags.InvokeMethod,
                                null, item, new object[] { this.Source, indexType, recalculateIndex });
                            return item;
                        }
                    }
                }
            }
            return null;
        }
    }
}
