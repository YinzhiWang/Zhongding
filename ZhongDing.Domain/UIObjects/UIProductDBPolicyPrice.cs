using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIProductDBPolicyPrice : UIProductPrice
    {
        public decimal? BidPrice { get; set; }

        public double? FeeRatio { get; set; }

        public decimal? PreferredPrice { get; set; }

        public decimal? PolicyPrice { get; set; }

        /// <summary>
        /// 规格：每件数量
        /// </summary>
        public int? NumberInLargePackage { get; set; }

        /// <summary>
        /// 规格：基本单位
        /// </summary>
        public string UnitOfMeasurement { get; set; }

        /// <summary>
        /// 包装描述
        /// </summary>
        public string PackageDescription { get; set; }
    }
}
