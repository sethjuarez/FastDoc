using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace FastDoc.Core
{
    internal static class PrettyPrintExtensions
    {
        public static string GetName(this Type type, bool full = false)
        {
            var nullableType = Nullable.GetUnderlyingType(type);
            if (nullableType != null)
                return nullableType.Name + "?";

            if (!type.IsGenericType)
            {
                //if (type == typeof(string))
                //    return "string";
                //else if (type == typeof(int))
                //    return "int";
                //else if (type == typeof(decimal))
                //    return "decimal";
                //else if (type == typeof(object))
                //    return "object";
                //else if (type == typeof(void))
                //    return "void";
                //else if (type == typeof(double))
                //    return "double";
                //else return type.Name;

                if (type.IsPrimitive)
                    return type.Name
                        .Replace("Int32", "int")
                        .Replace("Decimal", "decimal")
                        .Replace("Object", "object")
                        .Replace("Void", "void")
                        .Replace("Double", "double")
                        .Replace("Boolean", "bool");
                else if (type == typeof(string))
                    return "string";
                else
                    return type.Name;
            }

            var sb = new StringBuilder(type.Name.Substring(0, type.Name.IndexOf('`')));

            sb.Append('<');
            var first = true;
            foreach (var t in type.GetGenericArguments())
            {
                if (!first)
                    sb.Append(',');
                sb.Append(GetName(t));
                first = false;
            }
            sb.Append('>');
            if (full)
                return string.Format("{0}.{1}", type.Namespace, sb);
            else
                return sb.ToString();
        }

        public static string GetName(this MemberInfo member, bool full = false)
        {
            if (member is PropertyInfo)
                return (member as PropertyInfo).GetName(full);
            else if (member is MethodBase)
                return (member as MethodBase).GetName(full);
            else if (member is EventInfo)
                return (member as EventInfo).GetName(full);
            else if (member is FieldInfo)
                return (member as FieldInfo).GetName(full);
            else
                return member.Name;
        }

        public static string GetName(this EventInfo method, bool full = false)
        {
            var sigBuilder = new StringBuilder();
            sigBuilder.Append(GetName(method.EventHandlerType));
            sigBuilder.Append(" ");
            sigBuilder.Append(method.Name);
            if (full)
                return string.Format("{0}.{1}.{2}", method.DeclaringType.Namespace, method.DeclaringType.Name, sigBuilder);
            else
                return sigBuilder.ToString();
        }

        public static string GetName(this MethodBase method, bool full = false)
        {
            var firstParam = true;
            var sigBuilder = new StringBuilder();

            if (method is MethodInfo)
            {
                var name = GetName((method as MethodInfo).ReturnType);
                sigBuilder.Append(name);
                sigBuilder.Append(' ');
            }

            if (full)
                sigBuilder.Append(string.Format("{0}.{1}.", method.DeclaringType.Namespace, method.DeclaringType.Name));


            sigBuilder.Append(method.Name.Replace('.', '#'));

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
                    sigBuilder.Append(GetName(g));
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
                    sigBuilder.Append(GetName(param.ParameterType));
                    sigBuilder.Append(' ');
                    sigBuilder.Append(param.Name);
                    sigBuilder.Append("]");
                }
                else
                {
                    sigBuilder.Append(GetName(param.ParameterType));
                    sigBuilder.Append(' ');
                    sigBuilder.Append(param.Name);
                }
            }
            sigBuilder.Append(")");


            return sigBuilder.ToString();
        }

        public static string GetName(this PropertyInfo pr, bool full = false)
        {
            var name = "";
            if (pr.GetMethod != null)
            {
                name = pr.GetMethod.GetName()
                                   .Replace("get_", "")
                                   .Replace("()", "")
                                   .Replace("(", "[")
                                   .Replace(")", "]");
            }
            else if (pr.SetMethod != null)
            {
                name = pr.SetMethod.GetName()
                                   .Replace("void set_", "")
                                   .Replace("(", "[");
                var i = name.LastIndexOf(", ");
                name = name.Insert(i, "]");
                name = name.Replace("], ", "] = ");
                name = name.Replace(")", "");
                return name;
            }
            else
                name = pr.ToString();

            if (full)
                return string.Format("{0}.{1}.{2}", pr.DeclaringType.Namespace, pr.DeclaringType.Name, name);
            else
                return name;
        }

        public static string GetName(this FieldInfo fi, bool full = false)
        {
            var name = String.Format("{0} {1}", GetName(fi.FieldType), fi.Name);
            if (full)
                return string.Format("{0}.{1}.{2}", fi.DeclaringType.Namespace, fi.DeclaringType.Name, name);
            else
                return name;

        }

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
                    sigBuilder.Append(GetName(g));
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
                    sigBuilder.Append(GetName(param.ParameterType));
                    sigBuilder.Append(' ');
                    sigBuilder.Append(param.Name);
                    sigBuilder.Append("]");
                }
                else
                {
                    sigBuilder.Append(GetName(param.ParameterType));
                    sigBuilder.Append(' ');
                    sigBuilder.Append(param.Name);
                }
            }
            sigBuilder.Append(")");
            return sigBuilder.ToString();
        }





    }

}