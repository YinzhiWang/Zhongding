﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchWorkflowBase : UISearchBase
    {
        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int WorkflowStatusID { get; set; }

        public IEnumerable<int> IncludeWorkflowStatusIDs { get; set; }
    }
}