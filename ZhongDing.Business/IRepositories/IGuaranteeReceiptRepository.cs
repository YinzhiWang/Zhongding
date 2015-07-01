using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface IGuaranteeReceiptRepository : IBaseRepository<GuaranteeReceipt>
    {
        IList<UIGuaranteeReceipt> GetUIList(Domain.UISearchObjects.UISearchGuaranteeReceipt uiSearchObj, int p1, int p2, out int totalRecords);
    }
}
