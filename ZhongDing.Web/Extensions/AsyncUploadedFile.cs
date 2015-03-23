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
        /// 导入数据类型ID
        /// </summary>
        public int ImportDataTypeID { get; set; }

        /// <summary>
        /// 当前文件实体ID
        /// </summary>
        public int CurrentEntityID { get; set; }

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