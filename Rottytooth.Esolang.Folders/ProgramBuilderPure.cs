using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rottytooth.Esolang.Folders
{

    /// <summary>
    /// Folders program builder with v2.0 style parsing
    /// </summary>
    public class ProgramBuilderPure : ProgramBuilder
    {

        public ProgramBuilderPure(string path) : base(path) { }

        /// <summary>
        /// Navigates folder structure and returns program in C#
        /// </summary>
        /// <returns>runnable C#, minus class and method</returns>
        public override void BuildProgram()
        {
            StringBuilder program = new StringBuilder();
            StringBuilder declarations = new StringBuilder();

            DirectoryInfo baseDir = new DirectoryInfo(Path);


            foreach (DirectoryInfo subdir in baseDir.GetDirectories().CustomSort())
            {
                ParseCommand(subdir.FullName, program, declarations);
            }

            this.ProgramText = program.ToString();
            this.Declarations = declarations.ToString();
        }

        private void ParseCommand(string path, StringBuilder program, StringBuilder declarations)
        {
            DirectoryInfo baseDir = new DirectoryInfo(path);

            DirectoryInfo[] subDirs = baseDir.GetDirectories().CustomSort().ToArray();

            if (subDirs.Length < 2)
            {
                throw new SyntaxError("A command needs at least two subdirectories -- bad path at" + baseDir.FullName);
            }
            int commandID = subDirs[0].GetDirectories().Length;


            DirectoryInfo[] commandDirs; // for subcommands

            try
            {
                switch (commandID)
                {
                    case (int)CommandEnum.If:
                        program.Append("if(");
                        ParseExpression(subDirs[1].FullName, program); // second directory is the expression
                        program.Append(")");
                        program.Append("\n{\n");

                        commandDirs = subDirs[2].GetDirectories().CustomSort().ToArray();

                        for (int i = 0; i < commandDirs.Length; i++) // third directory is the list of commands to carry out
                        {
                            ParseCommand(commandDirs[i].FullName, program, declarations);
                        }
                        program.Append("\n}\n");
                        break;

                    case (int)CommandEnum.While:
                        program.Append("while(");
                        ParseExpression(baseDir.GetDirectories()[1].FullName, program);
                        program.Append(")");
                        program.Append("\n{\n");

                        commandDirs = subDirs[2].GetDirectories().CustomSort().ToArray();

                        for (int i = 0; i < commandDirs.Length; i++)
                        {
                            ParseCommand(commandDirs[i].FullName, program, declarations);
                        }
                        program.Append("\n}\n");
                        break;
                    case (int)CommandEnum.Declare:
                        string variableType = ParseType(baseDir.GetDirectories()[1].FullName).ToString().ToLower();
                        string variableName = "Var" + baseDir.GetDirectories()[2].GetDirectories().Length.ToString();

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
                    case (int)CommandEnum.Let:
                        // second directory is the variable name
                        program.Append(" Var");
                        program.Append(subDirs[1].GetDirectories().Length);

                        // third directory is the expression
                        program.Append(" = (");
                        ParseExpression(subDirs[2].FullName, program);
                        program.Append(");\n");
                        break;
                    case (int)CommandEnum.Print:
                        program.Append("Console.Write(");
                        ParseExpression(subDirs[1].FullName, program);
                        program.Append(".ToString()");
                        program.Append(");\n");
                        break;
                    case (int)CommandEnum.Input:
                        program.Append(" Var");
                        program.Append(subDirs[1].GetDirectories().Length);
                        program.Append(" = Console.ReadLine(");
                        program.Append(");\n");
                        break;
                    default:

                        throw new SyntaxError("Could not determine type of command at " + baseDir.FullName);
                }
            }
            catch (Exception)
            {
                throw new SyntaxError("Command failed to build at path " + baseDir.FullName);
            }
        }

        private void ParseExpression(string path, StringBuilder program)
        {
            DirectoryInfo baseDir = new DirectoryInfo(path);

            DirectoryInfo[] subDirs = baseDir.GetDirectories().CustomSort().ToArray();

            if (subDirs.Length < 2)
            {
                throw new SyntaxError("An expression needs at least two subdirectories -- bad path at" + baseDir.FullName);
            }
            int expressionID = subDirs[0].GetDirectories().Length;

            try
            {
                switch (expressionID)
                {
                    case (int)ExpressionEnum.Variable:
                        // variable is easy, we just get the number of folders inside the second folder and that tells us the name
                        int intVar = subDirs[1].GetDirectories().Length;
                        program.Append(" Var" + intVar.ToString());
                        break;
                    case (int)ExpressionEnum.Add:
                        // second and third folders give us the two things to add
                        program.Append("(");
                        ParseExpression(subDirs[1].FullName, program);
                        program.Append(" + ");
                        ParseExpression(subDirs[2].FullName, program);
                        program.Append(" ) ");
                        break;
                    case (int)ExpressionEnum.Subtract:
                        // second and third folders give us the two things to subtract
                        program.Append("(");
                        ParseExpression(subDirs[1].FullName, program);
                        program.Append(" - ");
                        ParseExpression(subDirs[2].FullName, program);
                        program.Append(" ) ");
                        break;
                    case (int)ExpressionEnum.Multiply:
                        // second and third folders give us the two things to multiply
                        program.Append("(");
                        ParseExpression(subDirs[1].FullName, program);
                        program.Append(" * ");
                        ParseExpression(subDirs[2].FullName, program);
                        program.Append(" ) ");
                        break;
                    case (int)ExpressionEnum.Divide:
                        // second and third folders give us the two things to divide
                        program.Append("(");
                        ParseExpression(subDirs[1].FullName, program);
                        program.Append(" * ");
                        ParseExpression(subDirs[2].FullName, program);
                        program.Append(" ) ");
                        break;
                    case (int)ExpressionEnum.LiteralValue:
                        // first, get type
                        TypeEnum type = ParseType(subDirs[1].FullName);
                        int value;

                        switch (type)
                        {
                            case TypeEnum.Int:
                                value = GetEncodedValueFromSubdirectories(subDirs[2].FullName);
                                program.Append(" " + value.ToString() + " ");
                                break;
                            case TypeEnum.Float:
                                value = GetEncodedValueFromSubdirectories(subDirs[2].FullName);
                                program.Append(" " + ((double)value).ToString() + " ");
                                break;
                            case TypeEnum.String:
                                StringBuilder s = new StringBuilder();
                                foreach (DirectoryInfo charDir in subDirs[2].GetDirectories().CustomSort())
                                {
                                    s.Append((char)GetEncodedValueFromSubdirectories(charDir.FullName));
                                }
                                program.Append("\"" + s.Replace("\"", "\\\"") + "\"");
                                break;
                            case TypeEnum.Char:
                                value = GetEncodedValueFromSubdirectories(subDirs[2].FullName);
                                program.Append("'" + ((char)value).ToString() + "'");
                                break;
                        }
                        break;
                    case (int)ExpressionEnum.EqualTo:
                        program.Append("(");
                        ParseExpression(subDirs[1].FullName, program);
                        program.Append(" == ");
                        ParseExpression(subDirs[2].FullName, program);
                        program.Append(")");
                        break;
                    case (int)ExpressionEnum.GreaterThan:
                        program.Append("(");
                        ParseExpression(subDirs[1].FullName, program);
                        program.Append(" > ");
                        ParseExpression(subDirs[2].FullName, program);
                        program.Append(")");
                        break;
                    case (int)ExpressionEnum.LessThan:
                        program.Append("(");
                        ParseExpression(subDirs[1].FullName, program);
                        program.Append(" < ");
                        ParseExpression(subDirs[2].FullName, program);
                        program.Append(")");
                        break;
                    default:
                        throw new SyntaxError("Could not determine type of expression at path " + baseDir.FullName);
                }
            }
            catch (Exception)
            {
                throw new SyntaxError("Expression failed to build at path " + baseDir.FullName);
            }
        }

        /// <summary>
        /// For a directory, reads the number contained
        /// </summary>
        /// <param name="path">Path for where to look for value</param>
        /// <returns>value as an int</returns>
        private int GetEncodedValueFromSubdirectories(string path)
        {
            // The value is stored in an additional folder, the mapping is like this:
            //  - each subfolder is 4 bits
            //  - each of these subfolders has (up to) 4 folders, each containing a bit (let's call them sub-sub-folders)
            //  - they are read alphabetically
            //  - if the sub-sub-folder has a folder, it's a 1, otherwise, it's a zero
            //
            // So like this:
            // ---- 0
            // |
            // ----|----1
            // |
            // ---- 0
            // |
            // ----|--- 1
            // Equals 0101, or 0x5 for the first entry

            DirectoryInfo rootOfNumber = new DirectoryInfo(path);
            DirectoryInfo[] hexDigits = rootOfNumber.GetDirectories();

            int currentValue = 0;
            int nibDigitCounter = 0;
            int bitDigitCounter = 0;

            foreach(DirectoryInfo hexdir in hexDigits.CustomSort().ToArray().Reverse())
            {
                foreach (DirectoryInfo bitdir in hexdir.GetDirectories().CustomSort().ToArray().Reverse())
                {
                    currentValue += (int)Math.Pow(2.0, (double)bitDigitCounter) * bitdir.GetDirectories().Length;
                    bitDigitCounter++;
                }
                nibDigitCounter++;
                bitDigitCounter = nibDigitCounter * 4;
            }

            return currentValue;
        }

        // the number of subdirectories tells us what type something is -- can check the enum for mapping
        private TypeEnum ParseType(string path)
        {
            DirectoryInfo baseDir = new DirectoryInfo(path);
            TypeEnum type = (TypeEnum)baseDir.GetDirectories().Length;
            return type;
        }
    }
}
