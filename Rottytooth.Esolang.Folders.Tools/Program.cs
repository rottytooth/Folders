using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rottytooth.Esolang.Folders.Tools
{
    /// <summary>
    /// A tool to read and write values
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.Error.WriteLine("Usage:");
                Console.Error.WriteLine("FoldersTools LiteralBuilder type value [add_gitignore]");
                Console.Error.WriteLine("Converts a literal like 5.6 or \"Hello, World!|\" into a series of folders");
                Console.Error.WriteLine("type = char, int, float, string");
                Console.Error.WriteLine("value = the value to convert");
                Console.Error.WriteLine("[add_gitignore] (optional) allows for adding .gitignores to all terminal folders");
                Console.Error.WriteLine("eg FoldersTools LiteralBuilder char e");
                Console.Error.WriteLine("or FoldersTools LiteralBuilder string \"A test string\" true");
                Console.Error.WriteLine("");
                Console.Error.WriteLine("FoldersTools LiteralToFolders inpath outpath");
                Console.Error.WriteLine("Converts a Folders program into a Folders.JS program (folders to arrays)");
            }

            if (args[0] == "LiteralBuilder")
            {
                LiteralToFolders.Write(args[1], args[2], true);
            }

            if (args[0] == "FoldersToArrays")
            {
                Console.WriteLine(FoldersToArrays.Build(args[1], args[2]));
            }
        }
    }
}
