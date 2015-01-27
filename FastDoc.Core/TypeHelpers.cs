using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace FastDoc.Core
{
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
    }
}
