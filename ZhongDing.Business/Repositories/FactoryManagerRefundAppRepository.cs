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
    public class FactoryManagerRefundAppRepository : BaseRepository<FactoryManagerRefundApplication>, IFactoryManagerRefundAppRepository
    {
        public IList<UIFactoryManagerRefundApp> GetUIList(UISearchFactoryManagerRefundApp uiSearchObj)
        {
            IList<UIFactoryManagerRefundApp> uiEntities = new List<UIFactoryManagerRefundApp>();

            IQueryable<FactoryManagerRefundApplication> query = null;

            var whereFuncs = new List<Expression<Func<FactoryManagerRefundApplication, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.ClientUserID > 0)
                    whereFuncs.Add(x => x.ClientUserID == uiSearchObj.ClientUserID);

                if (uiSearchObj.ProductID > 0)
                    whereFuncs.Add(x => x.ProductID == uiSearchObj.ProductID);

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

            query = GetList(whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC);

            if (query != null)
            {
                uiEntities = (from q in query
                              join c in DB.Company on q.CompanyID equals c.ID
                              join cu in DB.ClientUser on q.ClientUserID equals cu.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join ws in DB.WorkflowStatus on q.WorkflowStatusID equals ws.ID
                              join cbu in DB.Users on q.CreatedBy equals cbu.UserID into tempCBU
                              from tcbu in tempCBU.DefaultIfEmpty()
                              select new UIFactoryManagerRefundApp()
                              {
                                  ID = q.ID,
                                  CompanyName = c.CompanyName,
                                  ClientUserName = cu.ClientName,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  BeginDate = q.BeginDate,
                                  EndDate = q.EndDate,
                                  RefundAmount = q.RefundAmount,
                                  WorkflowStatusID = q.WorkflowStatusID,
                                  WorkflowStatus = ws.StatusName,
                                  CreatedByUserID = q.CreatedBy,
                                  CreatedBy = tcbu == null ? string.Empty : tcbu.FullName,
                                  CreatedOn = q.CreatedOn
                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIFactoryManagerRefundApp> GetUIList(UISearchFactoryManagerRefundApp uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIFactoryManagerRefundApp> uiEntities = new List<UIFactoryManagerRefundApp>();

            int total = 0;

            IQueryable<FactoryManagerRefundApplication> query = null;

            var whereFuncs = new List<Expression<Func<FactoryManagerRefundApplication, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.ClientUserID > 0)
                    whereFuncs.Add(x => x.ClientUserID == uiSearchObj.ClientUserID);

                if (uiSearchObj.ProductID > 0)
                    whereFuncs.Add(x => x.ProductID == uiSearchObj.ProductID);

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
                              join c in DB.Company on q.CompanyID equals c.ID
                              join cu in DB.ClientUser on q.ClientUserID equals cu.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join ws in DB.WorkflowStatus on q.WorkflowStatusID equals ws.ID
                              join cbu in DB.Users on q.CreatedBy equals cbu.UserID into tempCBU
                              from tcbu in tempCBU.DefaultIfEmpty()
                              select new UIFactoryManagerRefundApp()
                              {
                                  ID = q.ID,
                                  CompanyName = c.CompanyName,
                                  ClientUserName = cu.ClientName,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  BeginDate = q.BeginDate,
                                  EndDate = q.EndDate,
                                  RefundAmount = q.RefundAmount,
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

    }
}
