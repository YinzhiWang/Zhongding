using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.Repositories
{
    public class ClientSaleApplicationRepository : BaseRepository<ClientSaleApplication>, IClientSaleApplicationRepository
    {
        public IList<UIClientSaleApplication> GetUIList(UISearchClientSaleApplication uiSearchObj = null)
        {
            throw new NotImplementedException();
        }

        public IList<UIClientSaleApplication> GetUIList(UISearchClientSaleApplication uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIClientSaleApplication> uiEntities = new List<UIClientSaleApplication>();

            int total = 0;

            IQueryable<ClientSaleApplication> query = null;

            List<Expression<Func<ClientSaleApplication, bool>>> whereFuncs = new List<Expression<Func<ClientSaleApplication, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

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

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join soa in DB.SalesOrderApplication on q.SalesOrderApplicationID equals soa.ID
                              join sm in DB.SalesModel on q.SalesModelID equals sm.ID
                              join cu in DB.ClientUser on q.ClientUserID equals cu.ID
                              join cc in DB.ClientCompany on q.ClientCompanyID equals cc.ID
                              select new UIClientSaleApplication()
                              {
                                  ID = q.ID,
                                  OrderCode = soa.OrderCode,
                                  OrderDate = soa.OrderDate,
                                  SalesModel = sm.SalesModelName,
                                  ClientUserName = cu.ClientName,
                                  ClientCompanyName = cc.Name,
                                  IsGuaranteeTransaction = q.IsGuaranteeTransaction,
                                  IsReturnedGuaranteeAmount = q.IsReturnedGuaranteeAmount,
                                  IsStop = soa.IsStop
                              }).ToList();

                foreach (var uiEntity in uiEntities.Where(x => x.IsGuaranteeTransaction == true))
                {
                    if (uiEntity.IsReturnedGuaranteeAmount)
                        uiEntity.IconUrlOfGuarantee = "";
                    else
                        uiEntity.IconUrlOfGuarantee = "";
                }
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
