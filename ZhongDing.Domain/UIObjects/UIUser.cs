using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIUser : UIBase
    {
        public Guid? AspnetUserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string DepartmentName { get; set; }
        public string Position { get; set; }
        public string MobilePhone { get; set; }
        public DateTime? EnrollDate { get; set; }
    }
}
