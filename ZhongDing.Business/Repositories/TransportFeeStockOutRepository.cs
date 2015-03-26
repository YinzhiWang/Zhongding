using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.Repositories
{
    public class TransportFeeStockOutRepository : BaseRepository<TransportFeeStockOut>, ITransportFeeStockOutRepository
    {
        public IList<Domain.UIObjects.UITransportFeeStockOut> GetTransportFeeStockOutsByTransportFeeID(int transportFeeID, out int totalRecords)
        {
            int total = 0;

            var query = from transportFeeStockOut in DB.TransportFeeStockOut
                        join stockOut in DB.StockOut on transportFeeStockOut.StockOutID equals stockOut.ID
                        where transportFeeStockOut.TransportFeeID == transportFeeID && transportFeeStockOut.IsDeleted == false
                        select new UITransportFeeStockOut()
                        {
                            ID = transportFeeStockOut.ID,
                            Code = stockOut.Code,
                            ReceiverName = stockOut.ReceiverName,
                            ReceiverPhone = stockOut.ReceiverPhone,
                            ReceiverAddress = stockOut.ReceiverAddress,

                        };
            total = query.Count();
            totalRecords = total;
            return query.ToList();
        }

        public bool ExistByTransportFeeIdAndStockOutID(int transportFeeId, int stockOutID)
        {
            bool exist = DB.TransportFeeStockOut.Where(x => x.TransportFeeID == transportFeeId && x.StockOutID == stockOutID && x.IsDeleted == false).Any();
            return exist;
        }
    }
}
