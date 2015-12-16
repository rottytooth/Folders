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

            HelloWorldBasic();

            Console.WriteLine();

            NinetyNineBottlesBasic();

            Console.WriteLine();

            LongStringBasic();

            Console.WriteLine();

            HelloWorldPure();

            Console.WriteLine();
        }

        [TestMethod]
        public static void HelloWorldBasic()
        {
            string errors = "";
            Assert.IsTrue(Folders.Program.Compile(@"BasicPrograms\Hello World", ref errors, false, false));
        }

        [TestMethod]
        public static void NinetyNineBottlesBasic()
        {
            string errors = "";
            Assert.IsTrue(Folders.Program.Compile(@"BasicPrograms\99 Bottles", ref errors, false, false));
        }

        [TestMethod]
        public static void LongStringBasic()
        {
            string errors = "";
            Assert.IsTrue(Folders.Program.Compile(@"BasicPrograms\Long String", ref errors, false, false));
        }

        [TestMethod]
        public static void HelloWorldPure()
        {
            string errors = "";
            Assert.IsTrue(Folders.Program.Compile(@"PurePrograms\Hello World", ref errors, false, true));
        }

    }
}
