using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Common.Extension;

namespace ZhongDing.Business.Repositories
{
    public class TransportFeeRepository : BaseRepository<TransportFee>, ITransportFeeRepository
    {
        public IList<Domain.UIObjects.UITransportFee> GetUIList(Domain.UISearchObjects.UISearchTransportFee uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UITransportFee> uiWarehouses = new List<UITransportFee>();
            int total = 0;

            IQueryable<TransportFee> query = null;

            List<Expression<Func<TransportFee, bool>>> whereFuncs = new List<Expression<Func<TransportFee, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.TransportFeeType > -1)
                    whereFuncs.Add(x => x.TransportFeeType == uiSearchObj.TransportFeeType);
                if (uiSearchObj.TransportCompanyNumber.IsNotNullOrEmpty())
                {
                    whereFuncs.Add(x => x.TransportCompanyNumber.Contains(uiSearchObj.TransportCompanyNumber));
                }
                if (uiSearchObj.TransportCompanyName.IsNotNullOrEmpty())
                {
                    whereFuncs.Add(x => x.TransportCompany.CompanyName.Contains(uiSearchObj.TransportCompanyName));
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiWarehouses = (from q in query
                                join transportCompany in DB.TransportCompany on q.TransportCompanyID equals transportCompany.ID
                                join user in DB.Users on q.CreatedBy equals user.UserID
                                select new UITransportFee()
                                {
                                    ID = q.ID,
                                    Driver = q.Driver,
                                    DriverTelephone = q.DriverTelephone,
                                    EndPlace = q.EndPlace,
                                    EndPlaceTelephone = q.EndPlaceTelephone,
                                    Fee = q.Fee,
                                    Remark = q.Remark,
                                    SendDate = q.SendDate,
                                    StartPlace = q.StartPlace,
                                    StartPlaceTelephone = q.StartPlaceTelephone,
                                    TransportCompanyID = q.TransportCompanyID,
                                    TransportCompanyNumber = q.TransportCompanyNumber,
                                    TransportCompanyName = transportCompany.CompanyName,
                                    TransportFeeType = q.TransportFeeType,
                                    CreatedByUserName = user.UserName
                                }).ToList();
            }

            totalRecords = total;
            foreach (var item in uiWarehouses)
            {
                item.TransportFeeTypeText = item.TransportFeeType == (int)ETransportFeeType.StockIn ? "入库" : "出库";
            }
            return uiWarehouses;
        }






        public IList<UITransportFee> GetUIListForSaleAppStockOut(int stockOutID)
        {
            var query = from transportFeeStockOut in DB.TransportFeeStockOut
                        join transportFee in DB.TransportFee on transportFeeStockOut.TransportFeeID equals transportFee.ID
                        join transportCompany in DB.TransportCompany on transportFee.TransportCompanyID equals transportCompany.ID
                        where transportFeeStockOut.IsDeleted == false && transportFee.IsDeleted == false
                        select new UITransportFee()
                        {
                            ID = transportFee.ID,
                            Fee = transportFee.Fee,
                            Remark = transportFee.Remark,
                            SendDate = transportFee.SendDate,
                            TransportCompanyNumber = transportFee.TransportCompanyNumber,
                            TransportCompanyName = transportCompany.CompanyName,
                        };
            return query.ToList();
        }
    }
}
