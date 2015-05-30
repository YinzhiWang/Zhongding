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
    public class TransportFeeStockInRepository : BaseRepository<TransportFeeStockIn>, ITransportFeeStockInRepository
    {
        public IList<UITransportFeeStockIn> GetTransportFeeStockInsByTransportFeeID(int transportFeeID, out int totalRecords)
        {
            int total = 0;

            var query = from transportFeeStockIn in DB.TransportFeeStockIn
                        join stockIn in DB.StockIn on transportFeeStockIn.StockInID equals stockIn.ID
                        join user in DB.Users on stockIn.CreatedBy equals user.UserID
                        where transportFeeStockIn.TransportFeeID == transportFeeID && transportFeeStockIn.IsDeleted == false
                        select new UITransportFeeStockIn()
                        {
                            ID = transportFeeStockIn.ID,
                            Code = stockIn.Code,
                            EntryDate = stockIn.EntryDate,
                            CreatedByText = user.FullName
                        };
            total = query.Count();

            totalRecords = total;

            return query.ToList();
        }


        public bool ExistByTransportFeeIdAndStockInID(int transportFeeId, int stockInID)
        {
            bool exist = DB.TransportFeeStockIn.Where(x => x.TransportFeeID == transportFeeId && x.StockInID == stockInID && x.IsDeleted == false).Any();
            return exist;
        }
    }
}
