 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStructure
{

    public interface ITree<T>
    {
        T Root();                                //求树的根结点
        T Parent(T t);                           //求结点t的双亲结点
        T Child(T t, int i);                     //求结点t的第i个子结点
        T RightSibling(T t);                     //求结点t第一个右边兄弟结点
        bool Insert(T s, T t, int i);            //将树s加入树中作为结点t的第i颗子树
        T Delete(T t, int i);                    //删除结点t的第i颗子树
        void Traverse(int TraverseType);         //按某种方式遍历树
        void Clear();                            //清空树
        bool IsEmpty();                          //判断是否为空
        int GetDepth(T t);                          //求树的深度
    }


    /// <summary>
    /// 循环顺序队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class CSeqQueue<T>
    {
        private int maxsize;       //循环顺序队列的容量
        private T[] data;          //数组，用于存储循环顺序队列中的数据元素
        private int front;         //指示最近一个已经离开队列的元素所占有的位置 循环顺序队列的对头
        private int rear;          //指示最近一个进入队列的元素的位置           循环顺序队列的队尾

        public T this[int index]
        {
            get { return data[index]; }
            set { data[index] = value; }
        }

        //容量属性
        public int Maxsize
        {
            get { return maxsize; }
            set { maxsize = value; }
        }

        //对头指示器属性
        public int Front
        {
            get { return front; }
            set { front = value; }
        }

        //队尾指示器属性
        public int Rear
        {
            get { return rear; }
            set { rear = value; }
        }

        public CSeqQueue()
        {

        }

        public CSeqQueue(int size)
        {
            data = new T[size];
            maxsize = size;
            front = rear = -1;
        }

        //判断循环顺序队列是否为满
        public bool IsFull()
        {
            if ((front == -1 && rear == maxsize - 1) || (rear + 1) % maxsize == front)
                return true;
            else
                return false;
        }

        //清空循环顺序列表
        public void Clear()
        {
            front = rear = -1;
        }

        //判断循环顺序队列是否为空
        public bool IsEmpty()
        {
            if (front == rear)
                return true;
            else
                return false;
        }

        //入队操作
        public void EnQueue(T elem)
        {
            if (IsFull())
            {
                Console.WriteLine("Queue is Full !");
                return;
            }
            rear = (rear + 1) % maxsize;
            data[rear] = elem;
        }

        //出队操作
        public T DeQueue()
        {
            if (IsEmpty())
            {
                Console.WriteLine("Queue is Empty !");
                return default(T);
            }
            front = (front + 1) % maxsize;
            return data[front];
        }

        //获取对头数据元素
        public T GetFront()
        {
            if (IsEmpty())
            {
                Console.WriteLine("Queue is Empty !");
                return default(T);
            }
            return data[(front + 1) % maxsize];//front从-1开始
        }

        //求循环顺序队列的长度
        public int GetLength()
        {
            return (rear - front + maxsize) % maxsize;
        }
    }

    /// <summary>
    /// 树的多链表结点类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class MLNode<T>
    {
        private T data;                   //存储结点的数据
        private MLNode<T>[] childs;       //存储子结点的位置

        public MLNode(int max)
        {
            childs = new MLNode<T>[max];
            for (int i = 0; i < childs.Length; i++)
            {
                childs[i] = null;
            }
        }

        public T Data
        {
            get { return data; }
            set { data = value; }
        }

        public MLNode<T>[] Childs
        {
            get { return childs; }
            set { childs = value; }
        }
    }
     

    class MLTree<T> : ITree<MLNode<T>>
    {
        private MLNode<T> head;

        public MLNode<T> Head
        {
            get { return head; }
            set { head = value; }
        }

        public MLTree()
        {
            head = null;
        }

        public MLTree(MLNode<T> node)
        {
            head = node;
        }

        //求树的根结点
        public MLNode<T> Root()
        {
            return head;
        }

        public void Clear()
        {
            head = null;
        }

        //待测试！！！
        public int GetDepth(MLNode <T> root)
        {
            int len;
            if (root == null)
            {
                return 0;
            }
            for (int i = 0; i < root.Childs.Length; i++)
            {
                if (root.Childs[i] != null)
                {
                    len = GetDepth(root.Childs[i]);
                    return len + 1;
                }
            }
            return 0;
        }

        public bool IsEmpty()
        {
            return head == null;
        }

        //求结点t的双亲结点，如果t的双亲结点存在，返回双亲结点，否则返回空
        //按层序遍历的算法进行查找
        public MLNode<T> Parent(MLNode<T> t)
        {
            MLNode<T> temp = head;
            if (IsEmpty() || t == null) return null;
            if (temp.Data.Equals(t.Data)) return null;
            CSeqQueue<MLNode<T>> queue = new CSeqQueue<MLNode<T>>(50);
            queue.EnQueue(temp);
            while (!queue.IsEmpty())
            {
                temp = (MLNode<T>)queue.DeQueue();
                for (int i = 0; i < temp.Childs.Length; i++)
                {
                    if (temp.Childs[i] != null)
                    {
                        if (temp.Childs[i].Data.Equals(t.Data))
                        {
                            return temp;
                        }
                        else
                        {
                            queue.EnQueue(temp.Childs[i]);
                        }
                    }
                }
            }
            return null;
        }

        //求结点t的第i个子结点。如果存在，返回第i个子结点，否则返回空
        //i=0时，表示求第一个子节点
        public MLNode<T> Child(MLNode<T> t, int i)
        {
            if (t != null && i < t.Childs.Length)
            {
                return t.Childs[i];
            }
            else
            {
                return null;
            }
        }

        //求结点t第一个右边兄弟结点，如果存在，返回第一个右边兄弟结点，否则返回空
        public MLNode<T> RightSibling(MLNode<T> t)
        {
            MLNode<T> pt = Parent(t);
            if (pt != null)
            {
                int i = FindRank(t);
                return Child(pt, i + 1);
            }
            else
            {
                return null;
            }
        }

        //查找结点t在兄弟中的排行，成功时返回位置，否则返回-1
        private int FindRank(MLNode<T> t)
        {
            MLNode<T> pt = Parent(t);
            for (int i = 0; i < pt.Childs.Length; i++)
            {
                MLNode<T> temp = pt.Childs[i];
                if (temp != null && temp.Data.Equals(t.Data))
                {
                    return i;
                }
            }
            return -1;
        }

        //查找在树中的结点t，成功是返回t的位置，否则返回null
        private MLNode<T> FindNode(MLNode<T> t)
        {
            if (head.Data.Equals(t.Data)) return head;
            MLNode<T> pt = Parent(t);
            foreach (MLNode<T> temp in pt.Childs)
            {
                if (temp != null && temp.Data.Equals(t.Data))
                {
                    return temp;
                }
            }
            return null;
        }

        //把以s为头结点的树插入到树中作为结点t的第i颗子树。成功返回true
        public bool Insert(MLNode<T> s, MLNode<T> t, int i)
        {
            if (IsEmpty()) head = t;
            t = FindNode(t);
            if (t != null && t.Childs.Length > i)
            {
                t.Childs[i] = s;
                return true;
            }
            else
            {
                return false;
            }
        }

        //删除结点t的第i个子树。返回第i颗子树的根结点。
        public MLNode<T> Delete(MLNode<T> t, int i)
        {
            t = FindNode(t);
            MLNode<T> node = null;
            if (t != null && t.Childs.Length > i)
            {
                node = t.Childs[i];
                t.Childs[i] = null;
            }
            return node;
        }


        //先序遍历
        //根结点->遍历根结点的左子树->遍历根结点的右子树 
        public void preorder(MLNode<T> root)
        {
            if (root == null)
                return;
            Console.WriteLine(root.Data + " ");
            for (int i = 0; i < root.Childs.Length; i++)
            {
                preorder(root.Childs[i]);
            }
        }


        //后序遍历
        //遍历根结点的左子树->遍历根结点的右子树->根结点
        public void postorder(MLNode<T> root)
        {
            if (root == null)
            { return; }
            for (int i = 0; i < root.Childs.Length; i++)
            {
                postorder(root.Childs[i]);
            }
            Console.WriteLine(root.Data + " ");
        }


        //层次遍历
        //引入队列 
        public void LevelOrder(MLNode<T> root)
        {
            Console.WriteLine("遍历开始：");
            if (root == null)
            {
                Console.WriteLine("没有结点！");
                return;
            }

            MLNode<T> temp = root;

            CSeqQueue<MLNode<T>> queue = new CSeqQueue<MLNode<T>>(50);
            queue.EnQueue(temp);
            while (!queue.IsEmpty())
            {
                temp = (MLNode<T>)queue.DeQueue();
                Console.WriteLine(temp.Data + " ");
                for (int i = 0; i < temp.Childs.Length; i++)
                {
                    if (temp.Childs[i] != null)
                    {
                        queue.EnQueue(temp.Childs[i]);
                    }
                }
            }
            Console.WriteLine("遍历结束！");
        }

        //按某种方式遍历树
        //0 先序
        //1 后序
        //2 层序
        public void Traverse(int TraverseType)
        {
            if (TraverseType == 0) preorder(head);
            else if (TraverseType == 1) postorder(head);
            else LevelOrder(head);
        }
    }

    class Tree
    {
        static void Main(string[] args)
        {
            MLTree<string> tree = new MLTree<string>();
            char ch;
            do
            {
                Console.WriteLine("1.添加结点");
                Console.WriteLine("2.删除结点");
                Console.WriteLine("3.遍历树");
                Console.WriteLine("4.添加子结点");
                Console.WriteLine("5.退出");
                Console.WriteLine();
                ch = Convert.ToChar(Console.ReadLine());
                switch (ch)
                {
                    case '1':
                        Console.WriteLine("输入父结点：");
                        string str = Convert.ToString(Console.ReadLine());
                        MLNode<string> pt = new MLNode<string>(4);
                        pt.Data = str;
                        Console.WriteLine("输入子结点：");
                        str = Convert.ToString(Console.ReadLine());
                        MLNode<string> ct = new MLNode<string>(4);
                        ct.Data = str;
                        Console.WriteLine("输入插入子结点的位置：");
                        int i = Convert.ToInt32(Console.ReadLine());
                        bool ok = tree.Insert(ct, pt, i);
                        if (ok)
                        {
                            Console.WriteLine("插入{0}成功", ct.Data);
                            Console.WriteLine(tree.GetDepth(pt));
                        }
                        break;
                    case '2':
                        Console.WriteLine("输入要删除的结点：");
                        str = Convert.ToString(Console.ReadLine());
                        pt = new MLNode<string>(4);
                        pt.Data = str;
                        tree.Delete(pt, 0);
                        break;
                    case '3':
                        tree.Traverse(2);
                        break;
                }

            } while (ch != '5');
        }
    }
}