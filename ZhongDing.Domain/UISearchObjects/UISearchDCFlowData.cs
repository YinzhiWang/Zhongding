using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchDCFlowData : UISearchBase
    {
        public int DistributionCompanyID { get; set; }

        public int ProductID { get; set; }

        public int ImportFileLogID { get; set; }

        /// <summary>
        /// 是否排除被覆盖的数据
        /// </summary>
        public bool ExcludeOverwritten { get; set; }
    }
}
