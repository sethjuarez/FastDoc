using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace FastDoc.Core
{
    [Serializable]
    public class ItemNode : Node
    {
        public ItemType ItemType { get; set; }
        public Type Type { get; set; }

        public static IEnumerable<ItemNode> GetItems(Assembly asmbly, string nmspc)
        {
            var q = from t in asmbly.ExportedTypes
                    where t.Namespace == nmspc
                    select t;


            foreach (var type in q)
            {
                var item = new ItemNode
                {
                    Name = type.GetName(),
                    FullName = type.GetName(full: true),
                    Type = type,
                    ItemType = type.GetStructureType()
                };

                foreach (var m in MemberNode.GetMembers(type))
                    item.Push(m);

                yield return item;
            }
        }

        public override string ToString()
        {
            return Type.FullName;
        }
    }
}
