using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace FastDoc.Core
{
    [Serializable]
    public class MemberNode : Node
    {
        public MemberTypes MemberType { get; set; }
        public MemberInfo MemberInfo { get; set; }

        public static IEnumerable<MemberNode> GetMembers(Type t)
        {
            foreach (var member in t.GetMembers())
            {
                if (!member.Name.Contains("get_") && !member.Name.Contains("set_"))
                    yield return new MemberNode
                    {
                        Name = member.GetName(),
                        FullName = member.GetName(full: true),
                        MemberType = member.MemberType,
                        MemberInfo = member
                    };
            }
        }

        public override string ToString()
        {
            return MemberInfo.GetMemberElementName();
        }
    }
}
