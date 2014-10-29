using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Business.IRepositories
{
    /// <summary>
    /// 接口：需要自动编号
    /// </summary>
    public interface IAutoSerialNo
    {
        /// <summary>
        /// 获取实体的最大主键ID
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetMaxEntityID();
    }
}
