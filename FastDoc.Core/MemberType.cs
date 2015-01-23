using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace FastDoc.Core
{
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
}
