using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface ICertificateTypeRepository : IBaseRepository<CertificateType>
    {
        /// <summary>
        /// 获取证照类型列表
        /// </summary>
        /// <param name="ownerTypeID">The owner type ID.</param>
        /// <returns>IList{UICertificateType}.</returns>
        IList<UICertificateType> GetUIList(int? ownerTypeID);
    }
}
