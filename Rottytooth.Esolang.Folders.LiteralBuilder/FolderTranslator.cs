using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rottytooth.Esolang.Folders;
using System.IO;

namespace Rottytooth.Esolang.Folders.LiteralBuilder
{
    class FolderTranslator
    {
        public static void Write(string type, string value, bool writeGitIgnores = false)
        {
            string path = @"Z:\out";

            // clear all directories from this one
            foreach (DirectoryInfo dir in new DirectoryInfo(path).GetDirectories())
            {
                dir.Delete(true);
            }

            // the three directories in every literal
            DirectoryInfo literalDir = CreateFolder(path, 1);
            DirectoryInfo typeDir = CreateFolder(path, 2);
            DirectoryInfo expDir = CreateFolder(path, 3);

            // the literal dir marks it as type literal. This requires 5 folders
            CreateFolders(literalDir.FullName, 5);

            switch(type)
            {
                case "int":
                    int intValue = 0;
                    if (!Int32.TryParse(value.Trim(), out intValue))
                    {
                        throw new Exception("Invalid int passed");
                    }
                    CreateFolders(typeDir.FullName, (int)TypeEnum.Int);
                    NumberToFoldersHex(expDir.FullName, intValue);
                    break;
                case "char":
                    char charValue = '\0';
                    if (!Char.TryParse(value.Trim(), out charValue))
                    {
                        throw new Exception("Invalid char passed");
                    }
                    CreateFolders(typeDir.FullName, (int)TypeEnum.Char);
                    NumberToFoldersHex(expDir.FullName, (int)charValue);
                    break;
                case "string":
                case "str":
                    CreateFolders(typeDir.FullName, (int)TypeEnum.String);
                    for (int letterCount = 0; letterCount < value.Trim().Length; letterCount++)
                    {
                        DirectoryInfo currLtr = CreateFolder(expDir.FullName, letterCount + 1);
                        NumberToFoldersHex(currLtr.FullName, (int)value.Trim()[letterCount]);
                    }
                    break;
                case "float":
                    // FIXME: floats seem fundamentally broken in Folders and mays need some rethinking
                    break;
            }

            if (writeGitIgnores) WriteGitIgnores(path);
        }

        private static void WriteGitIgnores(string path)
        {
            string[] subdirs = Directory.GetDirectories(path);
            if (subdirs.Length > 0)
            {
                foreach (string subdir in subdirs) 
                    WriteGitIgnores(Path.Combine(path, subdir));
            }
            else File.Create(Path.Combine(path, ".gitignore"));
        }

        /// <summary>
        /// Create a single folder called New Folder, with the number provided in parantheses, if
        /// greater than one
        /// </summary>
        /// <param name="path">parent folder</param>
        /// <param name="digitCount"></param>
        /// <returns></returns>
        private static DirectoryInfo CreateFolder(string path, int digitCount)
        {
            return Directory.CreateDirectory(Path.Combine(path, "New Folder" + (digitCount > 1 ? $" ({digitCount})" : "")));
        }

        /// <summary>
        /// Create the number of folders asked for
        /// </summary>
        /// <param name="path"></param>
        /// <param name="value"></param>
        private static void CreateFolders(string path, int value)
        {
            for (int count = 0; count < value; count++)
            {
                CreateFolder(path, count + 1);
            }
        }

        /// <summary>
        /// Create the folders to represent a hex value
        /// </summary>
        /// <param name="path"></param>
        /// <param name="value"></param>
        private static void NumberToFoldersHex(string path, int value)
        {
            if (value == 0) return;

            string hexValue = Convert.ToInt32(value).ToString("X");
            for (int hexDigitCount = 0; hexDigitCount < hexValue.Length; hexDigitCount++)
            {
                DirectoryInfo hexDigitFldr = CreateFolder(path, hexDigitCount + 1);
                string bin = Convert.ToString(Convert.ToInt32(hexValue[hexDigitCount].ToString(), 16), 2).PadLeft(4, '0');
                for (int binDigitCount = 0; binDigitCount < bin.Length; binDigitCount++)
                {
                    DirectoryInfo bitPath = CreateFolder(hexDigitFldr.FullName, binDigitCount + 1);
                    if (bin[binDigitCount] == '1') CreateFolder(bitPath.FullName, 0);
                }
                Console.WriteLine(bin);
            }
        }
    }
}
