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
            DateTime? lastTransportFeeStockOutSmsReminderDate = null;
            var query = from transportFeeStockOut in DB.TransportFeeStockOut
                        join stockOut in DB.StockOut on transportFeeStockOut.StockOutID equals stockOut.ID
                        join transportFeeStockOutSmsReminder in DB.TransportFeeStockOutSmsReminder on transportFeeStockOut.TransportFeeStockOutSmsReminderID equals transportFeeStockOutSmsReminder.ID into tempTransportFeeStockOutSmsReminderList
                        from tempTransportFeeStockOutSmsReminder in tempTransportFeeStockOutSmsReminderList.DefaultIfEmpty()
                        where transportFeeStockOut.TransportFeeID == transportFeeID && transportFeeStockOut.IsDeleted == false
                        select new UITransportFeeStockOut()
                        {
                            ID = transportFeeStockOut.ID,
                            Code = stockOut.Code,
                            ReceiverName = stockOut.ReceiverName,
                            ReceiverPhone = stockOut.ReceiverPhone,
                            ReceiverAddress = stockOut.ReceiverAddress,
                            LastTransportFeeStockOutSmsReminderDate = tempTransportFeeStockOutSmsReminder == null ? lastTransportFeeStockOutSmsReminderDate : tempTransportFeeStockOutSmsReminder.CreatedOn

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


        public IList<UITransportFeeStockOut> GetUIListForSaleAppStockOut(int stockOutID)
        {
            DateTime? lastTransportFeeStockOutSmsReminderDate = null;
            var query = from transportFeeStockOut in DB.TransportFeeStockOut
                        join transportFee in DB.TransportFee on transportFeeStockOut.TransportFeeID equals transportFee.ID
                        join transportCompany in DB.TransportCompany on transportFee.TransportCompanyID equals transportCompany.ID
                        join transportFeeStockOutSmsReminder in DB.TransportFeeStockOutSmsReminder on transportFeeStockOut.TransportFeeStockOutSmsReminderID equals transportFeeStockOutSmsReminder.ID into tempTransportFeeStockOutSmsReminderList
                        from tempTransportFeeStockOutSmsReminder in tempTransportFeeStockOutSmsReminderList.DefaultIfEmpty()
                        where transportFeeStockOut.IsDeleted == false && transportFee.IsDeleted == false && transportFeeStockOut.StockOutID == stockOutID
                        select new UITransportFeeStockOut()
                        {
                            ID = transportFeeStockOut.ID,
                            Fee = transportFee.Fee,
                            Remark = transportFee.Remark,
                            SendDate = transportFee.SendDate,
                            TransportCompanyNumber = transportFee.TransportCompanyNumber,
                            TransportCompanyName = transportCompany.CompanyName,
                            LastTransportFeeStockOutSmsReminderDate = tempTransportFeeStockOutSmsReminder == null ? lastTransportFeeStockOutSmsReminderDate : tempTransportFeeStockOutSmsReminder.CreatedOn
                        };
            return query.ToList();
        }
    }
}
