using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Graph
{
    
    abstract partial class Graph<T> where T : IElement<T>
    {
        public Node<T> SearchRoot { get; set; }
        public abstract List<Node<T>> DisplayChildNodes(Node<T> node);
        public abstract List<Node<T>> DisplaySortedChildNodes(Node<T> node);

        public static MatrixGraph<BasicElement> GenerateGraphByMatrixTxt(string path)
        {
            List<BasicElement> nodeDataList = null;
            double?[,] edgesMatrix = null;
            if (File.Exists(path))
            {
                nodeDataList = new List<BasicElement>();

                string[] lines = File.ReadAllLines(path);
                int colNum = lines[0].Split(',').Length;
                if (colNum == lines.Length - 1 || colNum == lines.Length)
                {
                    if (colNum == lines.Length - 1)
                        nodeDataList = new BasicElement().GetElementList(lines[0]);
                    else
                        nodeDataList = new BasicElement().GetElementList(lines.Length);

                    edgesMatrix = new double?[colNum, colNum];

                    for (int i = colNum==lines.Length?0:1; i < colNum; i++)
                    {
                        string[] cells = lines[i].Split(',');
                        for (int j = 0; j < cells.Length; j++)
                        {
                            edgesMatrix[j, i] = cells[j].Equals(string.Empty) ? null : (double?) double.Parse(cells[j]);
                        }
                    }
                }                
            }
            return new MatrixGraph<BasicElement> { EdgesMatrix = edgesMatrix, NodeDataList = nodeDataList };
        }
        public static ListGraph<BasicElement> GenerateBasicGraphByCmdInput()
        {
            List<Node<BasicElement>> graph = new List<Node<BasicElement>>();
            int id = 0;
            while (true)
            {
                Console.Write("Quiere agregar una nueva relación? ");
                if (Console.ReadLine().Equals("no"))
                    break;
                Console.Write("Primer nodo: ");
                string node1val = Console.ReadLine();
                Console.Write("Segundo nodo: ");
                string node2val = Console.ReadLine();
                Console.Write("Valor de la arista: ");
                string edgeStr = Console.ReadLine();
                double edge = 0;
                try
                {
                    edge = edgeStr.Equals(string.Empty) ? 0 : double.Parse(edgeStr);
                }
                catch (Exception) { }

                Node<BasicElement> node1 = graph.Find(n => n.Element.Value.Equals(node1val));
                Node<BasicElement> node2 = graph.Find(n => n.Element.Value.Equals(node2val));

                if (node1 == null)
                {
                    node1 = new Node<BasicElement> { Element = new BasicElement { Id = id++, Value = node1val } };
                    graph.Add(node1);
                }
                if (node2 == null)
                {
                    node2 = new Node<BasicElement> { Element = new BasicElement { Id = id++, Value = node2val } };
                    graph.Add(node2);
                }

                if (node1.ElementList.Find(e => e.Node == node2) == null)
                    node1.ElementList.Add(new NodeRelation<BasicElement> { Node = node2, Edge = edge });
                if (node2.ElementList.Find(e => e.Node == node1) == null)
                    node2.ElementList.Add(new NodeRelation<BasicElement> { Node = node1, Edge = edge });
                                
            }
            return new ListGraph<BasicElement> { NodeList = graph };
        }

        public abstract void ResetSearchLbls();
    }

    class ListGraph<T> : Graph<T> where T : IElement<T>
    {
        public List<Node<T>> NodeList { get; set; }

        public override List<Node<T>> DisplayChildNodes(Node<T> node)
        {
            if (node.ElementList.Count > 0)
                return (from n in node.ElementList select n.Node).ToList();
            else
                return null;
        }

        public override List<Node<T>> DisplaySortedChildNodes(Node<T> node)
        {
            
            throw new NotImplementedException();
        }

        public override void ResetSearchLbls()
        {
            for (int i = 0; i < NodeList.Count; i++)
            {
                NodeList[i].SeachLbl =  new SearchLbl<T>();
            }
        }
    }

    class MatrixGraph<T> : Graph<T> where T : IElement<T>
    {
        public List<T> NodeDataList { get; set; }
        public double?[,] EdgesMatrix { get; set; }
        
        public override List<Node<T>> DisplayChildNodes(Node<T> node)
        {
            List<Node<T>> list = new List<Node<T>>();
            for (int i = 0; i < EdgesMatrix.GetLength(0); i++)
            {
                if (EdgesMatrix[node.Element.Id, i] != null)
                    list.Add(new Node<T> { Element = NodeDataList[i] });
            }
            return list;
        }

        public override List<Node<T>> DisplaySortedChildNodes(Node<T> node)
        {
            throw new NotImplementedException();
        }

        public override void ResetSearchLbls()
        {
            throw new NotImplementedException();
        }
    }
}
