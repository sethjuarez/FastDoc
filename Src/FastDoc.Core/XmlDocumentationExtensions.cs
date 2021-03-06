﻿// Reading XML Documentation at Run-Time
// Bradley Smith - 2010/11/25

using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace FastDoc.Core
{
    /// <summary>
    /// Provides extension methods for reading XML comments from reflected members.
    /// </summary>
    internal static class XmlDocumentationExtensions
    {

        /// <summary>The cached XML.</summary>
        private static Dictionary<string, XDocument> cachedXml;

        /// <summary>Static constructor.</summary>
        static XmlDocumentationExtensions()
        {
            cachedXml = new Dictionary<string, XDocument>(StringComparer.OrdinalIgnoreCase);
        }
        /// <summary>Convert parameter.</summary>
        /// <param name="p">The ParameterInfo to process.</param>
        /// <returns>The parameter converted.</returns>
        private static string ConvertParameter(ParameterInfo p)
        {
            if (p.ParameterType.FullName == null) return "";

            if (p.ParameterType.IsGenericType)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(p.ParameterType.FullName.Substring(0, p.ParameterType.FullName.IndexOf('`')));
                sb.Append("{");
                sb.Append(string.Join(",", p.ParameterType.GetGenericArguments().Select(x => x.FullName)));
                sb.Append("}");
                return sb.ToString();
            }
            else return p.ParameterType.FullName ?? "";
        }
        /// <summary>Gets parameter list.</summary>
        /// <param name="method">The method.</param>
        /// <returns>The parameter list.</returns>
        private static string GetParameterList(MethodBase method)
        {
            string paramTypesList = String.Join(
                                    ",",
                                    method.GetParameters()
                                        .Cast<ParameterInfo>()
                                        .Select(ConvertParameter)
                                        .ToArray()
                                );
            return paramTypesList;
        }
        /// <summary>
        /// Returns the expected name for a member element in the XML documentation file.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or
        /// illegal values.</exception>
        /// <param name="member">The reflected member.</param>
        /// <returns>The name of the member element.</returns>
        public static string GetMemberElementName(this MemberInfo member)
        {
            char prefixCode;
            string memberName = (member is Type)
                ? ((Type)member).FullName                               // member is a Type
                : (string.Format("{0}.{1}", member.DeclaringType.FullName, member.Name));  // member belongs to a Type

            switch (member.MemberType)
            {
                case MemberTypes.Constructor:
                    // XML documentation uses slightly different constructor names
                    memberName = memberName.Replace(".ctor", "#ctor");
                    goto case MemberTypes.Method;
                case MemberTypes.Method:
                    prefixCode = 'M';

                    // parameters are listed according to their type, not their name
                    var method = (MethodBase)member;
                    string paramTypesList = GetParameterList(method);
                    if (!String.IsNullOrEmpty(paramTypesList)) memberName += string.Format("({0})", paramTypesList);
                    break;

                case MemberTypes.Event:
                    prefixCode = 'E';
                    break;

                case MemberTypes.Field:
                    prefixCode = 'F';
                    break;

                case MemberTypes.NestedType:
                    // XML documentation uses slightly different nested type names
                    memberName = memberName.Replace('+', '.');
                    goto case MemberTypes.TypeInfo;
                case MemberTypes.TypeInfo:
                    prefixCode = 'T';
                    break;

                case MemberTypes.Property:
                    prefixCode = 'P';
                    break;

                default:
                    throw new ArgumentException("Unknown member type", "member");
            }

            // elements are of the form "M:Namespace.Class.Method"
            return String.Format("{0}:{1}", prefixCode, memberName);
        }
        /// <summary>Returns the XML documentation (summary tag) for the specified member.</summary>
        /// <param name="member">The reflected member.</param>
        /// <returns>The contents of the summary tag for the member.</returns>
        public static string GetXmlDocumentation(this MemberInfo member)
        {
            AssemblyName assemblyName = member.Module.Assembly.GetName();
            return GetXmlDocumentation(member, assemblyName.Name + ".xml");
        }
        /// <summary>Returns the XML documentation (summary tag) for the specified member.</summary>
        /// <param name="member">The reflected member.</param>
        /// <param name="pathToXmlFile">Path to the XML documentation file.</param>
        /// <returns>The contents of the summary tag for the member.</returns>
        public static string GetXmlDocumentation(this MemberInfo member, string pathToXmlFile)
        {
            AssemblyName assemblyName = member.Module.Assembly.GetName();
            XDocument xml = null;

            if (cachedXml.ContainsKey(assemblyName.FullName))
                xml = cachedXml[assemblyName.FullName];
            else
                cachedXml[assemblyName.FullName] = (xml = XDocument.Load(pathToXmlFile));

            return GetXmlDocumentation(member, xml);
        }
        /// <summary>Returns the XML documentation (summary tag) for the specified member.</summary>
        /// <param name="member">The reflected member.</param>
        /// <param name="xml">XML documentation.</param>
        /// <returns>The contents of the summary tag for the member.</returns>
        public static string GetXmlDocumentation(this MemberInfo member, XDocument xml)
        {
            var memberPath = GetMemberElementName(member);
            return xml.XPathEvaluate(
                String.Format(
                    "string(/doc/members/member[@name='{0}']/summary)",
                    memberPath
                )
            ).ToString().Trim();
        }
        /// <summary>
        /// Returns the XML documentation (returns/param tag) for the specified parameter.
        /// </summary>
        /// <param name="parameter">The reflected parameter (or return value).</param>
        /// <returns>The contents of the returns/param tag for the parameter.</returns>
        public static string GetXmlDocumentation(this ParameterInfo parameter)
        {
            AssemblyName assemblyName = parameter.Member.Module.Assembly.GetName();
            return GetXmlDocumentation(parameter, assemblyName.Name + ".xml");
        }
        /// <summary>
        /// Returns the XML documentation (returns/param tag) for the specified parameter.
        /// </summary>
        /// <param name="parameter">The reflected parameter (or return value).</param>
        /// <param name="pathToXmlFile">Path to the XML documentation file.</param>
        /// <returns>The contents of the returns/param tag for the parameter.</returns>
        public static string GetXmlDocumentation(this ParameterInfo parameter, string pathToXmlFile)
        {
            AssemblyName assemblyName = parameter.Member.Module.Assembly.GetName();
            XDocument xml = null;

            if (cachedXml.ContainsKey(assemblyName.FullName))
                xml = cachedXml[assemblyName.FullName];
            else
                cachedXml[assemblyName.FullName] = (xml = XDocument.Load(pathToXmlFile));

            return GetXmlDocumentation(parameter, xml);
        }
        /// <summary>
        /// Returns the XML documentation (returns/param tag) for the specified parameter.
        /// </summary>
        /// <param name="parameter">The reflected parameter (or return value).</param>
        /// <param name="xml">XML documentation.</param>
        /// <returns>The contents of the returns/param tag for the parameter.</returns>
        public static string GetXmlDocumentation(this ParameterInfo parameter, XDocument xml)
        {
            if (parameter.IsRetval || String.IsNullOrEmpty(parameter.Name))
                return xml.XPathEvaluate(
                    String.Format(
                        "string(/doc/members/member[@name='{0}']/returns)",
                        GetMemberElementName(parameter.Member)
                    )
                ).ToString().Trim();
            else
                return xml.XPathEvaluate(
                    String.Format(
                        "string(/doc/members/member[@name='{0}']/param[@name='{1}'])",
                        GetMemberElementName(parameter.Member),
                        parameter.Name
                    )
                ).ToString().Trim();
        }
    }
}
