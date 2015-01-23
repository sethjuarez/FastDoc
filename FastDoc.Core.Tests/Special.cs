using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///
///
namespace N  // "N:N"
{
    ///
    ///
    public unsafe class X    // "T:N.X"
    {
        /// <summary>
        /// 
        /// </summary>
        public X() { }   // "M:N.X.#ctor"
        // public X(){} // "M:N.X.#cctor", a class constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        public X(int i) { }  // "M:N.X.#ctor(System.Int32)"
        /// <summary>
        /// 
        /// </summary>
        ~X() { }  // "M:N.X.Finalize", destructor's representation in metadata

        /// <summary>
        /// 
        /// </summary>
        public string q;  // "F:N.X.q"
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public const double PI = 3.14;  // "F:N.X.PI"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public int f() { return 1; }  // "M:N.X.f"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public int bb(string s, ref int y, void* z) { return 1; }
        // "M:N.X.bb(System.String,System.Int32@,=System.Void*)"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="xx"></param>
        /// <returns></returns>
        public int gg(short[] array1, int[,] array) { return 0; }
        // "M:N.X.gg(System.Int16[], System.Int32[0:,0:])"
        /// <summary>
        /// 
        /// </summary>
        public static X operator +(X x, X xx) { return x; } // "M:N.X.op_Addition(N.X,N.X)"
        /// <summary>
        /// 
        /// </summary>
        public int prop { get { return 1; } set { } } // "P:N.X.prop"
        /// <summary>
        /// 
        /// </summary>
        ///
        public event D d; // "E:N.X.d"
        /// <summary>
        /// 
        /// </summary>
        public int this[string s] { get { return 1; } } // "P:N.X.Item(System.String)"
        /// <summary>
        /// 
        /// </summary>
        public class Nested { } // "T:N.X.Nested"
        /// <summary>
        /// 
        /// </summary>
        public delegate void D(int i); // "T:N.X.D"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static explicit operator int(X x) { return 1; }
        // "M:N.X.op_Explicit(N.X)~System.Int32"
    }
}

