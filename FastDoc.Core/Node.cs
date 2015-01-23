using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace FastDoc.Core
{
    [Serializable]
    public class Node
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public List<Node> Children { get; set; }
        public Node Push(Node n)
        {
            if (Children == null)
                Children = new List<Node>();

            if (!Children.Contains(n))
                Children.Add(n);

            return n;
        }

        public Node Push(string name)
        {
            if (Children == null)
                Children = new List<Node>();

            var q = Children.Where(c => c.Name == name);
            if (q.Count() == 1)
                return q.First();
            else
            {
                var node = new Node { Name = name };
                Push(node);
                return node;
            }
        }

        public static Node Generate(Assembly assembly)
        {
            Node root = null;
            foreach (var t in assembly.ExportedTypes)
            {
                string[] name = t.Namespace.Split('.');
                if (root == null)
                    root = new Node { Name = name[0], FullName = name[0] };

                var n = root;
                for (int i = 1; i < name.Length; i++)
                {
                    n = n.Push(name[i]);
                    if (n.Name != t.Name)
                        n.FullName = String.Join(".", name, 0, i + 1);
                }
            }


            return Generate(assembly, root);
        }

        public static Node Generate(Assembly assembly, Node root)
        {
            if (root.Children != null)
                foreach (var node in root.Children)
                    Generate(assembly, node);

            foreach (var item in ItemNode.GetItems(assembly, root.FullName))
                root.Push(item);

            return root;
        }
    }

}