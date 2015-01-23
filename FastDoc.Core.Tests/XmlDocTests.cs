using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDoc.Core.Tests
{
    [TestFixture]
    public class XmlDocTests
    {
        [Test]
        public void RetrieveMethod()
        {
            XmlDoc doc = new XmlDoc("numl.xml");
            
        }
    }
}
