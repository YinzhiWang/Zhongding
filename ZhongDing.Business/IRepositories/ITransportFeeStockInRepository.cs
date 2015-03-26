using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface ITransportFeeStockInRepository : IBaseRepository<TransportFeeStockIn>
    {
        IList<UITransportFeeStockIn> GetTransportFeeStockInsByTransportFeeID(int transportFeeID, out int totalRecords);

        bool ExistByTransportFeeIdAndStockInID(int transportFeeId, int stockInID);
    }
}
