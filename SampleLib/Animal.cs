using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SampleLib
{
    /// <summary>
    /// You may have some primary information about this class.
    /// </summary>
    /// <remarks>
    /// You may have some additional information about this class.
    /// </remarks>
    public class Animal
    {
        
        public Animal()
        {
            
        }

        // <C> TEST!
        /// <summary><c>MyMethod</c> is a method in the <c>MyClass</c> class.
        /// </summary>
        public static void MyMethod(int Int1)
        {
        }


        // <EXAMPLE> <CODE> test
        
        /// <summary>
        /// The GetZero method.
        /// </summary>
        /// <example> This sample shows how to call the GetZero method.
        /// <code>
        ///   class MyClass 
        ///   {
        ///      public static int Main() 
        ///      {
        ///         return GetZero();
        ///      }
        ///   }
        /// </code>
        /// </example>
        public static int GetZero()
        {
            return 0;
        }


        // exception tag
        /// <exception cref="System.Exception">Thrown when... .</exception>
        public static void ExceptionThrower(int method)
        {

        }

        /// <remarks>Here is an example of a bulleted list:
        /// <list type="bullet">
        /// <item>
        /// <description>Item 1.</description>
        /// </item>
        /// <item>
        /// <description>Item 2.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public static void ListExample(int one, int two)
        {

        }

        /// <summary>MyMethod is a method in the MyClass class.
        /// <para>Here's how you could make a second paragraph in a description. <see cref="System.Console.WriteLine"/> for information about output statements.</para>
        /// <seealso cref="MyClass.Main"/>
        /// </summary>
        public static void MyMethod2(int Int1)
        {
        }

        /// <param name="Int1">Used to indicate status.</param>
        public static void MyMethod3(int Int1)
        {
        }

        /// <remarks>MyMethod is a method in the MyClass class.  
        /// The <paramref name="Int1"/> parameter takes a number.
        /// </remarks>
        public static void MyMethod4(int Int1)
        {
        }

        /// <returns>Returns zero.</returns>
        public static int GetZero1()
        {
            return 0;
        }


        /// <summary>MyMethod is a method in the MyClass class.
        /// <para>Here's how you could make a second paragraph in a description. <see cref="System.Console.WriteLine"/> for information about output statements.</para>
        /// <seealso cref="MyClass.Main"/>
        /// </summary>
        public static void MyMethod5(int Int1)
        {
        }

        private string name;
        /// <value>Name accesses the value of the name data member</value>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
    }
}
