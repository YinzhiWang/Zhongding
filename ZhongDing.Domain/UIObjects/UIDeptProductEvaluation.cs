using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDeptProductEvaluation : UIBase
    {
        public int DepartmentID { get; set; }

        public string ProductName { get; set; }

        public double? InvestigateRatio { get; set; }

        public double? SalesRatio { get; set; }

        /// <summary>
        /// 小计比率
        /// </summary>
        public double? SubtotalRatio { get; set; }
    }
}
