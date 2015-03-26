using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface ITransportFeeStockOutRepository : IBaseRepository<TransportFeeStockOut>
    {
        IList<UITransportFeeStockOut> GetTransportFeeStockOutsByTransportFeeID(int transportFeeID, out int totalRecords);

        bool ExistByTransportFeeIdAndStockOutID(int transportFeeId, int stockOutID);
    }
}
