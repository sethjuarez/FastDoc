using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FastDoc.Core.Tests
{
    [TestFixture]
    public class XmlDocTests
    {

        [Test]
        public void RetrieveAssembly()
        {

            var assembly = Assembly.LoadFrom("FastDoc.Core.Tests.dll");
            var root = Node.Generate(assembly);
        }

        [Test]
        public void RetrieveMethod()
        {
            XmlDoc doc = new XmlDoc("test.xml");

        }
    }
}
