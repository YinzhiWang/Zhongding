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
                                    CreatedByUserName = user.FullName
                                }).ToList();
            }

            totalRecords = total;
            foreach (var item in uiWarehouses)
            {
                item.TransportFeeTypeText = item.TransportFeeType == (int)ETransportFeeType.StockIn ? "入库" : "出库";
            }
            return uiWarehouses;
        }








        public IList<UITransportFee> GetUIListReimbursementDetail(Domain.UISearchObjects.UISearchTransportFee uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UITransportFee> uiEntitys = new List<UITransportFee>();
            int total = 0;


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

            int sourceNotUse = 2, sourceUse = 1;//未使用的
            var queryStockIn = from transportFee in DB.TransportFee
                               join transportFeeStockIn in DB.TransportFeeStockIn on transportFee.ID equals transportFeeStockIn.TransportFeeID
                               join stockIn in DB.StockIn on transportFeeStockIn.StockInID equals stockIn.ID
                               where transportFee.IsDeleted == false
                               && transportFeeStockIn.IsDeleted == false
                               && stockIn.CreatedBy == uiSearchObj.UserID
                               && !DB.ReimbursementDetailTransportFee.Where(x => x.IsDeleted == false && x.TransportFeeID == transportFee.ID).Any()
                               group transportFee by transportFee.ID into transportFeeGroup
                               select new
                               {
                                   TransportFeeID = transportFeeGroup.Key,
                                   ReimbursementDetailTransportFeeID = 0,
                                   ReimbursementDetailID = 0,
                                   Comment = string.Empty
                               };

            var queryStockOut = from transportFee in DB.TransportFee
                                join transportFeeStockOut in DB.TransportFeeStockOut on transportFee.ID equals transportFeeStockOut.TransportFeeID
                                join stockOut in DB.StockOut on transportFeeStockOut.StockOutID equals stockOut.ID
                                where transportFee.IsDeleted == false
                                && transportFeeStockOut.IsDeleted == false
                                && stockOut.CreatedBy == uiSearchObj.UserID
                                && !DB.ReimbursementDetailTransportFee.Where(x => x.IsDeleted == false && x.TransportFeeID == transportFee.ID).Any()
                                group transportFee by transportFee.ID into transportFeeGroup
                                select new
                                {
                                    TransportFeeID = transportFeeGroup.Key,
                                    ReimbursementDetailTransportFeeID = 0,
                                    ReimbursementDetailID = 0,
                                    Comment = string.Empty
                                };



            var queryAlreadyUsed = from reimbursementDetailTransportFee in DB.ReimbursementDetailTransportFee
                                   where reimbursementDetailTransportFee.ReimbursementDetailID == uiSearchObj.ReimbursementDetailID
                                   && reimbursementDetailTransportFee.IsDeleted == false
                                   select new
                                   {
                                       TransportFeeID = reimbursementDetailTransportFee.TransportFeeID,
                                       ReimbursementDetailTransportFeeID = reimbursementDetailTransportFee.ID,
                                       ReimbursementDetailID = reimbursementDetailTransportFee.ReimbursementDetailID,
                                       Comment = reimbursementDetailTransportFee.Comment
                                   };

            var queryStockInAndStockOut = queryStockIn.Union(queryStockOut).Union(queryAlreadyUsed);


            var queryResult = from q in queryStockInAndStockOut
                              join transportFee in DB.TransportFee on q.TransportFeeID equals transportFee.ID
                              orderby q.ReimbursementDetailTransportFeeID descending
                              select new UITransportFee()
                              {
                                  ID = transportFee.ID,
                                  Fee = transportFee.Fee,
                                  Remark = transportFee.Remark,
                                  SendDate = transportFee.SendDate,
                                  StartPlace = transportFee.StartPlace,
                                  StartPlaceTelephone = transportFee.StartPlaceTelephone,
                                  TransportCompanyNumber = transportFee.TransportCompanyNumber,
                                  TransportFeeType = transportFee.TransportFeeType,
                                  ReimbursementDetailTransportFeeID = q.ReimbursementDetailTransportFeeID,
                                  Comment = q.Comment,
                                  ReimbursementDetailID = q.ReimbursementDetailID
                              };

            totalRecords = total;


            uiEntitys = queryResult.OrderByDescending(x => x.ID)
              .Skip(pageSize * pageIndex).Take(pageSize).ToList();

            foreach (var item in uiEntitys)
            {
                item.TransportFeeTypeText = item.TransportFeeType == (int)ETransportFeeType.StockIn ? "入库" : "出库";
            }
            return uiEntitys;
        }
    }
}
