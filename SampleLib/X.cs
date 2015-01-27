using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleLib
{
    ///
    ///
    public unsafe class X    // "T:SampleLib.X"
    {
        /// <summary>
        /// 
        /// </summary>
        public X() { }   // "M:SampleLib.X.#ctor"
        // public X(){} // "M:SampleLib.X.#cctor", a class constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        public X(int i) { }  // "M:SampleLib.X.#ctor(System.Int32)"
        /// <summary>
        /// 
        /// </summary>
        ~X() { }  // "M:SampleLib.X.Finalize", destructor's representation in metadata

        /// <summary>
        /// 
        /// </summary>
        public string q;  // "F:SampleLib.X.q"
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public const double PI = 3.14;  // "F:SampleLib.X.PI"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public int f() { return 1; }  // "M:SampleLib.X.f"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public int bb(string s, ref int y, void* z) { return 1; }
        // "M:SampleLib.X.bb(System.String,System.Int32@,=System.Void*)"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="xx"></param>
        /// <returns></returns>
        public int gg(short[] array1, int[,] array) { return 0; }
        // "M:SampleLib.X.gg(System.Int16[], System.Int32[0:,0:])"
        /// <summary>
        /// 
        /// </summary>
        public static X operator +(X x, X xx) { return x; } // "M:SampleLib.X.op_Addition(N.X,N.X)"
        /// <summary>
        /// 
        /// </summary>
        public int prop { get { return 1; } set { } } // "P:SampleLib.X.prop"
        /// <summary>
        /// 
        /// </summary>
        ///
        public event D d; // "E:SampleLib.X.d"
        /// <summary>
        /// 
        /// </summary>
        public int this[string s] { get { return 1; } } // "P:SampleLib.X.Item(System.String)"
        /// <summary>
        /// 
        /// </summary>
        public class Nested { } // "T:SampleLib.X.Nested"
        /// <summary>
        /// 
        /// </summary>
        public delegate void D(int i); // "T:SampleLib.X.D"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static explicit operator int(X x) { return 1; }
        // "M:SampleLib.X.op_Explicit(N.X)~System.Int32"
    }
}
