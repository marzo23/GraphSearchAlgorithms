using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class SearchLbl<T>
    {
        public double? Value { get; set; }
        public Node<T> PreviousNode { get; set; }

        public SearchLbl()
        {
            Value = null;
            PreviousNode = null;
        }
    }

    public class Node<T>
    {
        public T Element { get; set; }
        public List<NodeRelation<T>> ElementList { get; set; }
        public SearchLbl<T> SeachLbl { get; set; }
        public Node()
        {
            ElementList = new List<NodeRelation<T>>();
            Element = default(T);
            SeachLbl = new SearchLbl<T>();
        }
    }

    public class NodeRelation<T>
    {
        public Node<T> Node { get; set; }
        public double Edge { get; set; }

        public NodeRelation()
        {
            Node = new Node<T>();
            Edge = 0;
        }
    }
    
}
