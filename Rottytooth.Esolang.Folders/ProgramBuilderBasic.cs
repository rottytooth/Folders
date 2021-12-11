using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Rottytooth.Esolang.Folders.Runtime;

namespace Rottytooth.Esolang.Folders
{
    public class ProgramBuilderBasic : ProgramBuilder
    {

        public ProgramBuilderBasic(string path) : base(path) { }

        /// <summary>
        /// If subdirectory doesn't exist, create it -- otherwise, delete everything in it
        /// </summary>
        /// <param name="baseDir"></param>
        /// <param name="subdirName"></param>
        private void MakeSubdirectory(DirectoryInfo baseDir, string subdirName)
        {
            DirectoryInfo subdir = new DirectoryInfo(baseDir.FullName + "\\" + subdirName);

            if (!subdir.Exists)
                subdir.Create();

            foreach (FileInfo file in subdir.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in subdir.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        /// <summary>
        /// Navigates folder structure and returns program in C#
        /// </summary>
        /// <returns>runnable C#, minus class and method</returns>
        public override void BuildProgram()
        {
            StringBuilder program = new StringBuilder();
            StringBuilder declarations = new StringBuilder();

            DirectoryInfo baseDir = new DirectoryInfo(Path);


            foreach (DirectoryInfo subdir in baseDir.GetDirectories())
            {
                ParseCommand(subdir.FullName, program, declarations);
            }

            this.ProgramText = program.ToString();
            this.Declarations = declarations.ToString();
        }

        private void ParseCommand(string path, StringBuilder program, StringBuilder declarations)
        {
            DirectoryInfo baseDir = new DirectoryInfo(path);

            string commandtext = ResolveName(baseDir.Name);

            DirectoryInfo[] subdir;

            switch (commandtext)
            {
                case "print":
                    program.Append("Console.Write(");
                    subdir = baseDir.GetDirectories();
                    for (int i = 0; i < subdir.Length; i++)
                    {
                        if (i > 0)
                        {
                            program.Append(" + ");
                        }
                        ParseExpression(subdir[i].FullName, program);
                        program.Append(".ToString()");
                    }
                    program.Append(");\n");
                    break;
                case "input":
                    program.Append(" Var");
                    subdir = baseDir.GetDirectories();
                    program.Append(subdir[1].GetDirectories().Length);
                    program.Append(" = Console.ReadLine(");
                    program.Append(");\n");
                    break;
                case "if":
                    program.Append("if(");
                    ParseExpression(baseDir.GetDirectories()[0].FullName, program);
                    program.Append(")");
                    program.Append("\n{\n");
                    subdir = baseDir.GetDirectories();
                    for (int i = 0; i < subdir.Length; i++)
                    {
                        ParseCommand(subdir[i].FullName, program, declarations);
                    }
                    program.Append("\n}\n");
                    break;
                case "let":
                    DirectoryInfo[] subdirs = baseDir.GetDirectories();
                    program.Append(ResolveName(subdirs[0].Name));
                    program.Append(" = ");
                    for (int i = 1; i < subdirs.Length; i++)
                        ParseExpression(subdirs[i].FullName, program);
                    program.Append(";\n");
                    break;
                case "declare":
                    string variableType = ResolveName(baseDir.GetDirectories()[1].Name);
                    string variableName = ResolveName(baseDir.GetDirectories()[0].Name);

                    declarations.Append("\npublic static ");
                    declarations.Append(variableType);
                    declarations.Append(" ");
                    declarations.Append(variableName);
                    declarations.AppendLine(@" {
                        get
                        {
                            return (" + variableType + @") _varManager.GetVariable(""" + variableName + @""");
                        } set {
                            _varManager.SetVariable(""" + variableName + @""", value);
                        }
                    }");

                    break;
                case "while":
                    program.Append("while(");
                    ParseExpression(baseDir.GetDirectories()[0].FullName, program);
                    program.Append(")");
                    program.Append("\n{\n");
                    subdir = baseDir.GetDirectories();
                    for (int i = 1; i < subdir.Length; i++)
                    {
                        ParseCommand(subdir[i].FullName, program, declarations);
                    }
                    program.Append("\n}\n");
                    break;
            }
        }

        private void ParseExpression(string path, StringBuilder program)
        {
            DirectoryInfo baseDir = new DirectoryInfo(path);

            string commandtext = ResolveName(baseDir.Name);

            switch (commandtext)
            {
                case "string": // if it's a string, grab the content and put quotes around it
                    program.Append("\"" + SpecialSymbols.Decode(baseDir.GetDirectories()[0].Name) + "\"");
                    break;
                case "int":
                case "float":
                case "char":
                    program.Append(SpecialSymbols.Decode(baseDir.GetDirectories()[0].Name));
                    break;
                default: // if we don't know what it is, treat as a literal
                    program.Append(ResolveName(baseDir.Name));
                    break;
            }
        }

        /// <summary>
        /// Takes folder name and returns a cleaned up version, with alternate names swapped out
        /// </summary>
        /// <param name="foldername">name of folder as it is on file system</param>
        /// <returns>resulting command name</returns>
        private string ResolveName(string foldername)
        {
            // allow for ordering of folders, multiple folders with same command
            foldername = GetRegex(foldername, @"\d+ (.*)"); // remove leading number
            foldername = GetRegex(foldername, @"(.*) \(\d+\)"); // remove parentheses & number from end
            foldername = GetRegex(foldername, @"(.*) - Copy"); // remove Copy

            foldername = SpecialSymbols.Decode(foldername);

            // if we have a command using an alternate name, match it here
            if (SpecialSymbols.AltCommandNames.Keys.Contains(foldername.Trim()))
            {
                foldername = SpecialSymbols.AltCommandNames[foldername];
            }
            return foldername.Trim();
        }


        private string GetRegex(string commandtext, string pattern)
        {
            Match removeHeading = Regex.Match(commandtext, pattern);
            if (removeHeading.Groups.Count > 1 && removeHeading.Groups[1].Success)
            {
                return removeHeading.Groups[1].Value;
            }
            return commandtext;
        }
    }
}
