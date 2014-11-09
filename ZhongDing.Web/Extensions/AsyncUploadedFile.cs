using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZhongDing.Web.Extensions
{
    /// <summary>
    /// 类：异步上传的文件
    /// </summary>
    public class AsyncUploadedFile
    {
        /// <summary>
        /// 所有者类型ID
        /// </summary>
        /// <value>The owner type ID.</value>
        public int OwnerTypeID { get; set; }

        /// <summary>
        /// 供应商合同文件ID
        /// </summary>
        /// <value>The supplier contract file ID.</value>
        public int SupplierContractFileID { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath { get; set; }
    }
}