using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIUserGroupPermission : UIBase
    {
        public int UserGroupPermissionID { get; set; }

        public int Value { get; set; }

        public string Name { get; set; }


        public bool HasCreate { get; set; }
        public bool HasEdit { get; set; }
        public bool HasDelete { get; set; }
        public bool HasView { get; set; }
        public bool HasPrint { get; set; }
        public bool HasExport { get; set; }


        public bool HasPermissionCreate { get; set; }
        public bool HasPermissionEdit { get; set; }
        public bool HasPermissionDelete { get; set; }
        public bool HasPermissionView { get; set; }
        public bool HasPermissionPrint { get; set; }
        public bool HasPermissionExport { get; set; }

        public int PermissionID { get; set; }
    }
}
