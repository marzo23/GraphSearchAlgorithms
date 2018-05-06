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

        public ListGraph()
        {
            NodeList = new List<Node<T>>();
        }

        public override List<Node<T>> DisplayChildNodes(Node<T> node)
        {
            if (node.ElementList.Count > 0)
                return (from n in node.ElementList select n.Node).ToList();
            else
                return null;
        }

        public MatrixGraph<T> GenerateMatrixGraphFromListGraph()
        {
            MatrixGraph<T> matrixGraph = null;
            if (NodeList != null)
            {
                matrixGraph = new MatrixGraph<T>();
                matrixGraph.EdgesMatrix = new double?[NodeList.Count, NodeList.Count];
                for (int i = 0; i < NodeList.Count; i++)
                {
                    matrixGraph.NodeDataList.Add(NodeList[i].Element);
                    for (int j = 0; j < NodeList.Count; j++)
                    {
                        matrixGraph.EdgesMatrix[i, j] = null;
                    }
                    for (int j = 0; j < NodeList[i].ElementList.Count; j++)
                    {
                        matrixGraph.EdgesMatrix[NodeList[i].ElementList[j].Node.Element.Id, i] = NodeList[i].ElementList[j].Edge;
                    }
                }
            }
            return matrixGraph;
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

        public void RUNALL(ListGraph<T> grafo1, Node<T> inicio, int destinationId)
        {
            List<Node<T>> deptProcessed = null;
            List<Node<T>> ampProcessed = null;
            Node<T> resultdept = grafo1.DepthSearchById(inicio, destinationId, null, ref deptProcessed);
            Node<T> resultamp = grafo1.AmplitudeSearchById(inicio, destinationId, null, ref ampProcessed);
            if (deptProcessed != null)
            {
                Console.WriteLine("\nDEPTH: ");
                foreach (Node<T> item in deptProcessed)
                {
                    Console.Write(item.Element.Id + ", ");
                }

                //Console.WriteLine("Recorrido final: ");
                //Node<T> aux = resultdept;
                //while (aux.SeachLbl.PreviousNode != null)
                //{
                //    Console.WriteLine(aux.Element.Id + ", ");
                //    aux = aux.SeachLbl.PreviousNode;
                //}
            }
            if (ampProcessed != null)
            {
                Console.WriteLine("\nAmplitude: ");
                foreach (Node<T> item in ampProcessed)
                {
                    Console.Write(item.Element.Id + ", ");
                }

                //Console.WriteLine("Recorrido final: ");
                //Node<T> aux = resultamp;
                //while (aux.SeachLbl.PreviousNode != null)
                //{
                //    Console.WriteLine(aux.Element.Id + ", ");
                //    aux = aux.SeachLbl.PreviousNode;
                //}
            }

            List<Node<T>> limitedDepthProcessed = null;
            List<Node<T>> IterativeDepthProcessed = null;
            Node<T> resultLimitedDepth = grafo1.LimitedDepthSearchById(5, inicio, destinationId, null, ref limitedDepthProcessed);
            Node<T> resultIterativeDepth = grafo1.IterativeDepthSearchById(3, 1, inicio, destinationId, null, ref IterativeDepthProcessed);
            if (limitedDepthProcessed != null && resultLimitedDepth != null)
            {
                Console.WriteLine("\nLimited: ");
                foreach (Node<T> item in limitedDepthProcessed)
                {
                    Console.Write(item.Element.Id + ", ");
                }

                //Console.WriteLine("Recorrido final: ");
                //Node<T> aux = resultLimitedDepth;
                //while (aux.SeachLbl.PreviousNode != null)
                //{
                //    Console.WriteLine(aux.Element.Id + ", ");
                //    aux = aux.SeachLbl.PreviousNode;
                //}
            }
            else
                Console.WriteLine("\nNo fue encontrado por búsqueda limitada por profundidad 5");
            if (IterativeDepthProcessed != null && resultIterativeDepth != null)
            {
                Console.WriteLine("\nIterative: ");
                foreach (Node<T> item in IterativeDepthProcessed)
                {
                    Console.Write(item.Element.Id + ", ");
                }

                //Console.WriteLine("Recorrido final: ");
                //Node<T> aux = resultIterativeDepth;
                //while (aux.SeachLbl.PreviousNode != null)
                //{
                //    Console.WriteLine(aux.Element.Id + ", ");
                //    aux = aux.SeachLbl.PreviousNode;
                //}
            }

            List<Node<T>> memoryClimbingProcessed = null;
            List<Node<T>> bestFirstProcessed = null;
            Node<T> destination = grafo1.NodeList.Find(f => f.Element.Id == destinationId);
            Node<T> resultClimbingM = grafo1.MemoryHillClimbingSearchByElement(inicio, destination.Element, null, ref memoryClimbingProcessed);
            Node<T> resultBestFirst = grafo1.BestFirstSearchById(inicio, destinationId, null, ref bestFirstProcessed);
            if (memoryClimbingProcessed != null && resultClimbingM != null)
            {
                Console.WriteLine("\nSubida de la colina con memoria: ");
                foreach (Node<T> item in memoryClimbingProcessed)
                {
                    Console.Write(item.Element.Id + ", ");
                }

                //Console.WriteLine("Recorrido final: ");
                //Node<T> aux = resultClimbingM;
                //while (aux.SeachLbl.PreviousNode != null)
                //{
                //    Console.WriteLine(aux.Element.Id + ", ");
                //    aux = aux.SeachLbl.PreviousNode;
                //}
            }
            else
                Console.WriteLine("\nNo fue encontrado por búsqueda subida de la colina con memoria");
            if (bestFirstProcessed != null && resultBestFirst != null)
            {
                Console.WriteLine("\nprimero el mejor: ");
                foreach (Node<T> item in bestFirstProcessed)
                {
                    Console.Write(item.Element.Id + ", ");
                }

                //Console.WriteLine("Recorrido final: ");
                //Node<T> aux = resultBestFirst;
                //while (aux.SeachLbl.PreviousNode != null)
                //{
                //    Console.WriteLine(aux.Element.Id + ", ");
                //    aux = aux.SeachLbl.PreviousNode;
                //}
            }
            else
                Console.WriteLine("\nNo fue encontrado por búsqueda mejor el primero");

            List<Node<T>> climbingProcessed = null;
            List<Node<T>> dijkstraProcessed = null;
            Node<T> resultClimbing = grafo1.HillClimbingSearchByElement(inicio, destination.Element, null, ref climbingProcessed);
            Node<T> resultDijkstra = grafo1.UniformCostSearchById(inicio, destinationId, null, ref dijkstraProcessed);
            if (climbingProcessed != null && resultClimbing != null)
            {
                Console.WriteLine("\nClimbing sin memoria: ");
                foreach (Node<T> item in climbingProcessed)
                {
                    Console.Write(item.Element.Id + ", ");
                }

                //Console.WriteLine("Recorrido final: ");
                //Node<T> aux = resultClimbing;
                //while (aux.SeachLbl.PreviousNode != null)
                //{
                //    Console.WriteLine(aux.Element.Id + ", ");
                //    aux = aux.SeachLbl.PreviousNode;
                //}
            }
            else
                Console.WriteLine("\nNo fue encontrado por búsqueda subida de la colina SIN memoria");
            if (dijkstraProcessed != null && resultDijkstra != null)
            {
                Console.WriteLine("\nDIJKSTRA: ");
                foreach (Node<T> item in dijkstraProcessed)
                {
                    Console.Write(item.Element.Id + ", ");
                }

                //Console.WriteLine("Recorrido final: ");
                //Node<T> aux = resultDijkstra;
                //while (aux.SeachLbl.PreviousNode != null)
                //{
                //    Console.WriteLine(aux.Element.Id + ", ");
                //    aux = aux.SeachLbl.PreviousNode;
                //}
            }
            else
                Console.WriteLine("\nNo fue encontrado por DIJKSTRA");

            List<Node<T>> AEstrellaProcessed = null;
            Node<T> resultAEstrella = grafo1.AStartSearchByElement(inicio, destination.Element, null, ref AEstrellaProcessed);
            if (AEstrellaProcessed != null && resultAEstrella != null)
            {
                Console.WriteLine("\nA* busqueda **********************: ");
                foreach (Node<T> item in AEstrellaProcessed)
                {
                    Console.Write(item.Element.Id + ", ");
                }

                //Console.WriteLine("Recorrido final: ");
                //Node<T> aux = resultAEstrella;
                //while (aux.SeachLbl.PreviousNode != null)
                //{
                //    Console.WriteLine(aux.Element.Id + ", ");
                //    aux = aux.SeachLbl.PreviousNode;
                //}
            }
            else
                Console.WriteLine("\nNo fue encontrado por búsqueda A*");

        }
    }

    class MatrixGraph<T> : Graph<T> where T : IElement<T>
    {
        public List<T> NodeDataList { get; set; }
        public double?[,] EdgesMatrix { get; set; }
        
        public MatrixGraph()
        {
            NodeDataList = new List<T>();
            EdgesMatrix = null;
        }

        public ListGraph<T> GenerateListGraphFromMatrixGraph()
        {
            ListGraph<T> listGraph = null;

            if (NodeDataList!=null && EdgesMatrix != null)
            {
                listGraph = new ListGraph<T>();
                for (int i = 0; i < NodeDataList.Count; i++)
                    listGraph.NodeList.Add(new Node<T> { Element = NodeDataList[i] });

                for (int i = 0; i < NodeDataList.Count; i++)
                {
                    for (int j = 0; j < EdgesMatrix.GetLength(0); j++)
                    {
                        if (EdgesMatrix[j, i] != null && EdgesMatrix[j, i] != 0)//CORREGIR ARCHIVO DE DISTANCIAS
                        {
                            listGraph.NodeList[i].ElementList.Add(new NodeRelation<T> { Node = listGraph.NodeList[j], Edge = (double)EdgesMatrix[j, i] });
                        }
                    }
                }
            }
            return listGraph;
        }

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
