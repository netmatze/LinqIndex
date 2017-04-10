using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqIndex.IndexBuilding
{
    public class Tree<TInnerKey, TInnerElement> where TInnerKey : IComparable<TInnerKey>
    {
        private TreeNode<TInnerKey, TInnerElement> rootTreeNode;

        public TreeNode<TInnerKey, TInnerElement> RootTreeNode
        {
            get { return rootTreeNode; }
            set { rootTreeNode = value; }
        }

        public void Insert(TreeNode<TInnerKey, TInnerElement> treeNode)
        {
            TreeNode<TInnerKey, TInnerElement> foundTreeNode = FindTreeNode(rootTreeNode, treeNode.Key);
            if (foundTreeNode == null)
            {
                AddTreeNode(rootTreeNode, treeNode.Key, treeNode.Element); 
            }
        }

        public void Insert(TInnerKey key, TInnerElement element)
        {
            TreeNode<TInnerKey, TInnerElement> foundTreeNode = FindTreeNode(rootTreeNode, key);
            if (foundTreeNode == null)
            {
                AddTreeNode(rootTreeNode, key, element);
            }
        }

        public void Print()
        {
            Console.WriteLine(string.Format(" {0} ({1}, {2}, {3}) ", 
                rootTreeNode.Key, rootTreeNode.Leftbalance, rootTreeNode.Rightbalance, rootTreeNode.IsLeftHeavy));
            PrintTreeNode(rootTreeNode.LeftTreeNode);
            PrintTreeNode(rootTreeNode.RightTreeNode);
        }

        private void PrintTreeNode(TreeNode<TInnerKey, TInnerElement> treeNode)
        {
            if (treeNode != null)
            {
                Console.WriteLine(string.Format(" {0} ({1}, {2}, {3}) ) ", 
                    treeNode.Key, treeNode.Leftbalance, treeNode.Rightbalance, treeNode.IsLeftHeavy));
                PrintTreeNode(treeNode.LeftTreeNode);
                PrintTreeNode(treeNode.RightTreeNode);
            }
        }

        public void Delete(TInnerKey key)
        {
            TreeNode<TInnerKey, TInnerElement> foundTreeNode = FindTreeNode(rootTreeNode, key);
            if (foundTreeNode == null)
            {
                DeleteTreeNode(rootTreeNode, key);
            }
        }

        public TreeNode<TInnerKey, TInnerElement> Find(TInnerKey key)
        {
            return FindTreeNode(rootTreeNode, key);
        }

        public bool FindKey(TInnerKey key)
        {
            if (FindTreeNode(rootTreeNode, key) != null)
            {
                return true;
            }
            return false;
        }

        private List<TreeNode<TInnerKey, TInnerElement>> list;

        public List<TreeNode<TInnerKey, TInnerElement>> TraverseInOrder()
        {                           
            list = new List<TreeNode<TInnerKey, TInnerElement>>();
            InOrder(rootTreeNode);
            return list;
        }

        public void InOrder(TreeNode<TInnerKey, TInnerElement> root) 
        {
            if (!(root == null)) 
            {
                InOrder(root.LeftTreeNode);
                list.Add(root);
                InOrder(root.RightTreeNode);
            }
        }

        private TreeNode<TInnerKey, TInnerElement> SingeRotateRight(TreeNode<TInnerKey, TInnerElement> treeNode)
        {            
            TreeNode<TInnerKey, TInnerElement> tempLeftTreeNode = treeNode.LeftTreeNode;            
            treeNode.LeftTreeNode = tempLeftTreeNode.RightTreeNode;
            tempLeftTreeNode.RightTreeNode = treeNode;                                   
            if (treeNode == rootTreeNode)
            {
                rootTreeNode = tempLeftTreeNode;
            }
            tempLeftTreeNode.Rightbalance = 0;
            tempLeftTreeNode.Leftbalance = 0;
            RecalculateBalance(tempLeftTreeNode);
            return tempLeftTreeNode;
        }

        private TreeNode<TInnerKey, TInnerElement> DoubleRotateRight(TreeNode<TInnerKey, TInnerElement> treeNode)
        {            
            TreeNode<TInnerKey, TInnerElement> leftRotatedleftTreeNode = SingeRotateLeft(treeNode.LeftTreeNode);
            treeNode.LeftTreeNode = leftRotatedleftTreeNode;
            TreeNode<TInnerKey, TInnerElement> doublerotatedTreeNode = SingeRotateRight(treeNode);            
            return doublerotatedTreeNode;
        }

        private TreeNode<TInnerKey, TInnerElement> SingeRotateLeft(TreeNode<TInnerKey, TInnerElement> treeNode)
        {
            TreeNode<TInnerKey, TInnerElement> tempRightTreeNode = treeNode.RightTreeNode;
            treeNode.RightTreeNode = tempRightTreeNode.LeftTreeNode;
            tempRightTreeNode.LeftTreeNode = treeNode;
            if (treeNode == rootTreeNode)
            {
                rootTreeNode = tempRightTreeNode;
            }
            tempRightTreeNode.Rightbalance = 0;
            tempRightTreeNode.Leftbalance = 0;
            RecalculateBalance(tempRightTreeNode);
            return tempRightTreeNode;
        }

        private TreeNode<TInnerKey, TInnerElement> DoubleRotateLeft(TreeNode<TInnerKey, TInnerElement> treeNode)
        {            
            TreeNode<TInnerKey, TInnerElement> rigthRotatedrigthTreeNode = SingeRotateRight(treeNode.RightTreeNode);
            treeNode.RightTreeNode = rigthRotatedrigthTreeNode;
            TreeNode<TInnerKey, TInnerElement> doublerotatedTreeNode = SingeRotateLeft(treeNode);
            return doublerotatedTreeNode;
        }

        private TreeNode<TInnerKey, TInnerElement> RecalculateBalance(TreeNode<TInnerKey, TInnerElement> treeNode)
        {
            if (treeNode.RightTreeNode == null)
            {
                treeNode.Rightbalance = 0;                
            }            
            else
            {
                TreeNode<TInnerKey, TInnerElement> localTreeNode = RecalculateBalance(treeNode.RightTreeNode);
                if (localTreeNode.Rightbalance > localTreeNode.Leftbalance)
                    treeNode.Rightbalance = localTreeNode.Rightbalance + 1;
                else
                    treeNode.Rightbalance = localTreeNode.Leftbalance + 1;
                if (treeNode.Leftbalance > treeNode.Rightbalance + 1)
                {
                    treeNode.IsLeftHeavy = true;
                }
                else
                {
                    treeNode.IsLeftHeavy = false;
                }
            }
            if (treeNode.LeftTreeNode == null)
            {
                treeNode.Leftbalance = 0;                
            }
            else
            {
                TreeNode<TInnerKey, TInnerElement> localTreeNode = RecalculateBalance(treeNode.LeftTreeNode);
                if (localTreeNode.Rightbalance > localTreeNode.Leftbalance)
                    treeNode.Leftbalance = localTreeNode.Rightbalance + 1;
                else
                    treeNode.Leftbalance = localTreeNode.Leftbalance + 1;
                if (treeNode.Leftbalance > treeNode.Rightbalance + 1)
                {
                    treeNode.IsLeftHeavy = true;
                }
                else
                {
                    treeNode.IsLeftHeavy = false;
                }
            }
            return treeNode;
        }

        private void DeleteTreeNode(TreeNode<TInnerKey, TInnerElement> treeNode, TInnerKey key)
        {
            if (rootTreeNode != null)
            {
                
            }
        }

        private TreeNode<TInnerKey, TInnerElement> AddTreeNode(TreeNode<TInnerKey, TInnerElement> treeNode, TInnerKey key, TInnerElement element)
        {
            if (treeNode == null)
            {
                rootTreeNode = new TreeNode<TInnerKey, TInnerElement>(key, element, 0);
                return rootTreeNode;
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
                            TreeNode<TInnerKey, TInnerElement> returnTreeNode = AddTreeNode(treeNode.LeftTreeNode, key, element);
                            treeNode.LeftTreeNode = returnTreeNode;
                            if (returnTreeNode != null)
                            {                                                                                                   
                                if (treeNode.Leftbalance > treeNode.Rightbalance + 1)
                                {
                                    //Console.WriteLine("Left Unbalanced " + treeNode.Key);
                                    if (!treeNode.IsLeftHeavy)
                                    {
                                        if (treeNode.RightTreeNode.Rightbalance < treeNode.RightTreeNode.Leftbalance)
                                        {
                                            return DoubleRotateLeft(treeNode);
                                        }
                                        else
                                        {
                                            return SingeRotateLeft(treeNode);
                                        }
                                    }
                                    else
                                    {
                                        if (treeNode.LeftTreeNode.Leftbalance < treeNode.LeftTreeNode.Rightbalance)
                                        {
                                            return DoubleRotateRight(treeNode);
                                        }
                                        else
                                        {
                                            return SingeRotateRight(treeNode);
                                        }
                                    }
                                }
                                if (treeNode.Leftbalance + 1 < treeNode.Rightbalance)
                                {
                                    //Console.WriteLine("Right Unbalanced " + treeNode.Key);
                                    if (!treeNode.IsLeftHeavy)
                                    {
                                        if (treeNode.RightTreeNode.Rightbalance < treeNode.RightTreeNode.Leftbalance)
                                        {
                                            return DoubleRotateLeft(treeNode);
                                        }
                                        else
                                        {
                                            return SingeRotateLeft(treeNode);
                                        }
                                    }
                                    else
                                    {
                                        if (treeNode.LeftTreeNode.Leftbalance < treeNode.LeftTreeNode.Rightbalance)
                                        {
                                            return DoubleRotateRight(treeNode);
                                        }
                                        else
                                        {
                                            return SingeRotateRight(treeNode);
                                        }
                                    }                           
                                }                               
                            }
                            RecalculateBalance(treeNode);
                            return treeNode;                            
                        }
                        else
                        {
                            treeNode.LeftTreeNode = new TreeNode<TInnerKey, TInnerElement>(key, element, 0);
                            RecalculateBalance(treeNode);
                            return treeNode;
                        }                        
                    }
                    else if (value == -1)
                    {
                        if (treeNode.RightTreeNode != null)
                        {
                            TreeNode<TInnerKey, TInnerElement> returnTreeNode = AddTreeNode(treeNode.RightTreeNode, key, element);
                            treeNode.RightTreeNode = returnTreeNode;
                            if (returnTreeNode != null)
                            {                                                                                                  
                                if (treeNode.Leftbalance > treeNode.Rightbalance + 1)
                                {
                                    //Console.WriteLine("Left Unbalanced " + treeNode.Key);
                                    if (!treeNode.IsLeftHeavy)
                                    {
                                        if (treeNode.RightTreeNode.Rightbalance < treeNode.RightTreeNode.Leftbalance)
                                        {
                                            return DoubleRotateLeft(treeNode);
                                        }
                                        else
                                        {
                                            return SingeRotateLeft(treeNode);
                                        }
                                    }
                                    else
                                    {
                                        if (treeNode.LeftTreeNode.Leftbalance < treeNode.LeftTreeNode.Rightbalance)
                                        {
                                            return DoubleRotateRight(treeNode);
                                        }
                                        else
                                        {
                                            return SingeRotateRight(treeNode);
                                        }
                                    }
                                }
                                if (treeNode.Leftbalance + 1 < treeNode.Rightbalance)
                                {
                                    //Console.WriteLine("Right Unbalanced " + treeNode.Key);
                                    if (!treeNode.IsLeftHeavy)
                                    {
                                        if (treeNode.RightTreeNode.Rightbalance < treeNode.RightTreeNode.Leftbalance)
                                        {
                                            return DoubleRotateLeft(treeNode);
                                        }
                                        else
                                        {
                                            return SingeRotateLeft(treeNode);
                                        }
                                    }
                                    else
                                    {
                                        if (treeNode.LeftTreeNode.Leftbalance < treeNode.LeftTreeNode.Rightbalance)
                                        {
                                            return DoubleRotateRight(treeNode);
                                        }
                                        else
                                        {
                                            return SingeRotateRight(treeNode);
                                        }
                                    }                           
                                }                                
                            }
                            RecalculateBalance(treeNode);
                            return treeNode;
                        }
                        else
                        {
                            treeNode.RightTreeNode = new TreeNode<TInnerKey, TInnerElement>(key, element, 0);
                            RecalculateBalance(treeNode);
                            return treeNode; 
                        }
                    }
                }                
                return null;
            }
        }           

        private TreeNode<TInnerKey, TInnerElement> FindTreeNode(TreeNode<TInnerKey, TInnerElement> treeNode, TInnerKey key)
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
    }       

    public class TreeNode<TInnerKey, TInnerElement>
    {
        private int leftBalance;

        public int Leftbalance
        {
            get { return leftBalance; }
            set { leftBalance = value; }
        }

        private int rightBalance;

        public int Rightbalance
        {
            get { return rightBalance; }
            set { rightBalance = value; }
        }

        public int Maxbalance
        {
            get
            {
                if (leftBalance > rightBalance)
                    return leftBalance;
                else
                    return rightBalance; 
            }           
        }

        private bool isLeftHeavy;

        public bool IsLeftHeavy
        {
            get { return isLeftHeavy; }
            set { isLeftHeavy = value; }
        }

        private TreeNode<TInnerKey, TInnerElement> leftTreeNode;

        public TreeNode<TInnerKey, TInnerElement> LeftTreeNode
        {
            get { return leftTreeNode; }
            set { leftTreeNode = value; }
        }

        private TreeNode<TInnerKey, TInnerElement> rightTreeNode;

        public TreeNode<TInnerKey, TInnerElement> RightTreeNode
        {
            get { return rightTreeNode; }
            set { rightTreeNode = value; }
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

        public TreeNode(TInnerKey key, TInnerElement element, int treeDept)
        {
            this.key = key;
            this.element = element;
            this.leftBalance = treeDept;
            this.rightBalance = treeDept;
        }
    }
}
