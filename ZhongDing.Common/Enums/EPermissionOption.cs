using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    [Flags]
    public enum EPermissionOption
    {
        /// <summary>
        /// None 0
        /// </summary>
        None = 0,
        /// <summary>
        /// Create 1
        /// </summary>
        Create = 1,
        /// <summary>
        /// Edit 2
        /// </summary>
        Edit = 2,
        /// <summary>
        /// Delete 4
        /// </summary>
        Delete = 4,
        /// <summary>
        /// View 8
        /// </summary>
        View = 8,
        /// <summary>
        /// Print 16
        /// </summary>
        Print = 16,
        /// <summary>
        /// Export 32
        /// </summary>
        Export = 32,
    }
}
