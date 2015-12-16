using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rottytooth.Esolang.Folders
{
    public class SyntaxError : Exception
    {
        public SyntaxError(string message) : base(message) { }
    }
}
