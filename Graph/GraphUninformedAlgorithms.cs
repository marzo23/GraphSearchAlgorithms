using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    abstract partial class Graph<T> where T : IElement<T>
    {
        public Node<T> AmplitudeSearchById(Node<T> treeRoot, int id, Queue<Node<T>> toValidate, ref List<Node<T>> validated)
        {
            Node<T> aux = null;
            if (toValidate == null)
                toValidate = new Queue<Node<T>>();
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

                List<Node<T>> childNodeList = DisplayChildNodes(treeRoot);
                for (int i = 0; i < childNodeList.Count; i++)
                {
                    childNodeList[i].SeachLbl.PreviousNode = treeRoot;

                    if (!validated.Contains(childNodeList[i]))
                        toValidate.Enqueue(childNodeList[i]);
                }
            }
            if (toValidate.Count > 0)
                aux = AmplitudeSearchById(toValidate.Dequeue(), id, toValidate, ref validated);
            return aux;
        }

        public Node<T> DepthSearchById(Node<T> treeRoot, int id, Stack<Node<T>> toValidate, ref List<Node<T>> validated)
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

                List<Node<T>> childNodeList = DisplayChildNodes(treeRoot);
                for (int i = 0; i < childNodeList.Count; i++)
                {
                    childNodeList[i].SeachLbl.PreviousNode = treeRoot;

                    if (!validated.Contains(childNodeList[i]))
                        toValidate.Push(childNodeList[i]);
                }
            }
            if (toValidate.Count > 0)
                aux = DepthSearchById(toValidate.Pop(), id, toValidate, ref validated);
            return aux;
        }

        public Node<T> LimitedDepthSearchById(int limit, Node<T> treeRoot, int id, Stack<Node<T>> toValidate, ref List<Node<T>> validated)
        {
            if (limit <= 0)
                return null;
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

                List<Node<T>> childNodeList = DisplayChildNodes(treeRoot);
                for (int i = 0; i < childNodeList.Count; i++)
                {
                    childNodeList[i].SeachLbl.Value = limit - 1;
                    childNodeList[i].SeachLbl.PreviousNode = treeRoot;

                    if (!validated.Contains(childNodeList[i]))
                        toValidate.Push(childNodeList[i]);
                }

            }
            if (toValidate.Count > 0)
            {
                Node<T> ToValidateNode = toValidate.Pop();
                aux = LimitedDepthSearchById((int)ToValidateNode.SeachLbl.Value, ToValidateNode, id, toValidate, ref validated);
            }
            if (toValidate.Count <= 0 && limit > 0)
                return new Node<T>();
            return aux;
        }

        public Node<T> IterativeDepthSearchById(int firstLimit, int increment, Node<T> treeRoot, int id, Stack<Node<T>> toValidate, ref List<Node<T>> validated)
        {
            int counter = 0;
            Node<T> aux = null;
            bool flag = true;
            while (flag)
            {
                if (counter == 0)
                    counter = firstLimit;
                else
                    counter += increment;
                validated = null;
                aux = LimitedDepthSearchById(counter, treeRoot, id, toValidate, ref validated);
                if (aux != null)
                    break;
            }
            if (aux.Element == null)
                return null;
            return aux;

        }
    }
}
