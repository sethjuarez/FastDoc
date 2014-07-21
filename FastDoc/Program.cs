using System;
using System.Linq;
using FastDoc.Core;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FastDoc
{
    class Program
    {

        static readonly Regex trimmer = new Regex(@"\n\s+");
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Write(ConsoleColor.DarkGray, "Usage:\n\t");
                Write(ConsoleColor.Green, "EasyDoc.exe ");
                Write(ConsoleColor.White, "Assembly.dll ");
                Write(ConsoleColor.Gray, "[Assembly.xml]\n");
                Write(ConsoleColor.Yellow, "\txml optional (if ommitted will simply replace \".dll\" with \".xml\")\n");
            }
            else if (args.Length == 1)
            {
                var assembly = Assembly.LoadFrom(args[0]);
                var root = Node.Generate(assembly, args[0].Replace(".dll", ".xml"));
                Print(root);
            }
            else if (args.Length == 2)
            {
                var assembly = Assembly.LoadFrom(args[0]);
                var root = Node.Generate(assembly, args[1]);
                Print(root);
            }
        }

        static void Print(Node n, string pre = "")
        {
            Write(ConsoleColor.DarkGray, pre + " |-");
            var output = String.Format("{0} ({1})\n", n.Name, n.NodeType);
            Write(GetColor(n.NodeType), output);
            Write(ConsoleColor.DarkGray, pre + "  >");
            if (string.IsNullOrEmpty(n.Documentation))
                Write(ConsoleColor.Red, "  Missing\n");
            else
                Write(ConsoleColor.White, String.Format("  {0}\n", trimmer.Replace(n.Documentation, " ")));
            foreach (var node in n.Children.OrderBy(t => t.NodeType))
                Print(node, String.Format("{0} |\t", pre));
        }

        static void Write(ConsoleColor color, string item)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(item);
            Console.ForegroundColor = oldColor;
        }

        static ConsoleColor GetColor(NodeType type)
        {
            switch (type)
            {
                case NodeType.Namespace:
                    return ConsoleColor.White;
                case NodeType.Enum:
                    return ConsoleColor.Cyan;
                case NodeType.Class:
                    return ConsoleColor.Gray;
                case NodeType.Constructor:
                    return ConsoleColor.Green;
                case NodeType.Property:
                    return ConsoleColor.Magenta;
                case NodeType.Method:
                    return ConsoleColor.Green;
                case NodeType.Interface:
                    return ConsoleColor.Yellow;
                case NodeType.Field:
                    return ConsoleColor.DarkBlue;
                case NodeType.Parameter:
                    return ConsoleColor.DarkCyan;
                case NodeType.Unknown:
                    return ConsoleColor.DarkGray;
            }

            return ConsoleColor.White;
        }
    }
}
