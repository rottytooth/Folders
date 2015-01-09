using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rottytooth.Esolang.Folders.Runtime
{
    public static class SpecialSymbols
    {
        public const string VAR_STORAGE_NAME = "Local";


        public static readonly Dictionary<string, string> AltCommandNames =
            new Dictionary<string, string>()
            {
                {"New folder", "if"},
                {"Temp", "while"},
                {"Images", "declare"},
                {"Downloads", "let"},
                {"Setup", "print"},
                {"Logs", "input"},

                {"Vaction photos", "int"},
                {"Lang", "float"},
                {"Img", "string"},
                {"User", "double"}
            };

        private static readonly Dictionary<string, string> SymbolList =
            new Dictionary<string, string>()
            {
                {"%2F;", "/"},
                {"%5C;", "\\"},
                {"%2A;", "*"},
                {"&lt;", "<"},
                {"&gt;", ">"},
                {"%20;", " "},
                {"&lf;", "\\n"},
                {"%2E;", "."}
            };

        /// <summary>
        /// Decode ascii-encoded symbols for those that can't be stored in directory name
        /// </summary>
        /// <param name="foldername"></param>
        /// <returns></returns>
        public static string Decode(string foldername)
        {
            return SymbolList.Aggregate(foldername, (current, entry) => current.Replace(entry.Key, entry.Value));
        }

        public static string Encode(string foldername)
        {
            return SymbolList.Aggregate(foldername, (current, entry) => current.Replace(entry.Value, entry.Key));
        }
    }
}
