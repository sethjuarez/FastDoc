using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace FastDoc.Core
{
    internal static class PrettyPrintExtensions
    {
        /// <summary>
        /// Return the method signature as a string.
        /// </summary>
        /// <param name="method">The Method</param>
        /// <param name="callable">Return as an callable string(public void a(string b) would return a(b))</param>
        /// <returns>Method signature</returns>
        public static string Signature(this ConstructorInfo method)
        {
            var firstParam = true;
            var sigBuilder = new StringBuilder();

            sigBuilder.Append(method.Name);

            // Add method generics
            if (method.IsGenericMethod)
            {
                sigBuilder.Append("<");
                foreach (var g in method.GetGenericArguments())
                {
                    if (firstParam)
                        firstParam = false;
                    else
                        sigBuilder.Append(", ");
                    sigBuilder.Append(TypeName(g));
                }
                sigBuilder.Append(">");
            }
            sigBuilder.Append("(");
            firstParam = true;
            var secondParam = false;
            foreach (var param in method.GetParameters())
            {
                if (firstParam)
                {
                    firstParam = false;
                    if (method.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false))
                    {
                        sigBuilder.Append("this ");
                    }
                }
                else if (secondParam)
                    secondParam = false;
                else
                    sigBuilder.Append(", ");
                if (param.ParameterType.IsByRef)
                    sigBuilder.Append("ref ");
                else if (param.IsOut)
                    sigBuilder.Append("out ");

                if (param.IsOptional)
                {
                    sigBuilder.Append("[");
                    sigBuilder.Append(TypeName(param.ParameterType));
                    sigBuilder.Append(' ');
                    sigBuilder.Append(param.Name);
                    sigBuilder.Append("]");
                }
                else
                {
                    sigBuilder.Append(TypeName(param.ParameterType));
                    sigBuilder.Append(' ');
                    sigBuilder.Append(param.Name);
                }
            }
            sigBuilder.Append(")");
            return sigBuilder.ToString();
        }

        public static string Signature(this EventInfo method)
        {
            var sigBuilder = new StringBuilder();
            sigBuilder.Append(TypeName(method.EventHandlerType));
            sigBuilder.Append(" ");
            sigBuilder.Append(method.Name);            
            return sigBuilder.ToString();
        }

        /// <summary>
        /// Return the method signature as a string.
        /// </summary>
        /// <param name="method">The Method</param>
        /// <param name="callable">Return as an callable string(public void a(string b) would return a(b))</param>
        /// <returns>Method signature</returns>
        public static string Signature(this MethodInfo method)
        {
            var firstParam = true;
            var sigBuilder = new StringBuilder();

            sigBuilder.Append(TypeName(method.ReturnType));
            sigBuilder.Append(' ');

            sigBuilder.Append(method.Name);

            // Add method generics
            if (method.IsGenericMethod)
            {
                sigBuilder.Append("<");
                foreach (var g in method.GetGenericArguments())
                {
                    if (firstParam)
                        firstParam = false;
                    else
                        sigBuilder.Append(", ");
                    sigBuilder.Append(TypeName(g));
                }
                sigBuilder.Append(">");
            }
            sigBuilder.Append("(");
            firstParam = true;
            var secondParam = false;
            foreach (var param in method.GetParameters())
            {
                if (firstParam)
                {
                    firstParam = false;
                    if (method.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false))
                    {
                        sigBuilder.Append("this ");
                    }
                }
                else if (secondParam)
                    secondParam = false;
                else
                    sigBuilder.Append(", ");
                if (param.ParameterType.IsByRef)
                    sigBuilder.Append("ref ");
                else if (param.IsOut)
                    sigBuilder.Append("out ");

                if (param.IsOptional)
                {
                    sigBuilder.Append("[");
                    sigBuilder.Append(TypeName(param.ParameterType));
                    sigBuilder.Append(' ');
                    sigBuilder.Append(param.Name);
                    sigBuilder.Append("]");
                }
                else
                {
                    sigBuilder.Append(TypeName(param.ParameterType));
                    sigBuilder.Append(' ');
                    sigBuilder.Append(param.Name);
                }
            }
            sigBuilder.Append(")");
            return sigBuilder.ToString();
        }

        public static string TypeName(Type type)
        {
            var nullableType = Nullable.GetUnderlyingType(type);
            if (nullableType != null)
                return nullableType.Name + "?";

            if (!type.IsGenericType)
            {
                return type.Name
                    .Replace("String ", "string ")
                    .Replace("Int32 ", "int ")
                    .Replace("Decimal ", "decimal ")
                    .Replace("Object ", "object ")
                    .Replace("Void ", "void ")
                    .Replace("Double ", "double ");
            }

            var sb = new StringBuilder(type.Name.Substring(0, type.Name.IndexOf('`')));

            sb.Append('<');
            var first = true;
            foreach (var t in type.GetGenericArguments())
            {
                if (!first)
                    sb.Append(',');
                sb.Append(TypeName(t));
                first = false;
            }
            sb.Append('>');
            return sb.ToString();
        }

        public static string Signature(this PropertyInfo pr)
        {
            if (pr.GetMethod != null)
            {
                return pr.GetMethod.Signature()
                                   .Replace("get_", "")
                                   .Replace("()", "")
                                   .Replace("(", "[")
                                   .Replace(")", "]");
            }
            else if (pr.SetMethod != null)
            {
                var method = pr.SetMethod.Signature()
                                   .Replace("void set_", "")
                                   .Replace("(", "[");
                var i = method.LastIndexOf(", ");
                method = method.Insert(i, "]");
                method = method.Replace("], ", "] = ");
                method = method.Replace(")", "");
                return method;
            }
            else
                return pr.ToString();
        }

        public static string Signature(this FieldInfo fi)
        {
            return String.Format("{0} {1}", TypeName(fi.FieldType), fi.Name);
        }
    }

}