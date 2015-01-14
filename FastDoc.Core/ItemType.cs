using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace FastDoc.Core
{
    public enum ItemType
    {
        Class,
        Interface,
        Enum,
        Struct,
        Delegate,
        Unknown
    }

    public enum MemberType
    {
        Constant,
        Field,
        Method,
        Property,
        Indexer,
        Event,
        Operator,
        Constructor,
        Destructor,
        Type,
        Unknown
    }

    public enum InformationType
    {
        C,
        Code,
        Example,
        Exception,
        List,
        Para,
        Param,
        ParamRef,
        Remarks,
        Returns,
        See,
        SeeAlso,
        Summary,
        Value
    }


    public static class TypeHelpers
    {
        public static ItemType GetStructureType(this Type t)
        {
            if (t.IsClass)
                return ItemType.Class;
            else if (t.IsInterface)
                return ItemType.Interface;
            else if (t.IsEnum)
                return ItemType.Enum;
            else if (t.IsValueType)
                return ItemType.Struct;
            else if (t.IsSubclassOf(typeof(Delegate)))
                return ItemType.Delegate;
            else
                return ItemType.Unknown;
        }

        public static MemberType GetMemberType(this MemberInfo m)
        {
            if (m is ConstructorInfo)
                return MemberType.Constructor;
            else if (m is MethodInfo)
                return MemberType.Method;
            else if (m is EventInfo)
                return MemberType.Event;
            else if (m is FieldInfo)
                return MemberType.Field;
            else if (m is PropertyInfo)
                return MemberType.Property;
            else
                return MemberType.Unknown;
        }
    }
}
