using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace FastDoc.Core
{

    public enum NodeType
    {
        Namespace,
        Enum,
        Class,
        Constructor,
        Property,
        Method,
        Interface,
        Field,
        Parameter,
        Unknown,
        Event
    }

    public class Node
    {
        /// <summary>
        /// Initializes a new instance of the Node class.
        /// </summary>
        public Node()
        {
            Name = "";
            FullName = "";
            Documentation = "";
            NodeType = Core.NodeType.Unknown;
            _children = new Node[] { };
        }

        public string Name { get; set; }
        public string FullName { get; set; }
        public string Documentation { get; set; }
        public NodeType NodeType { get; set; }
        public Type Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                SetNodeType();
            }
        }

        private Type _type;
        private Node[] _children;
        private static string _xml;
        public Node[] Children
        {
            get
            {
                return _children;
            }
        }

        public Node Push(string name)
        {
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

        public void Push(Node node)
        {
            Array.Resize(ref _children, Children.Length + 1);
            _children[_children.Length - 1] = node;
        }

        private void SetNodeType()
        {
            if (Type == null) NodeType = NodeType.Unknown;
            else if (Type.IsClass) NodeType = NodeType.Class;
            else if (Type.IsInterface) NodeType = NodeType.Interface;
            else if (Type.IsEnum) NodeType = NodeType.Enum;
        }

        public void GenerateMembers()
        {
            if (NodeType == NodeType.Class || NodeType == NodeType.Interface)
            {
                foreach (var member in Type.GetMembers())
                {
                    if (member.Name.Contains("get_") || member.Name.Contains("set_") || member.DeclaringType != Type) continue;

                    Node n = CreateMemberNode(member);

                    if (n == null)
                        throw new InvalidProgramException("Uh oh, not sure what we are working with here....");

                    Push(n);
                    GenerateMemberParameters(member, n);

                }
            }
            else if (NodeType == NodeType.Enum)
            {
                foreach (var item in Type.GetEnumNames())
                    Push(new Node { Name = item, NodeType = Core.NodeType.Property });
            }
        }


        private static Node CreateMemberNode(MemberInfo member)
        {
            Node n = null;
            if (member is MethodInfo)
                n = new Node { Name = (member as MethodInfo).Signature(), NodeType = NodeType.Method };
            else if (member is PropertyInfo)
                n = new Node { Name = (member as PropertyInfo).Signature(), NodeType = NodeType.Property };
            else if (member is ConstructorInfo)
                n = new Node { Name = (member as ConstructorInfo).Signature(), NodeType = NodeType.Constructor };
            else if (member is FieldInfo)
                n = new Node { Name = (member as FieldInfo).Signature(), NodeType = NodeType.Field };
            else if (member is EventInfo)
                n = new Node { Name = (member as EventInfo).Signature(), NodeType = NodeType.Event };

            n.FullName = member.ToString();
            n.Documentation = member.GetXmlDocumentation(_xml);
            return n;
        }

        private static Node GenerateMemberParameters(MemberInfo member, Node parent)
        {
            if (member is MethodBase)
            {
                MethodBase method = (MethodBase)member;
                foreach (ParameterInfo parameter in method.GetParameters())
                {
                    var p = new Node()
                    {
                        Name = String.Format("{0} {1}", PrettyPrintExtensions.TypeName(parameter.ParameterType), parameter.Name),
                        Documentation = parameter.GetXmlDocumentation(_xml),
                        NodeType = NodeType.Parameter,
                        FullName = parameter.Name
                    };

                    parent.Push(p);
                }
            }

            return parent;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, NodeType);
        }

        public static Node Generate(Assembly assembly, string xml)
        {
            _xml = xml;
            bool full = !string.IsNullOrWhiteSpace(_xml);

            Node root = null;
            foreach (var t in assembly.GetTypes())
            {
                string[] name = t.ToString().Split('.');

                if (root == null)
                    root = new Node { Name = name[0], FullName = name[0], NodeType = NodeType.Namespace };

                if (!name[name.Length - 1].Contains("__"))
                {
                    name[name.Length - 1] = PrettyPrintExtensions.TypeName(t);
                    var n = root;
                    for (int i = 1; i < name.Length; i++)
                    {
                        n = n.Push(name[i]);
                        if (n.Name == t.Name || n.Name.Contains("<"))
                        {
                            n.Type = t;
                            n.FullName = String.Join(".", name, 0, name.Length);
                            if (full)
                                n.GenerateMembers();
                        }
                        else
                        {
                            n.NodeType = NodeType.Namespace;
                            n.FullName = String.Join(".", name, 0, i + 1);
                        }
                    }
                }
            }

            return root;
        }
    }
}
