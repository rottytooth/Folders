using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Rottytooth.Esolang.Folders.Runtime
{
    public class VarManager
    {
        public DirectoryInfo VarDir;

        private const int MAX_FOLDER_SIZE = 98;

        public VarManager(string programName)
        {
            DirectoryInfo folders =
                new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                  @"\Folders Programs");
            if (!folders.Exists)
                folders.Create();

            this.VarDir = new DirectoryInfo(folders.FullName + @"\" + SpecialSymbols.Encode(programName));

            if (!VarDir.Exists)
                VarDir.Create();
        }

        public object GetVariable(string variableName)
        {
            // return value of variable from folder and return as correct type (also from folder)
            DirectoryInfo variableFolder = new DirectoryInfo(VarDir.FullName + @"\" + SpecialSymbols.Encode(variableName));

            DirectoryInfo[] subdirs = variableFolder.GetDirectories();

            Type type = Type.GetType(SpecialSymbols.Decode(subdirs[0].Name.Substring(2)));

            StringBuilder returnValue = new StringBuilder();
            for (int i = 1; i < subdirs.Length; i++)
            {
                returnValue.Append(SpecialSymbols.Decode(subdirs[i].Name.Substring(2)));
            }

            return Convert.ChangeType(returnValue.ToString(), type);
        }

        public void SetVariable(string variableName, object value)
        {
            // the folder to hold variable info (type and value)
            DirectoryInfo variableFolder = new DirectoryInfo(VarDir.FullName + @"\" + SpecialSymbols.Encode(variableName));

            if (!variableFolder.Exists)
                variableFolder.Create();

            foreach (DirectoryInfo subdir in variableFolder.GetDirectories())
            {
                subdir.Delete();
            }

            // variable type (first subdirectory -- we will label with 1 to be sure)
            variableFolder.CreateSubdirectory("1 " + SpecialSymbols.Encode(value.GetType().ToString()));

            // if it's a string more than our max folder size, we'll break it down into sections
            if (value.ToString().Length > MAX_FOLDER_SIZE)
            {
                int folderNum = 2;

                foreach (string substring in SplitByLength(value.ToString(), MAX_FOLDER_SIZE))
                {
                    variableFolder.CreateSubdirectory(folderNum.ToString() + " " + SpecialSymbols.Encode(substring));
                    folderNum++;
                }
            }
            else
            {
                variableFolder.CreateSubdirectory("2 " + SpecialSymbols.Encode(value.ToString()));
            }
        }

        public static IEnumerable<string> SplitByLength(string str, int maxLength)
        {
            int index = 0;
            while (true)
            {
                if (index + maxLength >= str.Length)
                {
                    yield return str.Substring(index);
                    yield break;
                }
                yield return str.Substring(index, maxLength);
                index += maxLength;
            }
        }
    }
}
