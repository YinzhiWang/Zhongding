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
    public class ClientTaskRefundAppRepository : BaseRepository<ClientTaskRefundApplication>, IClientTaskRefundAppRepository
    {
        public IList<UIClientTaskRefundApp> GetUIList(UISearchClientTaskRefundApp uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIClientTaskRefundApp> uiEntities = new List<UIClientTaskRefundApp>();

            int total = 0;

            IQueryable<ClientTaskRefundApplication> query = null;

            var whereFuncs = new List<Expression<Func<ClientTaskRefundApplication, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID.Equals(uiSearchObj.CompanyID));

                if (uiSearchObj.ClientUserID > 0)
                    whereFuncs.Add(x => x.ClientUserID.Equals(uiSearchObj.ClientUserID));

                if (uiSearchObj.ClientCompanyID > 0)
                    whereFuncs.Add(x => x.ClientCompanyID.Equals(uiSearchObj.ClientCompanyID));

                if (uiSearchObj.ProductID > 0)
                    whereFuncs.Add(x => x.ProductID.Equals(uiSearchObj.ProductID));

                if (uiSearchObj.WorkflowStatusID > 0)
                    whereFuncs.Add(x => x.WorkflowStatusID == uiSearchObj.WorkflowStatusID);

                if (uiSearchObj.IncludeWorkflowStatusIDs != null
                    && uiSearchObj.IncludeWorkflowStatusIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeWorkflowStatusIDs.Contains(x.WorkflowStatusID));

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.RefundDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddMonths(1);
                    whereFuncs.Add(x => x.RefundDate < uiSearchObj.EndDate);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join c in DB.Company on q.CompanyID equals c.ID
                              join cu in DB.ClientUser on q.ClientUserID equals cu.ID
                              join cc in DB.ClientCompany on q.ClientCompanyID equals cc.ID
                              join p in DB.Product on q.ProductID equals p.ID
                              join ps in DB.ProductSpecification on q.ProductSpecificationID equals ps.ID
                              join ws in DB.WorkflowStatus on q.WorkflowStatusID equals ws.ID
                              join cbu in DB.Users on q.CreatedBy equals cbu.UserID into tempCBU
                              from tcbu in tempCBU.DefaultIfEmpty()
                              select new UIClientTaskRefundApp
                              {
                                  ID = q.ID,
                                  CompanyName = c.CompanyName,
                                  ClientUserName = cu.ClientName,
                                  ClientCompanyName = cc.Name,
                                  ProductName = p.ProductName,
                                  Specification = ps.Specification,
                                  RefundAmount = q.RefundAmount ?? 0M,
                                  RefundDate = q.RefundDate,
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
