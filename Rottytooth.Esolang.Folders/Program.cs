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
        internal static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("COULD NOT COMPILE: no path provided");
                return;
            }

            // If /s option is provided, put code out as C# string
            if (args.Length > 1 && args[1] == "/s")
            {
                ProgramBuilder builder = new ProgramBuilder(args[0]);
                builder.BuildProgram();
                Console.Write(builder.ProgramText);
                return;
            }


            // Otherwise, compile the program

            string errors = "";

            bool succeeded = Compile(args[0], ref errors);

            if (succeeded)
            {
                Console.WriteLine("Complete");
            }
            else
            {
                Console.Error.WriteLine("Could not compile.\nERRORS:\n\n");
                Console.Error.WriteLine(errors);
            }
        }

        public static bool Compile(string path, ref string errors)
        {
            ProgramBuilder builder = new ProgramBuilder(path);

            builder.BuildProgram();

            CSharpCodeProvider csc =
                new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
            CompilerParameters parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" },
                builder.ProgramName + ".exe", true);
            parameters.GenerateExecutable = true;
            parameters.ReferencedAssemblies.Add("Rottytooth.Esolang.Folders.Runtime.dll");

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
                  public static void Main(string[] args) {
                    " + builder.ProgramText + @"
                  }
                }";

            CompilerResults results = csc.CompileAssemblyFromSource(parameters, entireProgram);

            // output any errors
            StringBuilder errorList = new StringBuilder();

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
