using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Folders = Rottytooth.Esolang.Folders;

namespace Rottytooth.Esolang.Folders.SamplePrograms
{
    [TestClass]
    public class Program
    {
        public static void Main(string[] args)
        {
            // Run each of the sample programs

            HelloWorld();

            NinetyNineBottles();

            LongString();
        }

        [TestMethod]
        public static void HelloWorld()
        {
            string errors = "";
            Assert.IsTrue(Folders.Program.Compile(@"Programs\Hello World", ref errors));
        }

        [TestMethod]
        public static void NinetyNineBottles()
        {
            string errors = "";
            Assert.IsTrue(Folders.Program.Compile(@"Programs\99 Bottles", ref errors));
        }

        [TestMethod]
        public static void LongString()
        {
            string errors = "";
            Assert.IsTrue(Folders.Program.Compile(@"Programs\Long String", ref errors));
        }

    }
}
