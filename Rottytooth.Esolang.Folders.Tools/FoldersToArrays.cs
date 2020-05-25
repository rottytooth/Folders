using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rottytooth.Esolang.Folders;

namespace Rottytooth.Esolang.Folders.Tools
{
    public class FoldersToArrays
    {
        public static string Build(string path, string outpath = "")
        {
            StringBuilder sb = new StringBuilder();
            Translate(sb, path);

            if (!string.IsNullOrEmpty(outpath))
            {
                File.WriteAllText(outpath, sb.ToString());
            }

            return sb.ToString();
        }

        public static StringBuilder Translate(StringBuilder program, string path)
        {
            program.Append("[");
            DirectoryInfo di = new DirectoryInfo(path.ToString());
            DirectoryInfo[] subdirs = di.GetDirectories().CustomSort().ToArray();
            for(int i = 0; i < subdirs.Length; i++)
            {
                if (i > 0) program.Append(",");
                Translate(program, subdirs[i].FullName);
            }
            program.Append("]");
            return program;
        }
    }
}
