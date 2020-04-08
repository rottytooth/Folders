using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rottytooth.Esolang.Folders.LiteralBuilder
{
    /// <summary>
    /// A tool to read and write values
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage:");
                Console.Error.WriteLine("LiteralBuilder type value [add_gitignore]");
                Console.Error.WriteLine("type = char, int, float, string");
                Console.Error.WriteLine("value = the value to convert");
                Console.Error.WriteLine("[add_gitignore] (optional) allows for adding .gitignores to all terminal folders");
                Console.Error.WriteLine("eg LiteralBuilder char e");
                Console.Error.WriteLine("or LiteralBuilder string \"A test string\" true");
            }

            FolderTranslator.Write(args[0], args[1], true);
        }
    }
}
