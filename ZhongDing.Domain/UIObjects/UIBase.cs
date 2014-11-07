using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    /// <summary>
    /// 类：基础UI对象
    /// </summary>
    public class UIBase
    {
        /// <summary>
        /// 实体对象ID
        /// </summary>
        /// <value>The ID.</value>
        public int ID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value>The created on.</value>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        /// <value>The created by.</value>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 最后更改时间
        /// </summary>
        /// <value>The last modified on.</value>
        public DateTime? LastModifiedOn { get; set; }

        /// <summary>
        /// 最后更改人
        /// </summary>
        /// <value>The last modified by.</value>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <value>The comments.</value>
        public string Comment { get; set; }

    }
}
