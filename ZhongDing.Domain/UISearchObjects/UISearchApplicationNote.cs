using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchApplicationNote : UISearchBase
    {
        public int ApplicationID { get; set; }

        public int NoteTypeID { get; set; }

        public int WorkflowID { get; set; }

        public int WorkflowStepID { get; set; }
    }
}
