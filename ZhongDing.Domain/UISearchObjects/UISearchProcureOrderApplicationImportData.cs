using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchProcureOrderApplicationImportData : UISearchBase
    {
        public int ImportFileLogID { get; set; }

        /// <summary>
        /// 是否排除被覆盖的数据
        /// </summary>
        public bool ExcludeOverwritten { get; set; }
    }
}
