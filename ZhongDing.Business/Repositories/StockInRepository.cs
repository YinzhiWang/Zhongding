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
    public class StockInRepository : BaseRepository<StockIn>, IStockInRepository
    {
        public IList<UIStockIn> GetUIList(UISearchStockIn uiSearchObj = null)
        {
            IList<UIStockIn> uiEntities = new List<UIStockIn>();

            IQueryable<StockIn> query = null;

            List<Expression<Func<StockIn, bool>>> whereFuncs = new List<Expression<Func<StockIn, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.Supplier.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.IncludeWorkflowStatusIDs != null
                    && uiSearchObj.IncludeWorkflowStatusIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeWorkflowStatusIDs.Contains(x.WorkflowStatusID));

                if (!string.IsNullOrEmpty(uiSearchObj.Code))
                    whereFuncs.Add(x => x.Code.Contains(uiSearchObj.Code));

                if (uiSearchObj.SupplierID > 0)
                    whereFuncs.Add(x => x.SupplierID.Equals(uiSearchObj.SupplierID));

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.EntryDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.EntryDate < uiSearchObj.EndDate);
                }

                if (uiSearchObj.WorkflowStatusID > 0)
                    whereFuncs.Add(x => x.WorkflowStatusID.Equals(uiSearchObj.WorkflowStatusID));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join s in DB.Supplier on q.SupplierID equals s.ID
                              join ws in DB.WorkflowStatus on q.WorkflowStatusID equals ws.ID
                              join cu in DB.Users on q.CreatedBy equals cu.UserID into tempCU
                              from tcu in tempCU.DefaultIfEmpty()
                              orderby q.CreatedOn descending
                              select new UIStockIn()
                              {
                                  ID = q.ID,
                                  Code = q.Code,
                                  EntryDate = q.EntryDate,
                                  SupplierName = s.SupplierName,
                                  WorkflowStatusID = q.WorkflowStatusID,
                                  WorkflowStatus = ws.StatusName,
                                  CreatedByUserID = q.CreatedBy,
                                  CreatedBy = tcu == null ? string.Empty : tcu.FullName
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIStockIn> GetUIList(UISearchStockIn uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIStockIn> uiEntities = new List<UIStockIn>();

            int total = 0;

            IQueryable<StockIn> query = null;

            List<Expression<Func<StockIn, bool>>> whereFuncs = new List<Expression<Func<StockIn, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.Supplier.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.IncludeWorkflowStatusIDs != null
                    && uiSearchObj.IncludeWorkflowStatusIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeWorkflowStatusIDs.Contains(x.WorkflowStatusID));

                if (!string.IsNullOrEmpty(uiSearchObj.Code))
                    whereFuncs.Add(x => x.Code.Contains(uiSearchObj.Code));

                if (uiSearchObj.SupplierID > 0)
                    whereFuncs.Add(x => x.SupplierID.Equals(uiSearchObj.SupplierID));

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.EntryDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.EntryDate < uiSearchObj.EndDate);
                }

                if (uiSearchObj.WorkflowStatusID > 0)
                    whereFuncs.Add(x => x.WorkflowStatusID.Equals(uiSearchObj.WorkflowStatusID));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join s in DB.Supplier on q.SupplierID equals s.ID
                              join ws in DB.WorkflowStatus on q.WorkflowStatusID equals ws.ID
                              join cu in DB.Users on q.CreatedBy equals cu.UserID into tempCU
                              from tcu in tempCU.DefaultIfEmpty()
                              orderby q.CreatedOn descending
                              select new UIStockIn()
                              {
                                  ID = q.ID,
                                  Code = q.Code,
                                  EntryDate = q.EntryDate,
                                  SupplierName = s.SupplierName,
                                  WorkflowStatusID = q.WorkflowStatusID,
                                  WorkflowStatus = ws.StatusName,
                                  CreatedByUserID = q.CreatedBy,
                                  CreatedBy = tcu == null ? string.Empty : tcu.FullName
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
        public IList<UIStockIn> GetUIListForTransportFee(UISearchStockIn uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIStockIn> uiEntities = new List<UIStockIn>();

            int total = 0;

            IQueryable<StockIn> query = null;

            List<Expression<Func<StockIn, bool>>> whereFuncs = new List<Expression<Func<StockIn, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.Supplier.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.IncludeWorkflowStatusIDs != null
                    && uiSearchObj.IncludeWorkflowStatusIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeWorkflowStatusIDs.Contains(x.WorkflowStatusID));

                if (!string.IsNullOrEmpty(uiSearchObj.Code))
                    whereFuncs.Add(x => x.Code.Contains(uiSearchObj.Code));

                if (uiSearchObj.SupplierID > 0)
                    whereFuncs.Add(x => x.SupplierID.Equals(uiSearchObj.SupplierID));

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.EntryDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.EntryDate < uiSearchObj.EndDate);
                }

                if (uiSearchObj.WorkflowStatusID > 0)
                    whereFuncs.Add(x => x.WorkflowStatusID.Equals(uiSearchObj.WorkflowStatusID));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join s in DB.Supplier on q.SupplierID equals s.ID
                              join ws in DB.WorkflowStatus on q.WorkflowStatusID equals ws.ID
                              join cu in DB.Users on q.CreatedBy equals cu.UserID into tempCU
                              from tcu in tempCU.DefaultIfEmpty()
                              join transportFeeStockIn in DB.TransportFeeStockIn.Where(x=>x.IsDeleted==false) on q.ID equals transportFeeStockIn.StockInID into tempTransportFeeStockIns
                              from tempTransportFeeStockIn in tempTransportFeeStockIns.DefaultIfEmpty()
                              where tempTransportFeeStockIn == null && q.WorkflowStatusID == (int)EWorkflowStatus.InWarehouse
                              orderby q.CreatedOn descending
                              select new UIStockIn()
                              {
                                  ID = q.ID,
                                  Code = q.Code,
                                  EntryDate = q.EntryDate,
                                  SupplierName = s.SupplierName,
                                  WorkflowStatusID = q.WorkflowStatusID,
                                  WorkflowStatus = ws.StatusName,
                                  CreatedByUserID = q.CreatedBy,
                                  CreatedBy = tcu == null ? string.Empty : tcu.FullName
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }

        public int? GetMaxEntityID()
        {
            if (this.DB.StockIn.Count() > 0)
                return this.DB.StockIn.Max(x => x.ID);
            else return null;
        }
    }
}
