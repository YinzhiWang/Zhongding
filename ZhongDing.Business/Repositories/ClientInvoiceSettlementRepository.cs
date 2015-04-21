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
    public class ClientInvoiceSettlementRepository : BaseRepository<ClientInvoiceSettlement>, IClientInvoiceSettlementRepository
    {
        public IList<UIClientInvoiceSettlement> GetUIList(UISearchClientInvoiceSettlement uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIClientInvoiceSettlement> uiEntities = new List<UIClientInvoiceSettlement>();

            int total = 0;

            IQueryable<ClientInvoiceSettlement> query = null;

            var whereFuncs = new List<Expression<Func<ClientInvoiceSettlement, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.ExcludeCanceled)
                    whereFuncs.Add(x => x.IsCanceled == false);

                if (uiSearchObj.CompanyID > 0)
                    whereFuncs.Add(x => x.CompanyID == uiSearchObj.CompanyID);

                if (uiSearchObj.ClientCompanyID > 0)
                    whereFuncs.Add(x => x.ClientCompanyID == uiSearchObj.ClientCompanyID);

                if (uiSearchObj.WorkflowStatusID > 0)
                    whereFuncs.Add(x => x.WorkflowStatusID == uiSearchObj.WorkflowStatusID);

                if (uiSearchObj.IncludeWorkflowStatusIDs != null
                    && uiSearchObj.IncludeWorkflowStatusIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeWorkflowStatusIDs.Contains(x.WorkflowStatusID));

                if (uiSearchObj.BeginDate.HasValue)
                    whereFuncs.Add(x => x.SettlementDate >= uiSearchObj.BeginDate);

                if (uiSearchObj.EndDate.HasValue)
                {
                    uiSearchObj.EndDate = uiSearchObj.EndDate.Value.AddDays(1);
                    whereFuncs.Add(x => x.SettlementDate < uiSearchObj.EndDate);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join cc in DB.ClientCompany on q.ClientCompanyID equals cc.ID
                              join ws in DB.WorkflowStatus on q.WorkflowStatusID equals ws.ID
                              join cbu in DB.Users on q.CreatedBy equals cbu.UserID into tempCBU
                              from tcbu in tempCBU.DefaultIfEmpty()
                              select new UIClientInvoiceSettlement()
                              {
                                  ID = q.ID,
                                  SettlementDate = q.SettlementDate,
                                  TotalInvoiceAmount = q.TotalInvoiceAmount,
                                  TotalPayAmount = q.TotalPayAmount,
                                  ClientCompanyName = cc.Name,
                                  InvoiceNumberArray = q.ClientInvoiceSettlementDetail
                                  .Where(x => x.IsDeleted == false).Select(x => x.InvoiceNumber),
                                  WorkflowStatusID = q.WorkflowStatusID,
                                  WorkflowStatus = ws.StatusName,
                                  CreatedByUserID = q.CreatedBy,
                                  CreatedBy = tcbu == null ? string.Empty : tcbu.FullName,
                                  PaidBy = q.PaidBy
                              }).ToList();

                foreach (var uiEntity in uiEntities)
                {
                    uiEntity.InvoiceNumbers = string.Join(", ", uiEntity.InvoiceNumberArray);
                }
            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
