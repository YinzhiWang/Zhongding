using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIImportErrorLog : UIBase
    {
        public int? ErrorRowIndex { get; set; }

        public string ErrorRowData { get; set; }

        /// <summary>
        /// 省略的错误信息
        /// </summary>
        public string AbbrErrorMsg { get; set; }

        public string ErrorMsg { get; set; }
    }
}
