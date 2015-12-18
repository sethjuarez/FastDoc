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
            Node root = new Node() 
            { 
                Name = assembly.GetName().Name, 
                FullName = assembly.GetName().Name, 
                Children = new List<Node>() 
            };

            Dictionary<string, Node> nodes = new Dictionary<string, Node>();
            nodes[root.Name] = root;
            foreach (var type in assembly.ExportedTypes)
            {
                if (!nodes.ContainsKey(type.Namespace))
                {
                    nodes[type.Namespace] = new Node { Name = type.Namespace, FullName = type.Namespace, Children = new List<Node>() };
                    root.Push(nodes[type.Namespace]);
                }

                var item = new ItemNode
                {
                    Name = type.GetName(),
                    FullName = type.GetName(full: true),
                    Type = type,
                    ItemType = type.GetStructureType()
                };

                foreach (var m in MemberNode.GetMembers(type))
                    item.Push(m);

                nodes[type.Namespace].Push(item);
            }

            return root;
        }

        public override string ToString()
        {
            return FullName;
        }
    }

}