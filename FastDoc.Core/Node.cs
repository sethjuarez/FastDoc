// file:	Node.cs
//
// summary:	Implements the node class
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace FastDoc.Core
{

    /// <summary>Values that represent NodeType.</summary>
    public enum NodeType
    {
        /// <summary>Event queue for all listeners interested in Namespace events.</summary>
        Namespace,
        /// <summary>Event queue for all listeners interested in Enum events.</summary>
        Enum,
        /// <summary>Event queue for all listeners interested in Class events.</summary>
        Class,
        /// <summary>Event queue for all listeners interested in Constructor events.</summary>
        Constructor,
        /// <summary>Event queue for all listeners interested in Property events.</summary>
        Property,
        /// <summary>Event queue for all listeners interested in Method events.</summary>
        Method,
        /// <summary>Event queue for all listeners interested in Interface events.</summary>
        Interface,
        /// <summary>Event queue for all listeners interested in Field events.</summary>
        Field,
        /// <summary>Event queue for all listeners interested in Parameter events.</summary>
        Parameter,
        /// <summary>Event queue for all listeners interested in Unknown events.</summary>
        Unknown,
        /// <summary>Event queue for all listeners interested in Event events.</summary>
        Event
    }

    /// <summary>A node.</summary>
    public class Node
    {
        /// <summary>Initializes a new instance of the Node class.</summary>
        public Node()
        {
            Name = "";
            FullName = "";
            Documentation = "";
            NodeType = Core.NodeType.Unknown;
            _children = new Node[] { };
        }
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>Gets or sets the name of the full.</summary>
        /// <value>The name of the full.</value>
        public string FullName { get; set; }
        /// <summary>Gets or sets the documentation.</summary>
        /// <value>The documentation.</value>
        public string Documentation { get; set; }
        /// <summary>Gets or sets the type of the node.</summary>
        /// <value>The type of the node.</value>
        public NodeType NodeType { get; set; }
        /// <summary>Gets or sets the type.</summary>
        /// <value>The type.</value>
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

        /// <summary>The type.</summary>
        private Type _type;
        /// <summary>The children.</summary>
        private Node[] _children;
        /// <summary>The XML.</summary>
        private static string _xml;
        /// <summary>Gets the children.</summary>
        /// <value>The children.</value>
        public Node[] Children
        {
            get
            {
                return _children;
            }
        }
        /// <summary>Pushes an object onto this stack.</summary>
        /// <param name="name">The name to push.</param>
        /// <returns>A Node.</returns>
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
        /// <summary>Pushes an object onto this stack.</summary>
        /// <param name="node">The node to push.</param>
        public void Push(Node node)
        {
            Array.Resize(ref _children, Children.Length + 1);
            _children[_children.Length - 1] = node;
        }

        /// <summary>Sets node type.</summary>
        private void SetNodeType()
        {
            if (Type == null) NodeType = NodeType.Unknown;
            else if (Type.IsClass) NodeType = NodeType.Class;
            else if (Type.IsInterface) NodeType = NodeType.Interface;
            else if (Type.IsEnum) NodeType = NodeType.Enum;
        }
        /// <summary>Generates the members.</summary>
        /// <exception cref="InvalidProgramException">Thrown when an Invalid Program error condition occurs.</exception>
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
        /// <summary>Creates member node.</summary>
        /// <param name="member">The member.</param>
        /// <returns>The new member node.</returns>
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
        /// <summary>Generates a member parameters.</summary>
        /// <param name="member">The member.</param>
        /// <param name="parent">The parent.</param>
        /// <returns>The member parameters.</returns>
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
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, NodeType);
        }
        /// <summary>Generates.</summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="xml">The XML.</param>
        /// <returns>A Node.</returns>
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
