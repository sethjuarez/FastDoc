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
        static void Main(string[] args)
        {
            var root = Load("SampleLib.dll");
        }

        static Node Load(string asmbly)
        {
            Node root = new Node() { Name = "Empty" };
            try
            {
                var assembly = Assembly.LoadFrom(asmbly);
                root = Node.Generate(assembly);

                //Print(root);
            }
            catch (Exception error)
            {
                Write(ConsoleColor.Red, error.Message + "\n");
                Write(ConsoleColor.Red, error.StackTrace + "\n");
            }

            return root;
        }


        static void Write(ConsoleColor color, string item)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(item);
            Console.ForegroundColor = oldColor;
        }
    }
}
