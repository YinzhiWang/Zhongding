using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchBase
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
        public int CreatedBy { get; set; }

        /// <summary>
        /// 最后更改时间
        /// </summary>
        /// <value>The last modified on.</value>
        public DateTime? LastModifiedOn { get; set; }

        /// <summary>
        /// 最后更改人
        /// </summary>
        /// <value>The last modified by.</value>
        public int? LastModifiedBy { get; set; }

        /// <summary>
        /// 是否需要分页
        /// </summary>
        /// <value><c>true</c> if this instance is need paging; otherwise, <c>false</c>.</value>
        public bool IsNeedPaging { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        /// <value>The index of the page.</value>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页条数
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize { get; set; }

    }
}
