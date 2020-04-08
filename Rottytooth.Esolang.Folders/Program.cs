using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using Rottytooth.Esolang.Folders.Runtime;

namespace Rottytooth.Esolang.Folders
{
    public class Program
    {
        const string ERROR_STRING = "Could not compile.\nERRORS:\n\n";

        const string HOW_TO_FOLDER =
            "USAGE: Folders [options] path_to_root_folder\n\nOptions include:/sTranspile to C# and output the C# rather than building\n/eCreate an exe, rather than running the code immediately";

        internal static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine(ERROR_STRING + "no path provided");
                return;
            }

            string path;
            string[] arguments = GetOptions(args, out path);

            // If /b option is provided, use "basic" or "concise" or "classic" folders interpreting

            // Basic Folders is now legacy
            bool pureFolders = !(arguments.Contains("b"));

            bool exe = !(arguments.Contains("e"));


            // If /s option is provided, put code out as C# string
            if (arguments.Contains("s"))
            {
                ProgramBuilder builder = ProgramBuilder.Generate(pureFolders, path);
                builder.BuildProgram();
                Console.Write(builder.ProgramText);
                return;
            }

            // Otherwise, compile the program

            string errors = "";

            bool succeeded = Compile(path, ref errors, exe, pureFolders);

            Console.WriteLine(); // clear line after program output
            if (succeeded)
            {
                Console.WriteLine("Complete");
            }
            else
            {
                Console.Error.Write(ERROR_STRING);
                Console.Error.WriteLine(errors);
            }
        }

        public static string[] GetOptions(string[] args, out string path)
        {
            var arguments = new List<string>();
            string outpath = "";

            foreach(string arg in args)
            {
                if (arg[0] == '/' || arg[0] == '-')
                {
                    arguments.Add(arg.Substring(1));
                }
                else
                {
                    outpath = arg;
                }
            }

            path = outpath;
            return arguments.ToArray();
        }

        /// <summary>
        /// Actually build the Folders program
        /// </summary>
        /// <param name="path">path to root directory of the Folders program</param>
        /// <param name="errors">used to return error messages</param>
        /// <param name="exe">whether we create an exe or simply compile + run in memory</param>
        /// <param name="pureFolders">Pure Folders (non-semantic folder names) vs the (legacy) Classic Folders Syntax</param>
        /// <returns></returns>
        public static bool Compile(string path, ref string errors, bool exe, bool pureFolders)
        {
            ProgramBuilder builder = ProgramBuilder.Generate(pureFolders, path);
            builder.BuildProgram();

            StringBuilder errorList = new StringBuilder();

            CompilerResults results;

            string entireProgram =
                @"using System;
                using System.IO;
                using Rottytooth.Esolang.Folders.Runtime;
                public static class Program {

                    private static VarManager _varManager;

                    static Program() {
                        _varManager = new VarManager(""" + builder.ProgramName + @""");
                    }

                " + builder.Declarations +
                @"
                  public static void Main() {
                    " + builder.ProgramText + @"
                  }
                }
                
                public class StartUp
                {
                    public void Execute()
                    {
                        Program.Main();
                    }
                }";

            using (CSharpCodeProvider csc =
                new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } }))
            {

                if (exe) // building to an executable
                {
                    CompilerParameters parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" },
                        builder.ProgramName + ".exe", true);
                    parameters.ReferencedAssemblies.Add("Rottytooth.Esolang.Folders.Runtime.dll");
                    parameters.GenerateExecutable = true;

                    results = csc.CompileAssemblyFromSource(parameters, entireProgram);

                }
                else // compiling in-memory and executing
                {
                    CompilerParameters parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" })
                        { GenerateInMemory = true };
                    parameters.ReferencedAssemblies.Add("Rottytooth.Esolang.Folders.Runtime.dll");

                    results = csc.CompileAssemblyFromSource(parameters, entireProgram);

                    if (results.Errors != null && results.Errors.Count == 0)
                    {
                        Type type = results.CompiledAssembly.GetType("StartUp");

                        var obj = Activator.CreateInstance(type);

                        var output = type.GetMethod("Execute").Invoke(obj, new object[] { });
                    }
                }
            }

            // output any errors
            results.Errors.Cast<CompilerError>().ToList().ForEach(error => errorList.AppendLine(error.ErrorText));

            if (errorList.Length > 0)
            {
                errors = errorList.ToString();
                return false;
            }

            // we have successfully compiled
            return true;
        }
    }
}
