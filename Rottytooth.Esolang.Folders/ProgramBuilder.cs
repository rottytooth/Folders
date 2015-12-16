using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rottytooth.Esolang.Folders
{
    public abstract class ProgramBuilder
    {

        public string Path { get; protected set; }

        /// <summary>
        /// Name of folder holding the program and the compiled executable
        /// </summary>
        public string ProgramName { get; protected set; }

        /// <summary>
        /// The program text inside of Main()
        /// </summary>
        public string ProgramText { get; protected set; }

        /// <summary>
        /// All variables are declared at global scope, at top of program
        /// </summary>
        public string Declarations { get; protected set; }

        public ProgramBuilder(string path)
        {
            this.Path = path;

            DirectoryInfo baseDir = new DirectoryInfo(Path);
            this.ProgramName = baseDir.Name.Replace(" ", String.Empty);
        }

        public abstract void BuildProgram();



        public static ProgramBuilder Generate(bool PureFolders, string path)
        {
            switch (PureFolders)
            {
                case false:
                    return new ProgramBuilderBasic(path);
                case true:
                    return new ProgramBuilderPure(path);
            }

            return new ProgramBuilderBasic(path); // it will never get here, but VS thinks it is possible so we have to add it
        }
    }
}
