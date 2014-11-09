using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZhongDing.Web.Extensions
{
    public class AjaxResponseData
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is success.
        /// </summary>
        /// <value><c>true</c> if this instance is success; otherwise, <c>false</c>.</value>
        public bool IsSuccess { set; get; }
        /// <summary>
        /// Gets or sets the return data.
        /// </summary>
        /// <value>The return data.</value>
        public string ReturnData { set; get; }
    }
}