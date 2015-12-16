using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rottytooth.Esolang.Folders
{
    public enum ExpressionEnum
    {
        Variable,

        Add,

        Subtract,

        Multiply,

        Divide,

        LiteralValue,
        
        /// <summary>
        /// Returns true based on expressions in first subdir and second subdir
        /// </summary>
        EqualTo,

        /// <summary>
        /// Returns true based on expressions in first subdir and second subdir
        /// </summary>
        GreaterThan,

        /// <summary>
        /// Returns true based on expressions in first subdir and second subdir
        /// </summary>
        LessThan
    }
}
