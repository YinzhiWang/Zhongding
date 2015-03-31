using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchImportFileLog : UISearchBase
    {
        public int ImportDataTypeID { get; set; }

        public int ImportStatusID { get; set; }

        public string FileName { get; set; }

        public DateTime? SettlementDate { get; set; }

    }
}
