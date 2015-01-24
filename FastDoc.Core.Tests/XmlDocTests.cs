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
        private Assembly _assembly;
        public Assembly Assembly
        {
            get
            {
                if (_assembly == null)
                    _assembly = Assembly.LoadFrom("SampleLib.dll");
                return _assembly;
            }

        }


        [Test]
        public void RetrieveAssembly()
        {
            var root = Node.Generate(Assembly);
        }

    }
}
