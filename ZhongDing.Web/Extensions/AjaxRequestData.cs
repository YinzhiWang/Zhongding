using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZhongDing.Common.Enums;

namespace ZhongDing.Web.Extensions
{
    public class AjaxRequestData
    {
        /// <summary>
        /// Gets or sets the type of the ajax action.
        /// </summary>
        /// <value>The type of the ajax action.</value>
        public EAjaxActionType AjaxActionType { get; set; }

        /// <summary>
        /// Gets or sets the uploaded file.
        /// </summary>
        /// <value>The uploaded file.</value>
        public AsyncUploadedFile UploadedFile { get; set; }
    }

}