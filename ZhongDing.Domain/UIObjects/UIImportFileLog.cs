using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIImportFileLog : UIBase
    {
        public DateTime SettlementDate { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public DateTime? ImportBeginDate { get; set; }

        public DateTime? ImportEndDate { get; set; }

        public int ImportStatusID { get; set; }

        public string ImportStatus { get; set; }

        public bool IsCompleted { get; set; }
    }
}
