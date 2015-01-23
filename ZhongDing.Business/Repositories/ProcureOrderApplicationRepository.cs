using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Common;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class ProcureOrderApplicationRepository : BaseRepository<ProcureOrderApplication>, IProcureOrderApplicationRepository
    {

        public IList<UIProcureOrderApplication> GetUIList(UISearchProcureOrderApplication uiSearchObj = null)
        {
            IList<UIProcureOrderApplication> uiEntities = new List<UIProcureOrderApplication>();

            IQueryable<ProcureOrderApplication> query = null;

            List<Expression<Func<ProcureOrderApplication, bool>>> whereFuncs = new List<Expression<Func<ProcureOrderApplication, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.Supplier.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.IncludeWorkflowStatusIDs != null
                    && uiSearchObj.IncludeWorkflowStatusIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeWorkflowStatusIDs.Contains(x.WorkflowStatusID));

                if (!string.IsNullOrEmpty(uiSearchObj.OrderCode))
                    whereFuncs.Add(x => x.OrderCode.Contains(uiSearchObj.OrderCode));

                if (uiSearchObj.SupplierID > 0)
                    whereFuncs.Add(x => x.SupplierID.Equals(uiSearchObj.SupplierID));

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.OrderDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.OrderDate < uiSearchObj.EndDate);
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
                              select new UIProcureOrderApplication()
                              {
                                  ID = q.ID,
                                  OrderCode = q.OrderCode,
                                  OrderDate = q.OrderDate,
                                  SupplierName = s.SupplierName,
                                  IsStop = q.IsStop,
                                  EstDeliveryDate = q.EstDeliveryDate,
                                  WorkflowStatusID = q.WorkflowStatusID,
                                  WorkflowStatus = ws.StatusName,
                                  CreatedByUserID = q.CreatedBy,
                                  CreatedBy = tcu == null ? string.Empty : tcu.FullName
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIProcureOrderApplication> GetUIList(UISearchProcureOrderApplication uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIProcureOrderApplication> uiEntities = new List<UIProcureOrderApplication>();

            int total = 0;

            IQueryable<ProcureOrderApplication> query = null;

            List<Expression<Func<ProcureOrderApplication, bool>>> whereFuncs = new List<Expression<Func<ProcureOrderApplication, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.Supplier.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.IncludeWorkflowStatusIDs != null
                    && uiSearchObj.IncludeWorkflowStatusIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeWorkflowStatusIDs.Contains(x.WorkflowStatusID));

                if (!string.IsNullOrEmpty(uiSearchObj.OrderCode))
                    whereFuncs.Add(x => x.OrderCode.Contains(uiSearchObj.OrderCode));

                if (uiSearchObj.SupplierID > 0)
                    whereFuncs.Add(x => x.SupplierID.Equals(uiSearchObj.SupplierID));

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.OrderDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.OrderDate < uiSearchObj.EndDate);
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
                              select new UIProcureOrderApplication()
                              {
                                  ID = q.ID,
                                  OrderCode = q.OrderCode,
                                  OrderDate = q.OrderDate,
                                  SupplierName = s.SupplierName,
                                  IsStop = q.IsStop,
                                  EstDeliveryDate = q.EstDeliveryDate,
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
            if (this.DB.ProcureOrderApplication.Count() > 0)
                return this.DB.ProcureOrderApplication.Max(x => x.ID);
            else return null;
        }
    }
}
