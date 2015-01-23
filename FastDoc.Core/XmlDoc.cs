using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDoc.Core
{
    public class XmlDoc
    {
        public string File { get; set; }

        /// <summary>
        /// Initializes a new instance of the XmlDoc class.
        /// </summary>
        /// <param name="file"></param>
        public XmlDoc(string file)
        {
            File = file;
        }

        public string GetDocumentation(ItemType type, string path)
        {
            return "";
        }

        public string GetDocumentation(MemberType type, string path)
        {
            return "";
        }
    }
}
