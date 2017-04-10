using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace LinqIndex.IndexBuilding
{   
    public sealed class Index<TKey, TElement> : IIndex<TKey, TElement> where TKey : IComparable<TKey>
    {
        public FastTree<TKey, TElement> tree; 

        public Index()
        {
           tree = new FastTree<TKey, TElement>();
        }

        public void Add(TKey key, TElement element)
        {            
            tree.Insert(key, element);         
        }

        internal void Delete(TKey key)
        {                     
            if (!tree.FindKey(key))
            {
                tree.Delete(key);
            }
        }

        public TElement this[TKey key]
        {
            get
            {
                FastTreeNode<TKey, TElement> treeNode = tree.Find(key);
                if(treeNode != null)
                {
                    return treeNode.Element;
                }
                return default(TElement);
            }
        }

        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            List<Grouping<TKey, TElement>> groupingList = new List<Grouping<TKey, TElement>>();           
            foreach (var item in tree.TraverseInOrder())
            {
                List<TElement> innerList = new List<TElement>();
                innerList.Add(item.Element);
                Grouping<TKey, TElement> grouping = new Grouping<TKey, TElement>(item.Key, innerList);
                groupingList.Add(grouping);
            }
            return groupingList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region IIndex<TKey,TElement> Member

        public TKey Max()
        {
            var treeNodes = tree.TraverseInOrder();
            var treeNode = treeNodes[treeNodes.Count - 1];
            return treeNode.Key;
        }

        public TKey Min()
        {
            var treeNodes = tree.TraverseInOrder();
            var treeNode = treeNodes[0];
            return treeNode.Key;
        }

        public IEnumerable<Grouping<TKey, TElement>> BiggerThanEnumerator(TKey key)
        {            
            List<Grouping<TKey, TElement>> list = new List<Grouping<TKey,TElement>>();
            IEnumerable<Grouping<TKey, TElement>> resultList = 
                tree.FindTreeNodesBiggerThanKey(tree.RootTreeNode, key, list);
            return resultList;
        }

        public IEnumerable<Grouping<TKey, TElement>> BiggerThanOrEqualEnumerator(TKey key)
        {
            List<Grouping<TKey, TElement>> list = new List<Grouping<TKey, TElement>>();
            IEnumerable<Grouping<TKey, TElement>> resultList =
                tree.FindTreeNodesBiggerOrEqualThanKey(tree.RootTreeNode, key, list);
            return resultList;
        }

        public IEnumerable<Grouping<TKey, TElement>> SmallerThanEnumerator(TKey key)
        {
            List<Grouping<TKey, TElement>> list = new List<Grouping<TKey, TElement>>();
            IEnumerable<Grouping<TKey, TElement>> resultList = 
                tree.FindTreeNodesSmallerThanKey(tree.RootTreeNode, key, list);
            return resultList;
        }

        public IEnumerable<Grouping<TKey, TElement>> SmallerThanOrEqualEnumerator(TKey key)
        {
            List<Grouping<TKey, TElement>> list = new List<Grouping<TKey, TElement>>();
            IEnumerable<Grouping<TKey, TElement>> resultList =
                tree.FindTreeNodesSmallerThanOrEqualKey(tree.RootTreeNode, key, list);
            return resultList;
        }

        public bool Contains(TKey key)
        {            
            return tree.FindKey(key);         
        }

        #endregion
    }   
}
