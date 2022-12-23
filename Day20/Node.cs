using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Day20
{
    internal class Node
    {
        public Node(long value)
        {
            Value = value;
        }

        public long Value { get; private set; }
        public Node Prev { get; set; }
        public Node Next { get; set; }
        public override string ToString()
        {
            return string.Format("Value: {0}, Prev: {1}, Next: {2}", Value, Prev.Value, Next.Value);
        }
    }

    internal class LinkedNodes
    {
        public LinkedNodes()
        {
            Nodes = new List<Node>();
        }

        public Node Add(long value)
        {
            Node node = new Node(value);
            if (Nodes.Count == 0)
            {
                node.Next = node;
                node.Prev = node;
            }
            else
            {
                node.Prev = Nodes[Nodes.Count - 1];
                node.Next = Nodes[0];
                Nodes[0].Prev = node;
                Nodes[Nodes.Count - 1].Next = node;
            }
            Nodes.Add(node);
            NodeCount++;
            ModValue = NodeCount - 1;

            return node;
        }

        public void MixNode(int nodeIndex)
        {
            // optimized mix logic from Jeff & Scott
            long steps = Nodes[nodeIndex].Value;
            long delta = Math.Abs(steps % ModValue);

            if (delta > NodeCount / 2)
            {
                delta = NodeCount - delta - 1;
                steps = -steps;
            }

            for (int i = 0; i < delta; i++)
            {
                if (steps < 0)
                {
                    Swap(Nodes[nodeIndex].Prev, Nodes[nodeIndex]);
                }
                else
                {
                    Swap(Nodes[nodeIndex], Nodes[nodeIndex].Next);
                }
            }

        }

        public long GetGroveCoordinates()
        {
            Node node = Nodes.Find(n => n.Value == 0);
            long sum = 0;
            for (int i = 0; i <= 3000; i++)
            {
                if (i % 1000 == 0)
                {
                    sum += node.Value;
                }
                node = node.Next;
            }

            return sum;
        }

        private void Swap(Node left, Node right)
        {
            Node leftPrev = left.Prev;
            Node next = right.Next;

            next.Prev = left;
            left.Prev = right;
            left.Next = next;
            right.Prev = leftPrev;
            right.Next = left;
            leftPrev.Next = right;
        }

        public void DumpNodes(string msg)
        {
            Console.WriteLine(msg);
            Node rootNode = Nodes[0];
            Node node = rootNode;
            do
            {
                Console.Write(node.Value + " ");
                node = node.Next;
            }
            while (node != rootNode);
            Console.WriteLine();
        }

        public List<Node> Nodes { get; set; }
        public long ModValue {get; set;}
        public long NodeCount { get; set;}

    }
}
