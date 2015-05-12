using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDBClientQuarterlyAssessmentReport
    {
        public string ClientUserName { get; set; }

        public string HospitalType { get; set; }

        public string ProductName { get; set; }

        public string Specification { get; set; }

        public string HospitalName { get; set; }

        public decimal? PromotionExpense { get; set; }

        public int QuarterTaskAssignment { get; set; }

        public int FirstMonthSalesQty { get; set; }

        public int SecondMonthSalesQty { get; set; }

        public int ThirdMonthSalesQty { get; set; }

        public decimal QuarterAmount { get; set; }

        public decimal RewardRate { get; set; }

        public decimal RewardAmount { get; set; }
    }
}
