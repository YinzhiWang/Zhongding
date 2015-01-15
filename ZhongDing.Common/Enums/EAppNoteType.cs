using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Enums
{
    /// <summary>
    /// 枚举：the note type of application
    /// </summary>
    public enum EAppNoteType : int
    {
        /// <summary>
        /// 单据备注
        /// </summary>
        Comment = 1,
        /// <summary>
        /// 单据审核意见
        /// </summary>
        AuditOpinion
    }
}
