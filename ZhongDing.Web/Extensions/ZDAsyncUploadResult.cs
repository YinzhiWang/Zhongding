using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Web.UI;

namespace ZhongDing.Web.Extensions
{
    public class ZDAsyncUploadResult : AsyncUploadResult
    {
        /// <summary>
        /// 原文件名(不含扩展名)
        /// </summary>
        public string FileNameWithoutExtension { get; set; }

        private string filePath;
        /// <summary>
        /// 文件保存的路径
        /// </summary>
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
    }
}