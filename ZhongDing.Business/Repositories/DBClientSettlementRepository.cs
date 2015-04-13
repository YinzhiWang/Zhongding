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
    public class DBClientSettlementRepository : BaseRepository<DBClientSettlement>, IDBClientSettlementRepository
    {

        public IList<UIDBClientSettlement> GetUIList(UISearchDBClientSettlement uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIDBClientSettlement> uiEntities = new List<UIDBClientSettlement>();

            int total = 0;

            IQueryable<DBClientSettlement> query = null;

            List<Expression<Func<DBClientSettlement, bool>>> whereFuncs = new List<Expression<Func<DBClientSettlement, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.HospitalTypeID > 0)
                    whereFuncs.Add(x => x.HospitalTypeID == uiSearchObj.HospitalTypeID);

                if (uiSearchObj.SettlementDate.HasValue)
                    whereFuncs.Add(x => x.SettlementDate == uiSearchObj.SettlementDate);

                if (uiSearchObj.WorkflowStatusID > 0)
                    whereFuncs.Add(x => x.WorkflowStatusID == uiSearchObj.WorkflowStatusID);

                if (uiSearchObj.IncludeWorkflowStatusIDs != null
                    && uiSearchObj.IncludeWorkflowStatusIDs.Count() > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeWorkflowStatusIDs.Contains(x.WorkflowStatusID));

            }

            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.SETTLEMENTDATE_DESC, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join ht in DB.HospitalType on q.HospitalTypeID equals ht.ID
                              join ws in DB.WorkflowStatus on q.WorkflowStatusID equals ws.ID
                              join cbu in DB.Users on q.CreatedBy equals cbu.UserID into tempCBU
                              from tcbu in tempCBU.DefaultIfEmpty()
                              select new UIDBClientSettlement()
                              {
                                  ID = q.ID,
                                  SettlementDate = q.SettlementDate,
                                  HospitalType = ht.TypeName,
                                  WorkflowStatusID = q.WorkflowStatusID,
                                  WorkflowStatus = ws.StatusName,
                                  CreatedByUserID = q.CreatedBy,
                                  CreatedBy = tcbu == null ? string.Empty : tcbu.FullName
                              }).ToList();

            }

            totalRecords = total;

            return uiEntities;
        }
    }
}
