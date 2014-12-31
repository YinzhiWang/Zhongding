using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    /// <summary>
    /// 工作流UI类基类
    /// </summary>
    public class UIWorkflowBase : UIBase
    {
        /// <summary>
        /// 工作流ID
        /// </summary>
        /// <value>The workflow ID.</value>
        public int WorkflowID { get; set; }

        /// <summary>
        /// 工作流名称
        /// </summary>
        /// <value>The name of the workfolw.</value>
        public string WorkfolwName { get; set; }

        /// <summary>
        /// 工作流状态ID
        /// </summary>
        public int WorkflowStatusID { get; set; }

        /// <summary>
        /// 工作流状态
        /// </summary>
        public string WorkflowStatus { get; set; }
    }
}
