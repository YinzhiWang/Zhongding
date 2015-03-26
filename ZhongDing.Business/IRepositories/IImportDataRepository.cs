using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;

namespace ZhongDing.Business.IRepositories
{
    /// <summary>
    /// 导入数据
    /// </summary>
    public interface IImportDataRepository
    {
        /// <summary>
        /// 导入数据（win service导入）
        /// </summary>
        void ImportData();
    }
}
