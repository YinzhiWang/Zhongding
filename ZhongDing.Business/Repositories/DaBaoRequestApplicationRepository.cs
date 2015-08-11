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
    public class DaBaoRequestApplicationRepository : BaseRepository<DaBaoRequestApplication>, IDaBaoRequestApplicationRepository
    {
        public IList<UIDaBaoApplication> GetUIList(UISearchDaBaoApplication uiSearchObj = null)
        {
            IList<UIDaBaoApplication> uiEntities = new List<UIDaBaoApplication>();

            IQueryable<DaBaoRequestApplication> query = null;

            List<Expression<Func<DaBaoRequestApplication, bool>>> whereFuncs = new List<Expression<Func<DaBaoRequestApplication, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.IncludeWorkflowStatusIDs != null
                    && uiSearchObj.IncludeWorkflowStatusIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeWorkflowStatusIDs.Contains(x.WorkflowStatusID));

                if (uiSearchObj.DepartmentID > 0)
                    whereFuncs.Add(x => x.DepartmentID == uiSearchObj.DepartmentID);

                if (uiSearchObj.DistributionCompanyID > 0)
                    whereFuncs.Add(x => x.DistributionCompanyID == uiSearchObj.DistributionCompanyID);

                if (uiSearchObj.WorkflowStatusID > 0)
                    whereFuncs.Add(x => x.WorkflowStatusID == uiSearchObj.WorkflowStatusID);

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.CreatedOn >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.CreatedOn < uiSearchObj.EndDate);
                }
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join d in DB.Department on q.DepartmentID equals d.ID
                              join dc in DB.DistributionCompany on q.DistributionCompanyID equals dc.ID
                              join ws in DB.WorkflowStatus on q.WorkflowStatusID equals ws.ID
                              join cu in DB.Users on q.CreatedBy equals cu.UserID into tempCU
                              from tcu in tempCU.DefaultIfEmpty()
                              orderby q.CreatedOn descending
                              select new UIDaBaoApplication()
                              {
                                  ID = q.ID,
                                  DepartmentName = d.DepartmentName,
                                  DistributionCompany = dc.Name,
                                  WorkflowStatusID = q.WorkflowStatusID,
                                  WorkflowStatus = ws.StatusName,
                                  CreatedOn = q.CreatedOn,
                                  CreatedBy = tcu == null ? string.Empty : tcu.FullName,
                                  CreatedByUserID = q.CreatedBy

                              }).ToList();
            }

            return uiEntities;
        }

        public IList<UIDaBaoApplication> GetUIList(UISearchDaBaoApplication uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDaBaoApplication> uiEntities = new List<UIDaBaoApplication>();

            int total = 0;

            IQueryable<DaBaoRequestApplication> query = null;

            List<Expression<Func<DaBaoRequestApplication, bool>>> whereFuncs = new List<Expression<Func<DaBaoRequestApplication, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.IncludeWorkflowStatusIDs != null
                    && uiSearchObj.IncludeWorkflowStatusIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeWorkflowStatusIDs.Contains(x.WorkflowStatusID));

                if (uiSearchObj.DepartmentID > 0)
                    whereFuncs.Add(x => x.DepartmentID == uiSearchObj.DepartmentID);

                if (uiSearchObj.DistributionCompanyID > 0)
                    whereFuncs.Add(x => x.DistributionCompanyID == uiSearchObj.DistributionCompanyID);

                if (uiSearchObj.WorkflowStatusID > 0)
                    whereFuncs.Add(x => x.WorkflowStatusID == uiSearchObj.WorkflowStatusID);

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
                              join d in DB.Department on q.DepartmentID equals d.ID
                              join dc in DB.DistributionCompany on q.DistributionCompanyID equals dc.ID
                              join ws in DB.WorkflowStatus on q.WorkflowStatusID equals ws.ID
                              join cu in DB.Users on q.CreatedBy equals cu.UserID into tempCU
                              from tcu in tempCU.DefaultIfEmpty()
                              select new UIDaBaoApplication()
                              {
                                  ID = q.ID,
                                  DepartmentName = d.DepartmentName,
                                  DistributionCompany = dc.Name,
                                  WorkflowStatusID = q.WorkflowStatusID,
                                  WorkflowStatus = ws.StatusName,
                                  CreatedOn = q.CreatedOn,
                                  CreatedBy = tcu == null ? string.Empty : tcu.FullName,
                                  CreatedByUserID = q.CreatedBy,
                                  OrderCode = q.DaBaoApplication != null ? q.DaBaoApplication.SalesOrderApplication.OrderCode : "",
                                  DaBaoApplicationID = q.DaBaoApplicationID
                              }).ToList();
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
