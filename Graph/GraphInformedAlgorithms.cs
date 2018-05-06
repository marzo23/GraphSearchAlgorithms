using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    abstract partial class Graph<T> where T : IElement<T>
    {
        public Node<T> UniformCostSearchById(Node<T> treeRoot, int id, List<Node<T>> toValidate, ref List<Node<T>> validated) //Dijkstra
        {
            Node<T> aux = null;
            if (toValidate == null)
                toValidate = new List<Node<T>>();
            if (validated == null)
            {
                validated = new List<Node<T>>();
                ResetSearchLbls();
                SearchRoot = treeRoot;
            }
            if (treeRoot != null)
            {
                if (treeRoot.Element.Id == id)
                    return treeRoot;
                validated.Add(treeRoot);

                for (int i = 0; i < treeRoot.ElementList.Count; i++)
                {
                    double value = (treeRoot.SeachLbl.Value == null ? 0 : (double)treeRoot.SeachLbl.Value) + treeRoot.ElementList[i].Edge;
                    if (treeRoot.ElementList[i].Node.SeachLbl.Value == null || treeRoot.ElementList[i].Node.SeachLbl.Value > value)
                    {
                        treeRoot.ElementList[i].Node.SeachLbl.PreviousNode = treeRoot;
                        treeRoot.ElementList[i].Node.SeachLbl.Value = value;
                    }

                    if (!validated.Contains(treeRoot.ElementList[i].Node))
                        toValidate.Add(treeRoot.ElementList[i].Node);
                }
                SortNodeList(ref toValidate, true);
            }
            if (toValidate.Count > 0)
            {
                Node<T> tmp = toValidate[0];
                toValidate.RemoveAt(0);
                aux = UniformCostSearchById(tmp, id, toValidate, ref validated);
            }
            return aux;
        }


        public Node<T> AStartSearchByElement(Node<T> treeRoot, T destination, List<Node<T>> toValidate, ref List<Node<T>> validated) 
        {
            Node<T> aux = null;
            if (toValidate == null)
                toValidate = new List<Node<T>>();
            if (validated == null)
            {
                validated = new List<Node<T>>();
                ResetSearchLbls();
                SearchRoot = treeRoot;
            }
            if (treeRoot != null)
            {
                if (treeRoot.Element.Id == destination.Id)
                    return treeRoot;
                validated.Add(treeRoot);

                for (int i = 0; i < treeRoot.ElementList.Count; i++)
                {
                    double value = (treeRoot.SeachLbl.Value == null ? 0 : (double)treeRoot.SeachLbl.Value) + treeRoot.ElementList[i].Edge;
                    if (treeRoot.ElementList[i].Node.SeachLbl.Value == null || treeRoot.ElementList[i].Node.SeachLbl.Value > value)
                    {
                        treeRoot.ElementList[i].Node.SeachLbl.PreviousNode = treeRoot;
                        treeRoot.ElementList[i].Node.SeachLbl.Value = value;
                    }

                    if (!validated.Contains(treeRoot.ElementList[i].Node))
                        toValidate.Add(treeRoot.ElementList[i].Node);
                }
                SortNodeList(ref toValidate, destination, true);
            }
            if (toValidate.Count > 0)
            {
                Node<T> tmp = toValidate[0];
                toValidate.RemoveAt(0);
                aux = AStartSearchByElement(tmp, destination, toValidate, ref validated);
            }
            return aux;
        }

        public Node<T> BestFirstSearchById(Node<T> treeRoot, int id, Stack<Node<T>> toValidate, ref List<Node<T>> validated)
        {
            Node<T> aux = null;
            if (toValidate == null)
                toValidate = new Stack<Node<T>>();
            if (validated == null)
            {
                validated = new List<Node<T>>();
                ResetSearchLbls();
                SearchRoot = treeRoot;
            }
            if (treeRoot != null)
            {
                if (treeRoot.Element.Id == id)
                    return treeRoot;
                validated.Add(treeRoot);

                List<Node<T>> tmpList = new List<Node<T>>();
                for (int i = 0; i < treeRoot.ElementList.Count; i++)
                {
                    treeRoot.ElementList[i].Node.SeachLbl.PreviousNode = treeRoot;
                    treeRoot.ElementList[i].Node.SeachLbl.Value = treeRoot.ElementList[i].Edge;

                    if (!validated.Contains(treeRoot.ElementList[i].Node))
                        tmpList.Add(treeRoot.ElementList[i].Node);
                }
                SortNodeList(ref tmpList, true);
                for (int i = tmpList.Count - 1; i >= 0; i--)
                {
                    toValidate.Push(tmpList[i]);
                }
            }
            if (toValidate.Count > 0)
            {
                aux = BestFirstSearchById(toValidate.Pop(), id, toValidate, ref validated);
            }
            return aux;
        }

        public Node<T> HillClimbingSearchByElement(Node<T> treeRoot, T destination, Stack<Node<T>> toValidate, ref List<Node<T>> validated)
        {
            Node<T> aux = null;
            if (toValidate == null)
                toValidate = new Stack<Node<T>>();
            if (validated == null)
            {
                validated = new List<Node<T>>();
                ResetSearchLbls();
                SearchRoot = treeRoot;
            }
            if (treeRoot != null)
            {
                if (treeRoot.Element.Id == destination.Id)
                    return treeRoot;
                validated.Add(treeRoot);

                List<Node<T>> tmpList = new List<Node<T>>();
                for (int i = 0; i < treeRoot.ElementList.Count; i++)
                {
                    treeRoot.ElementList[i].Node.SeachLbl.PreviousNode = treeRoot;
                    //treeRoot.ElementList[i].Node.SeachLbl.Value = treeRoot.ElementList[i].Edge;

                    if (!validated.Contains(treeRoot.ElementList[i].Node))
                        tmpList.Add(treeRoot.ElementList[i].Node);
                }
                SortNodeList(ref tmpList, destination, true);
                toValidate.Push(tmpList[0]); //DIFERENCIA CON EL DE MEMORIA (FOR)
            }
            if (toValidate.Count > 0)
                aux = HillClimbingSearchByElement(toValidate.Pop(), destination, toValidate, ref validated);
            return aux;
        }

        public Node<T> MemoryHillClimbingSearchByElement(Node<T> treeRoot, T destination, Stack<Node<T>> toValidate, ref List<Node<T>> validated)
        {
            Node<T> aux = null;
            if (toValidate == null)
                toValidate = new Stack<Node<T>>();
            if (validated == null)
            {
                validated = new List<Node<T>>();
                ResetSearchLbls();
                SearchRoot = treeRoot;
            }
            if (treeRoot != null)
            {
                if (treeRoot.Element.Id == destination.Id)
                    return treeRoot;
                validated.Add(treeRoot);

                List<Node<T>> tmpList = new List<Node<T>>();
                for (int i = 0; i < treeRoot.ElementList.Count; i++)
                {
                    treeRoot.ElementList[i].Node.SeachLbl.PreviousNode = treeRoot;
                    //treeRoot.ElementList[i].Node.SeachLbl.Value = treeRoot.ElementList[i].Edge;

                    if (!validated.Contains(treeRoot.ElementList[i].Node))
                        tmpList.Add(treeRoot.ElementList[i].Node);
                }
                SortNodeList(ref tmpList, destination, true);

                for (int i = tmpList.Count - 1; i >= 0; i--)
                {
                    toValidate.Push(tmpList[i]);
                }
            }
            if (toValidate.Count > 0)
                aux = HillClimbingSearchByElement(toValidate.Pop(), destination, toValidate, ref validated);
            return aux;
        }


        public List<Node<T>> SortNodeList(ref List<Node<T>> nodes, bool isAsc)
        {
            if (nodes.Count <= 0)
                return null;

            QuickSort(ref nodes,default(T), 0, nodes.Count - 1, isAsc?1:-1);

            return nodes;
        }

        public List<Node<T>> SortNodeList(ref List<Node<T>> nodes, T destination, bool isAsc)
        {
            if (nodes.Count <= 0)
                return null;

            QuickSort(ref nodes, destination, 0, nodes.Count - 1, isAsc ? 1 : -1);

            return nodes;
        }

        public void QuickSort(ref List<Node<T>> nodes, T destination, int min, int max, int orderBy) //1=asc -1=desc
        {
            int p = (max + min) / 2;
            double? pivot;
            if (destination != null && nodes[p].SeachLbl.Value != null)
                pivot = nodes[p].Element.HeuristicByDestination(destination) * nodes[p].SeachLbl.Value * orderBy; //ARREGLAR ESTO PARA HACER EL A*
            else
            {
                pivot = destination == null ? nodes[p].SeachLbl.Value : nodes[p].Element.HeuristicByDestination(destination);
                pivot = pivot == null ? 0 : pivot * orderBy;
            }            
            int i = min-1;
            int j = max+1;
            do
            {
                double? pivotMin = null;
                double? pivotMax = null;
                do
                {
                    i++;
                    if (destination != null && nodes[p].SeachLbl.Value != null)
                        pivotMin = nodes[i].SeachLbl.Value * nodes[i].Element.HeuristicByDestination(destination) * orderBy;
                    else
                    {
                        pivotMin = destination == null ? nodes[i].SeachLbl.Value : nodes[i].Element.HeuristicByDestination(destination);
                        pivotMin = pivotMin == null ? 0 : pivotMin * orderBy;
                    }                        
                }
                while (pivot > pivotMin);


                do
                {
                    j--;
                    if (destination != null && nodes[p].SeachLbl.Value != null)
                        pivotMax = nodes[j].SeachLbl.Value * nodes[j].Element.HeuristicByDestination(destination) * orderBy;
                    else
                    {
                        pivotMax = destination == null ? nodes[j].SeachLbl.Value : nodes[j].Element.HeuristicByDestination(destination);
                        pivotMax = pivotMax == null ? 0 : pivotMax * orderBy;
                    }                        
                }
                while (pivot < pivotMax);

                if (i <= j)
                {
                    Node<T> aux = nodes[i];
                    nodes[i] = nodes[j];
                    nodes[j] = aux;
                    i++;
                    j--;
                }

            } while (i <= j);
            if (min < j)
                QuickSort(ref nodes, destination, min, j, orderBy);
            if (max > i)
                QuickSort(ref nodes, destination, i, max, orderBy);
        }
    }

}
