using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Model
{
    public class Node<T>
    {
        public T data;
        private List<Node<T>> childrenNodes;

        public int GetLengthChildren()
        {
            return childrenNodes.Count;
        }
        Node()
        {
            childrenNodes=new List<Node<T>>();
        }
        public void GetChildrenNode(int index)
        { }
        public void RemoveChildrenNode(int index)
        { }
    }
}
