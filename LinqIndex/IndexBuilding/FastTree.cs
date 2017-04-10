using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqIndex.IndexBuilding
{
    public class FastTree<TInnerKey, TInnerElement> where TInnerKey : IComparable<TInnerKey>
    {
        private FastTreeNode<TInnerKey, TInnerElement> rootTreeNode;

        public FastTreeNode<TInnerKey, TInnerElement> RootTreeNode
        {
            get { return rootTreeNode; }
            set { rootTreeNode = value; }
        }

        public void Insert(TInnerKey key, TInnerElement element)
        {
            FastTreeNode<TInnerKey, TInnerElement> treeNode = new FastTreeNode<TInnerKey, TInnerElement>(key, element);
            Insert(treeNode);
        }

        private List<FastTreeNode<TInnerKey, TInnerElement>> list;

        public List<FastTreeNode<TInnerKey, TInnerElement>> TraverseInOrder()
        {
            list = new List<FastTreeNode<TInnerKey, TInnerElement>>();
            InOrder(rootTreeNode);
            return list;
        }

        public void InOrder(FastTreeNode<TInnerKey, TInnerElement> root)
        {
            if (!(root == null))
            {
                InOrder(root.LeftTreeNode);
                list.Add(root);
                InOrder(root.RightTreeNode);
            }
        }

        public void Print()
        {
            Console.WriteLine(string.Format(" {0} ",
                rootTreeNode.Key));
            PrintTreeNode(rootTreeNode.LeftTreeNode);
            PrintTreeNode(rootTreeNode.RightTreeNode);
        }

        private void PrintTreeNode(FastTreeNode<TInnerKey, TInnerElement> treeNode)
        {
            if (treeNode != null)
            {
                Console.WriteLine(string.Format(" {0} ",
                    treeNode.Key));
                PrintTreeNode(treeNode.LeftTreeNode);
                PrintTreeNode(treeNode.RightTreeNode);
            }
        }

        public bool FindKey(TInnerKey key)
        {
            if (FindTreeNode(rootTreeNode, key) != null)
            {
                return true;
            }
            return false;
        }

        public FastTreeNode<TInnerKey, TInnerElement> Find(TInnerKey key)
        {
            return FindTreeNode(rootTreeNode, key);
        }

        public IEnumerable<Grouping<TInnerKey, TInnerElement>> FindTreeNodesBiggerThanKey(
            FastTreeNode<TInnerKey, TInnerElement> treeNode, TInnerKey key, List<Grouping<TInnerKey, TInnerElement>> list)
        {
            if (treeNode == null)
            {
                return list;
            }
            else
            {
                int compareValue = Comparer.Default.Compare(treeNode.Key, key);
                if (compareValue == 1)                                      
                {
                    List<TInnerElement> innerList = new List<TInnerElement>();
                    innerList.Add(treeNode.Element);
                    Grouping<TInnerKey, TInnerElement> grouping = new Grouping<TInnerKey, TInnerElement>(treeNode.Key, innerList);
                    list.Add(grouping);
                    FindTreeNodesBiggerThanKey(treeNode.RightTreeNode, key, list);
                    FindTreeNodesBiggerThanKey(treeNode.LeftTreeNode, key, list);
                }
                else if (compareValue == -1 || compareValue == 0)
                {
                    FindTreeNodesBiggerThanKey(treeNode.RightTreeNode, key, list);
                }                
                return list;
            }
        }

        public IEnumerable<Grouping<TInnerKey, TInnerElement>> FindTreeNodesBiggerOrEqualThanKey(
           FastTreeNode<TInnerKey, TInnerElement> treeNode, TInnerKey key, List<Grouping<TInnerKey, TInnerElement>> list)
        {
            if (treeNode == null)
            {
                return list;
            }
            else
            {
                int compareValue = Comparer.Default.Compare(treeNode.Key, key);
                if (compareValue == 1 || compareValue == 0)
                {
                    List<TInnerElement> innerList = new List<TInnerElement>();
                    innerList.Add(treeNode.Element);
                    Grouping<TInnerKey, TInnerElement> grouping = new Grouping<TInnerKey, TInnerElement>(treeNode.Key, innerList);
                    list.Add(grouping);
                    FindTreeNodesBiggerOrEqualThanKey(treeNode.RightTreeNode, key, list);
                    FindTreeNodesBiggerOrEqualThanKey(treeNode.LeftTreeNode, key, list);
                }
                else if (compareValue == -1)
                {
                    FindTreeNodesBiggerOrEqualThanKey(treeNode.RightTreeNode, key, list);
                }
                return list;
            }
        }

        public List<Grouping<TInnerKey, TInnerElement>> FindTreeNodesSmallerThanKey(
            FastTreeNode<TInnerKey, TInnerElement> treeNode, TInnerKey key, List<Grouping<TInnerKey, TInnerElement>> list)
        {
            if (treeNode == null)
            {
                return list;
            }
            else
            {
                int compareValue = Comparer.Default.Compare(treeNode.Key, key);
                if (compareValue == -1)
                {
                    List<TInnerElement> innerList = new List<TInnerElement>();
                    innerList.Add(treeNode.Element);
                    Grouping<TInnerKey, TInnerElement> grouping = new Grouping<TInnerKey, TInnerElement>(treeNode.Key, innerList);
                    list.Add(grouping);
                    FindTreeNodesSmallerThanKey(treeNode.LeftTreeNode, key, list);
                    FindTreeNodesSmallerThanKey(treeNode.RightTreeNode, key, list);
                }
                else if (compareValue == 1 || compareValue == 0)
                {
                    FindTreeNodesSmallerThanKey(treeNode.LeftTreeNode, key, list);
                }
                return list;
            }
        }

        public List<Grouping<TInnerKey, TInnerElement>> FindTreeNodesSmallerThanOrEqualKey(
            FastTreeNode<TInnerKey, TInnerElement> treeNode, TInnerKey key, List<Grouping<TInnerKey, TInnerElement>> list)
        {
            if (treeNode == null)
            {
                return list;
            }
            else
            {
                int compareValue = Comparer.Default.Compare(treeNode.Key, key);
                if (compareValue == -1 || compareValue == 0)
                {
                    List<TInnerElement> innerList = new List<TInnerElement>();
                    innerList.Add(treeNode.Element);
                    Grouping<TInnerKey, TInnerElement> grouping = new Grouping<TInnerKey, TInnerElement>(treeNode.Key, innerList);
                    list.Add(grouping);
                    FindTreeNodesSmallerThanOrEqualKey(treeNode.LeftTreeNode, key, list);
                    FindTreeNodesSmallerThanOrEqualKey(treeNode.RightTreeNode, key, list);
                }
                else if (compareValue == 1)
                {
                    FindTreeNodesSmallerThanOrEqualKey(treeNode.LeftTreeNode, key, list);
                }
                return list;
            }
        }

        private FastTreeNode<TInnerKey, TInnerElement> FindTreeNode(FastTreeNode<TInnerKey, TInnerElement> treeNode, TInnerKey key)
        {
            if (treeNode == null)
            {
                return null;
            }
            else
            {
                if (EqualityComparer<TInnerKey>.Default.Equals(treeNode.Key, key))
                {
                    return treeNode;
                }
                else
                {
                    int value = ((IComparable<TInnerKey>)treeNode.Key).CompareTo(key);
                    if (value == 0)
                    {
                        return treeNode;
                    }
                    else if (value == 1)
                    {
                        if (treeNode.LeftTreeNode != null)
                        {
                            return FindTreeNode(treeNode.LeftTreeNode, key);
                        }
                    }
                    else if (value == -1)
                    {
                        if (treeNode.RightTreeNode != null)
                        {
                            return FindTreeNode(treeNode.RightTreeNode, key);
                        }
                    }
                }
                return null;
            }
        }    

        public void InsertBalance(FastTreeNode<TInnerKey, TInnerElement> treeNode, int balance)
        {
            treeNode.Balance += balance;
            balance = treeNode.Balance;
            if (balance == 0)
                return;
            if (balance == 2)
            {
                if (treeNode.LeftTreeNode.Balance == 1)
                {
                    RightRotation(treeNode);
                }
                else
                {
                    DoubleLeftRotation(treeNode);
                }
            }
            if (balance == -2)
            {
                if (treeNode.RightTreeNode.Balance == -1)
                {
                    LeftRotation(treeNode);
                }
                else
                {
                    DoubleRightRotation(treeNode);
                }
            }
            if (treeNode.ParentTreeNode != null)
            {
                if (treeNode.ParentTreeNode.LeftTreeNode == treeNode)
                {
                    InsertBalance(treeNode.ParentTreeNode, 1);
                }
                else
                {
                    InsertBalance(treeNode.ParentTreeNode, -1);
                }
            }
        }

        public void LeftRotation(FastTreeNode<TInnerKey, TInnerElement> treeNode)
        {
            FastTreeNode<TInnerKey, TInnerElement> tempRightTreeNode = treeNode.RightTreeNode;
            FastTreeNode<TInnerKey, TInnerElement> tempRightLeftTreeNode = tempRightTreeNode.LeftTreeNode;
            FastTreeNode<TInnerKey, TInnerElement> parentNode = treeNode.ParentTreeNode;
            tempRightTreeNode.ParentTreeNode = parentNode;
            tempRightTreeNode.LeftTreeNode = treeNode;
            treeNode.RightTreeNode = tempRightLeftTreeNode;
            treeNode.ParentTreeNode = tempRightTreeNode;            
            if (tempRightLeftTreeNode != null)
            {
                tempRightLeftTreeNode.ParentTreeNode = treeNode;
            }
            if (treeNode == rootTreeNode)
            {
                rootTreeNode = tempRightTreeNode;
            }
            else if (parentNode.RightTreeNode == treeNode)
            {
                parentNode.RightTreeNode = tempRightTreeNode;
            }
            else
            {
                parentNode.LeftTreeNode = tempRightTreeNode;
            }
            tempRightTreeNode.Balance++;
            treeNode.Balance = -tempRightTreeNode.Balance;
        }

        public void RightRotation(FastTreeNode<TInnerKey, TInnerElement> treeNode)
        {
            FastTreeNode<TInnerKey, TInnerElement> tempLeftTreeNode = treeNode.LeftTreeNode;
            FastTreeNode<TInnerKey, TInnerElement> tempLeftRightTreeNode = tempLeftTreeNode.RightTreeNode;
            FastTreeNode<TInnerKey, TInnerElement> parentNode = treeNode.ParentTreeNode;
            tempLeftTreeNode.ParentTreeNode = parentNode;
            tempLeftTreeNode.RightTreeNode = treeNode;
            treeNode.LeftTreeNode = tempLeftRightTreeNode;
            treeNode.ParentTreeNode = tempLeftTreeNode;
            if (tempLeftRightTreeNode != null)
            {
                tempLeftRightTreeNode.ParentTreeNode = treeNode;
            }                 
            if (treeNode == rootTreeNode)
            {
                rootTreeNode = tempLeftTreeNode;
            }
            else if (parentNode.RightTreeNode == treeNode)
            {
                parentNode.RightTreeNode = tempLeftTreeNode;
            }
            else
            {
                parentNode.LeftTreeNode = tempLeftTreeNode;
            }
            tempLeftTreeNode.Balance--;
            treeNode.Balance = -tempLeftTreeNode.Balance;
        }

        public void DoubleLeftRotation(FastTreeNode<TInnerKey, TInnerElement> treeNode)
        {
            FastTreeNode<TInnerKey, TInnerElement> left = treeNode.LeftTreeNode;
            FastTreeNode<TInnerKey, TInnerElement> leftRight = left.RightTreeNode;
            FastTreeNode<TInnerKey, TInnerElement> parent = treeNode.ParentTreeNode;
            FastTreeNode<TInnerKey, TInnerElement> leftRightRight = leftRight.RightTreeNode;
            FastTreeNode<TInnerKey, TInnerElement> leftRightLeft = leftRight.LeftTreeNode;

            leftRight.ParentTreeNode = parent;
            treeNode.LeftTreeNode = leftRightRight;
            left.RightTreeNode = leftRightLeft;
            leftRight.LeftTreeNode = left;
            leftRight.RightTreeNode = treeNode;
            left.ParentTreeNode = leftRight;
            treeNode.ParentTreeNode = leftRight;
            if (leftRightRight != null)
            {
                leftRightRight.ParentTreeNode = treeNode;
            }
            if (leftRightLeft != null)
            {
                leftRightLeft.ParentTreeNode = left;
            }
            if (treeNode == rootTreeNode)
            {
                rootTreeNode = leftRight;
            }
            else if (parent.LeftTreeNode == treeNode)
            {
                parent.LeftTreeNode = leftRight;
            }
            else
            {
                parent.RightTreeNode = leftRight;
            }
            if (leftRight.Balance == -1)
            {
                treeNode.Balance = 0;
                left.Balance = 1;
            }
            else if (leftRight.Balance == 0)
            {
                treeNode.Balance = 0;
                left.Balance = 0;
            }
            else
            {
                treeNode.Balance = -1;
                left.Balance = 0;
            }
            leftRight.Balance = 0;
        }

        public void DoubleRightRotation(FastTreeNode<TInnerKey, TInnerElement> treeNode)
        {
            FastTreeNode<TInnerKey, TInnerElement> right = treeNode.RightTreeNode;
            FastTreeNode<TInnerKey, TInnerElement> rightLeft = right.LeftTreeNode;
            FastTreeNode<TInnerKey, TInnerElement> parent = treeNode.ParentTreeNode;
            FastTreeNode<TInnerKey, TInnerElement> rightLeftLeft = rightLeft.LeftTreeNode;
            FastTreeNode<TInnerKey, TInnerElement> rightLeftRight = rightLeft.RightTreeNode;
            rightLeft.ParentTreeNode = parent;
            treeNode.RightTreeNode = rightLeftLeft;
            right.LeftTreeNode = rightLeftRight;
            rightLeft.RightTreeNode = right;
            rightLeft.LeftTreeNode = treeNode;
            right.ParentTreeNode = rightLeft;
            treeNode.ParentTreeNode = rightLeft;
            if (rightLeftLeft != null)
            {
                rightLeftLeft.ParentTreeNode = treeNode;
            }
            if (rightLeftRight != null)
            {
                rightLeftRight.ParentTreeNode = right;
            }
            if (treeNode == rootTreeNode)
            {
                rootTreeNode = rightLeft;
            }
            else if (parent.RightTreeNode == treeNode)
            {
                parent.RightTreeNode = rightLeft;
            }
            else
            {
                parent.LeftTreeNode = rightLeft;
            }
            if (rightLeft.Balance == 1)
            {
                treeNode.Balance = 0;
                right.Balance = -1;
            }
            else if (rightLeft.Balance == 0)
            {
                treeNode.Balance = 0;
                right.Balance = 0;
            }
            else
            {
                treeNode.Balance = 1;
                right.Balance = 0;
            }
            rightLeft.Balance = 0;
        }

        public void Insert(FastTreeNode<TInnerKey, TInnerElement> treeNode, FastTreeNode<TInnerKey, TInnerElement> startTreeNode = null)
        {                
            if (rootTreeNode == null)
            {
                rootTreeNode = treeNode;             
            }
            else
            {
                FastTreeNode<TInnerKey, TInnerElement> node = null;
                if (startTreeNode == null)
                {
                    node = rootTreeNode;
                }
                else
                {
                    node = startTreeNode;
                }
                int compare = Comparer<TInnerKey>.Default.Compare(treeNode.Key, node.Key);
                if (compare < 0)
                {
                    FastTreeNode<TInnerKey, TInnerElement> leftNode = node.LeftTreeNode;
                    if (leftNode == null)
                    {
                        node.LeftTreeNode = new FastTreeNode<TInnerKey, TInnerElement>(treeNode.Key, treeNode.Element);
                        node.LeftTreeNode.ParentTreeNode = node;
                        InsertBalance(node, 1);
                        return;
                    }
                    else
                    {
                        Insert(treeNode, leftNode);
                    }
                }
                else if (compare > 0)
                {
                    FastTreeNode<TInnerKey, TInnerElement> rightNode = node.RightTreeNode;
                    if (rightNode == null)
                    {
                        node.RightTreeNode = new FastTreeNode<TInnerKey, TInnerElement>(treeNode.Key, treeNode.Element);
                        node.RightTreeNode.ParentTreeNode = node;
                        InsertBalance(node, -1);
                        return;
                    }
                    else
                    {
                        Insert(treeNode, rightNode);
                    }
                }
                else if (compare == 0)
                {
                    FastTreeNode<TInnerKey, TInnerElement> rightNode = node.RightTreeNode;
                    if (rightNode == null)
                    {
                        node.RightTreeNode = new FastTreeNode<TInnerKey, TInnerElement>(treeNode.Key, treeNode.Element);
                        node.RightTreeNode.ParentTreeNode = node;
                        InsertBalance(node, -1);
                        return;
                    }
                    else
                    {
                        Insert(treeNode, rightNode);
                    }
                }
                else
                {
                    node.Element = treeNode.Element;
                }
            }
        }

        public void Delete(TInnerKey key)
        {
            FastTreeNode<TInnerKey, TInnerElement> foundTreeNode = FindTreeNode(rootTreeNode, key);
            if (foundTreeNode == null)
            {
                //DeleteTreeNode(rootTreeNode, key);
            }
        }
    }

    public class FastTreeNode<TInnerKey, TInnerElement>
    {
        private FastTreeNode<TInnerKey, TInnerElement> parentTreeNode;

        public FastTreeNode<TInnerKey, TInnerElement> ParentTreeNode
        {
            get { return parentTreeNode; }
            set { parentTreeNode = value; }
        }

        private FastTreeNode<TInnerKey, TInnerElement> leftTreeNode;

        public FastTreeNode<TInnerKey, TInnerElement> LeftTreeNode
        {
            get { return leftTreeNode; }
            set { leftTreeNode = value; }
        }

        private FastTreeNode<TInnerKey, TInnerElement> rightTreeNode;

        public FastTreeNode<TInnerKey, TInnerElement> RightTreeNode
        {
            get { return rightTreeNode; }
            set { rightTreeNode = value; }
        }

        private int balance;

        public int Balance
        {
            get { return balance; }
            set { balance = value; }
        }

        private TInnerKey key;

        public TInnerKey Key
        {
            get { return key; }
            set { key = value; }
        }

        private TInnerElement element;

        public TInnerElement Element
        {
            get { return element; }
            set { element = value; }
        }

        public FastTreeNode(TInnerKey key, TInnerElement element)
        {
            this.key = key;
            this.element = element;
        }
    }
}
