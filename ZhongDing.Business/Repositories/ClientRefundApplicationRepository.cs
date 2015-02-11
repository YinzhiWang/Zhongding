using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class ClientRefundApplicationRepository : BaseRepository<ClientRefundApplication>, IClientRefundApplicationRepository
    {
        public IList<UIClientRefundApplication> GetUIList(UISearchClientRefundApplication uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIClientRefundApplication> uiEntities = new List<UIClientRefundApplication>();

            int total = 0;

            IQueryable<ClientRefundApplication> query = null;

            List<Expression<Func<ClientRefundApplication, bool>>> whereFuncs = new List<Expression<Func<ClientRefundApplication, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.WarehouseID > 0)
                    whereFuncs.Add(x => x.ClientRefundAppDetail.Any(y => y.WarehouseID == uiSearchObj.WarehouseID));

                if (uiSearchObj.ClientUserID > 0)
                    whereFuncs.Add(x => x.ClientUserID == uiSearchObj.ClientUserID);

                if (uiSearchObj.ClientCompanyID > 0)
                    whereFuncs.Add(x => x.ClientCompanyID == uiSearchObj.ClientCompanyID);

                if (uiSearchObj.WorkflowStatusID > 0)
                    whereFuncs.Add(x => x.WorkflowStatusID == uiSearchObj.WorkflowStatusID);

                if (uiSearchObj.IncludeWorkflowStatusIDs != null
                    && uiSearchObj.IncludeWorkflowStatusIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeWorkflowStatusIDs.Contains(x.WorkflowStatusID));

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.CreatedOn >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.CreatedOn < uiSearchObj.EndDate);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join csa in DB.ClientSaleApplication on q.ClientSaleAppID equals csa.ID
                              join soa in DB.SalesOrderApplication on csa.SalesOrderApplicationID equals soa.ID
                              join sot in DB.SaleOrderType on soa.SaleOrderTypeID equals sot.ID
                              join c in DB.Company on q.CompanyID equals c.ID
                              join cu in DB.ClientUser on q.ClientUserID equals cu.ID
                              join cc in DB.ClientCompany on q.ClientCompanyID equals cc.ID
                              join ws in DB.WorkflowStatus on q.WorkflowStatusID equals ws.ID
                              join cbu in DB.Users on q.CreatedBy equals cbu.UserID into tempCBU
                              from tcbu in tempCBU.DefaultIfEmpty()
                              select new UIClientRefundApplication()
                              {
                                  ID = q.ID,
                                  OrderCode = soa.OrderCode,
                                  ClientUserName = cu.ClientName,
                                  ClientCompanyName = cc.Name,
                                  CompanyName = c.CompanyName,
                                  RefundAmount = q.ClientRefundAppDetail.Sum(x => x.RefundAmount),
                                  WorkflowStatusID = q.WorkflowStatusID,
                                  WorkflowStatus = ws.StatusName,
                                  CreatedByUserID = q.CreatedBy,
                                  CreatedBy = tcbu == null ? string.Empty : tcbu.FullName,
                                  CreatedOn = q.CreatedOn
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }

        public IList<UIClientNeedRefundOrder> GetNeedRefundOrders(UISearchClientNeedRefundOrder uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIClientNeedRefundOrder> uiEntities = new List<UIClientNeedRefundOrder>();

            int total = 0;

            var query = from gQurey in
                            ((from soad in DB.SalesOrderAppDetail
                              join soa in DB.SalesOrderApplication.Where(x => x.IsDeleted == false
                                  && (x.SaleOrderTypeID == (int)ESaleOrderType.AttractBusinessMode
                                    || x.SaleOrderTypeID == (int)ESaleOrderType.AttachedMode))
                                    on soad.SalesOrderApplicationID equals soa.ID
                              join w in DB.Warehouse.Where(x => x.SaleTypeID == (int)ESaleType.HighPrice)
                                    on soad.WarehouseID equals w.ID
                              join ph in DB.ProductHighPrice on new { soad.ProductID, soad.ProductSpecificationID }
                                    equals new { ph.ProductID, ph.ProductSpecificationID } into tempPH
                              from tph in tempPH.DefaultIfEmpty()
                              where soad.IsDeleted == false
                              && (uiSearchObj.CompanyID <= 0
                                    || (uiSearchObj.CompanyID > 0 && soa.ClientSaleApplication.Any(y => y.CompanyID == uiSearchObj.CompanyID)))
                              && (uiSearchObj.WarehouseID <= 0
                                    || (uiSearchObj.WarehouseID > 0 && soad.WarehouseID == uiSearchObj.WarehouseID))
                              && (uiSearchObj.ClientUserID <= 0
                                    || (uiSearchObj.ClientUserID > 0 && soa.ClientSaleApplication.Any(y => y.ClientUserID == uiSearchObj.ClientUserID)))
                              && (uiSearchObj.ClientCompanyID <= 0
                                    || (uiSearchObj.ClientCompanyID > 0 && soa.ClientSaleApplication.Any(y => y.ClientCompanyID == uiSearchObj.ClientCompanyID)))
                              select new
                              {
                                  SalesOrderApplicationID = soad.SalesOrderApplicationID,
                                  RefundAmount = (soad.SalesPrice
                                                  - (soad.SalesPrice - (tph == null ? 0 : (tph.ActualSalePrice ?? 0M))) * ((tph == null ? 0 : (tph.ClientTaxRatio ?? 0M)))
                                                  - (tph.ActualSalePrice ?? 0M)) * soad.Count
                              })
                              .GroupBy(x => new { x.SalesOrderApplicationID })
                              .Select(g => new { Key = g.Key, TotalRefundAmount = g.Sum(x => x.RefundAmount) }))
                        join soa in DB.SalesOrderApplication on gQurey.Key.SalesOrderApplicationID equals soa.ID
                        join csa in DB.ClientSaleApplication.Where(x => !x.ClientRefundApplication.Any(y => y.IsDeleted == false))
                            on soa.ID equals csa.SalesOrderApplicationID
                        join c in DB.Company on csa.CompanyID equals c.ID
                        join cu in DB.ClientUser on csa.ClientUserID equals cu.ID
                        join cc in DB.ClientCompany on csa.ClientCompanyID equals cc.ID
                        select new UIClientNeedRefundOrder
                        {
                            ClientSaleAppID = csa.ID,
                            CompanyID = csa.CompanyID,
                            CompanyName = c.CompanyName,
                            OrderCode = soa.OrderCode,
                            ClientUserName = cu.ClientName,
                            ClientCompanyName = cc.Name,
                            IsGuaranteed = csa.IsGuaranteed,
                            IsReceiptedGuaranteeAmount = csa.GuaranteeLog.Any(x => x.IsDeleted == false && x.IsReceipted == true),
                            RefundAmount = gQurey.TotalRefundAmount,
                            CreatedOn = csa.CreatedOn
                        };


            total = query.Count();

            uiEntities = query.AsQueryable()
                .OrderByDescending(x => x.CreatedOn)
                .Skip(pageIndex * pageSize)
                .Take(pageSize).ToList();

            foreach (var uiEntity in uiEntities.Where(x => x.IsGuaranteed == true))
            {
                if (uiEntity.IsReceiptedGuaranteeAmount)
                    uiEntity.IconUrlOfGuarantee = GlobalConst.Icons.ICON_GUARANTEE_RECEIPTED;
                else
                    uiEntity.IconUrlOfGuarantee = GlobalConst.Icons.ICON_GUARANTEE_NOT_RECEIPTED;
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
