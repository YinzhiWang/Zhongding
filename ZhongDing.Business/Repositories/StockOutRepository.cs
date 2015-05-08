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
    public class StockOutRepository : BaseRepository<StockOut>, IStockOutRepository
    {
        public IList<UIStockOut> GetUIList(UISearchStockOut uiSearchObj = null)
        {
            IList<UIStockOut> uiEntities = new List<UIStockOut>();

            IQueryable<StockOut> query = null;

            List<Expression<Func<StockOut, bool>>> whereFuncs = new List<Expression<Func<StockOut, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.ReceiverTypeID > 0)
                    whereFuncs.Add(x => x.ReceiverTypeID == uiSearchObj.ReceiverTypeID);

                if (uiSearchObj.ClientUserID > 0)
                    whereFuncs.Add(x => x.ClientUserID == uiSearchObj.ClientUserID);

                if (uiSearchObj.ClientCompanyID > 0)
                    whereFuncs.Add(x => x.ClientCompanyID == uiSearchObj.ClientCompanyID);

                if (uiSearchObj.WorkflowStatusID > 0)
                    whereFuncs.Add(x => x.WorkflowStatusID.Equals(uiSearchObj.WorkflowStatusID));

                if (uiSearchObj.IncludeWorkflowStatusIDs != null
                    && uiSearchObj.IncludeWorkflowStatusIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeWorkflowStatusIDs.Contains(x.WorkflowStatusID));

                if (uiSearchObj.DistributionCompanyID > 0)
                    whereFuncs.Add(x => x.DistributionCompanyID == uiSearchObj.DistributionCompanyID);

                if (!string.IsNullOrEmpty(uiSearchObj.Code))
                    whereFuncs.Add(x => x.Code.Contains(uiSearchObj.Code));

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.BillDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.BillDate < uiSearchObj.EndDate);
                }
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join ws in DB.WorkflowStatus on q.WorkflowStatusID equals ws.ID
                              join dc in DB.DistributionCompany on q.DistributionCompanyID equals dc.ID into tempDC
                              from tdc in tempDC.DefaultIfEmpty()
                              join cu in DB.ClientUser on q.ClientUserID equals cu.ID into tempCU
                              from tcu in tempCU.DefaultIfEmpty()
                              join cc in DB.ClientCompany on q.ClientUserID equals cc.ID into tempCC
                              from tcc in tempCC.DefaultIfEmpty()
                              join cb in DB.Users on q.CreatedBy equals cb.UserID into tempCB
                              from tcb in tempCB.DefaultIfEmpty()
                              orderby q.CreatedOn descending
                              select new UIStockOut()
                              {
                                  ID = q.ID,
                                  Code = q.Code,
                                  DistributionCompany = tdc == null ? string.Empty : tdc.Name,
                                  ClientUserName = tcu == null ? string.Empty : tcu.ClientName,
                                  ClientCompany = tcc == null ? string.Empty : tcc.Name,
                                  WorkflowStatusID = q.WorkflowStatusID,
                                  WorkflowStatus = ws.StatusName,
                                  ReceiverName = q.ReceiverName,
                                  ReceiverPhone = q.ReceiverPhone,
                                  ReceiverAddress = q.ReceiverAddress,
                                  CreatedByUserID = q.CreatedBy,
                                  CreatedBy = tcb == null ? string.Empty : tcb.FullName
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIStockOut> GetUIList(UISearchStockOut uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIStockOut> uiEntities = new List<UIStockOut>();

            int total = 0;

            IQueryable<StockOut> query = null;

            List<Expression<Func<StockOut, bool>>> whereFuncs = new List<Expression<Func<StockOut, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.ReceiverTypeID > 0)
                    whereFuncs.Add(x => x.ReceiverTypeID == uiSearchObj.ReceiverTypeID);

                if (uiSearchObj.ClientUserID > 0)
                    whereFuncs.Add(x => x.ClientUserID == uiSearchObj.ClientUserID);

                if (uiSearchObj.ClientCompanyID > 0)
                    whereFuncs.Add(x => x.ClientCompanyID == uiSearchObj.ClientCompanyID);

                if (uiSearchObj.WorkflowStatusID > 0)
                    whereFuncs.Add(x => x.WorkflowStatusID.Equals(uiSearchObj.WorkflowStatusID));

                if (uiSearchObj.IncludeWorkflowStatusIDs != null
                    && uiSearchObj.IncludeWorkflowStatusIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeWorkflowStatusIDs.Contains(x.WorkflowStatusID));

                if (uiSearchObj.DistributionCompanyID > 0)
                    whereFuncs.Add(x => x.DistributionCompanyID == uiSearchObj.DistributionCompanyID);

                if (!string.IsNullOrEmpty(uiSearchObj.Code))
                    whereFuncs.Add(x => x.Code.Contains(uiSearchObj.Code));

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.BillDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.BillDate < uiSearchObj.EndDate);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);
            DateTime? lastStockOutSmsReminderDate = null;
            if (query != null)
            {
                uiEntities = (from q in query
                              join ws in DB.WorkflowStatus on q.WorkflowStatusID equals ws.ID
                              join dc in DB.DistributionCompany on q.DistributionCompanyID equals dc.ID into tempDC
                              from tdc in tempDC.DefaultIfEmpty()
                              join cu in DB.ClientUser on q.ClientUserID equals cu.ID into tempCU
                              from tcu in tempCU.DefaultIfEmpty()
                              join cc in DB.ClientCompany on q.ClientUserID equals cc.ID into tempCC
                              from tcc in tempCC.DefaultIfEmpty()
                              join cb in DB.Users on q.CreatedBy equals cb.UserID into tempCB
                              from tcb in tempCB.DefaultIfEmpty()

                              select new UIStockOut()
                              {
                                  ID = q.ID,
                                  Code = q.Code,
                                  DistributionCompany = tdc == null ? string.Empty : tdc.Name,
                                  ClientUserName = tcu == null ? string.Empty : tcu.ClientName,
                                  ClientCompany = tcc == null ? string.Empty : tcc.Name,
                                  WorkflowStatusID = q.WorkflowStatusID,
                                  WorkflowStatus = ws.StatusName,
                                  ReceiverName = q.ReceiverName,
                                  ReceiverPhone = q.ReceiverPhone,
                                  ReceiverAddress = q.ReceiverAddress,
                                  CreatedByUserID = q.CreatedBy,
                                  CreatedBy = tcb == null ? string.Empty : tcb.FullName,

                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
        public IList<UIStockOut> GetUIListForTransportFee(UISearchStockOut uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIStockOut> uiEntities = new List<UIStockOut>();

            int total = 0;

            IQueryable<StockOut> query = null;

            List<Expression<Func<StockOut, bool>>> whereFuncs = new List<Expression<Func<StockOut, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.ReceiverTypeID > 0)
                    whereFuncs.Add(x => x.ReceiverTypeID == uiSearchObj.ReceiverTypeID);

                if (uiSearchObj.ClientUserID > 0)
                    whereFuncs.Add(x => x.ClientUserID == uiSearchObj.ClientUserID);

                if (uiSearchObj.ClientCompanyID > 0)
                    whereFuncs.Add(x => x.ClientCompanyID == uiSearchObj.ClientCompanyID);

                if (uiSearchObj.WorkflowStatusID > 0)
                    whereFuncs.Add(x => x.WorkflowStatusID.Equals(uiSearchObj.WorkflowStatusID));

                if (uiSearchObj.IncludeWorkflowStatusIDs != null
                    && uiSearchObj.IncludeWorkflowStatusIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeWorkflowStatusIDs.Contains(x.WorkflowStatusID));

                if (uiSearchObj.DistributionCompanyID > 0)
                    whereFuncs.Add(x => x.DistributionCompanyID == uiSearchObj.DistributionCompanyID);

                if (!string.IsNullOrEmpty(uiSearchObj.Code))
                    whereFuncs.Add(x => x.Code.Contains(uiSearchObj.Code));

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.BillDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.BillDate < uiSearchObj.EndDate);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);
            DateTime? lastStockOutSmsReminderDate = null;
            if (query != null)
            {
                uiEntities = (from q in query
                              join ws in DB.WorkflowStatus on q.WorkflowStatusID equals ws.ID
                              join dc in DB.DistributionCompany on q.DistributionCompanyID equals dc.ID into tempDC
                              from tdc in tempDC.DefaultIfEmpty()
                              join cu in DB.ClientUser on q.ClientUserID equals cu.ID into tempCU
                              from tcu in tempCU.DefaultIfEmpty()
                              join cc in DB.ClientCompany on q.ClientUserID equals cc.ID into tempCC
                              from tcc in tempCC.DefaultIfEmpty()
                              join cb in DB.Users on q.CreatedBy equals cb.UserID into tempCB
                              from tcb in tempCB.DefaultIfEmpty()
                              join transportFeeStockOut in DB.TransportFeeStockOut.Where(x => x.IsDeleted == false) on q.ID equals transportFeeStockOut.StockOutID into tempTransportFeeStockOuts
                              from tempTransportFeeStockOut in tempTransportFeeStockOuts.DefaultIfEmpty()
                              where tempTransportFeeStockOut == null && q.WorkflowStatusID == (int)EWorkflowStatus.OutWarehouse
                              select new UIStockOut()
                              {
                                  ID = q.ID,
                                  Code = q.Code,
                                  DistributionCompany = tdc == null ? string.Empty : tdc.Name,
                                  ClientUserName = tcu == null ? string.Empty : tcu.ClientName,
                                  ClientCompany = tcc == null ? string.Empty : tcc.Name,
                                  WorkflowStatusID = q.WorkflowStatusID,
                                  WorkflowStatus = ws.StatusName,
                                  ReceiverName = q.ReceiverName,
                                  ReceiverPhone = q.ReceiverPhone,
                                  ReceiverAddress = q.ReceiverAddress,
                                  CreatedByUserID = q.CreatedBy,
                                  CreatedBy = tcb == null ? string.Empty : tcb.FullName,

                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }

        public int? GetMaxEntityID()
        {
            if (this.DB.StockOut.Count() > 0)
                return this.DB.StockOut.Max(x => x.ID);
            else return null;
        }

        public int? GetStockOutQty(UISearchStockOut uiSearchObj)
        {
            var query = (from sod in DB.StockOutDetail
                         join so in DB.StockOut on sod.StockOutID equals so.ID
                         where so.IsDeleted == false && so.ReceiverTypeID == (int)EReceiverType.ClientUser
                         && so.CompanyID == uiSearchObj.CompanyID
                         && so.ClientUserID == uiSearchObj.ClientUserID
                         && so.ClientCompanyID == uiSearchObj.ClientCompanyID
                         && sod.IsDeleted == false && sod.ProductID == uiSearchObj.ProductID
                         && sod.ProductSpecificationID == uiSearchObj.ProductSpecificationID
                         && sod.CreatedOn >= uiSearchObj.BeginDate
                         && sod.CreatedOn < uiSearchObj.EndDate
                         select sod);

            if (query.Count() > 0)
                return query.Sum(x => x.OutQty);

            return null;
        }
    }
}
