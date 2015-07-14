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

namespace ZhongDing.Business.Repositories
{
    public class ReimbursementRepository : BaseRepository<Reimbursement>, IReimbursementRepository
    {
        public IList<Domain.UIObjects.UIReimbursement> GetUIList(Domain.UISearchObjects.UISearchReimbursement uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIReimbursement> uiEntitys = new List<UIReimbursement>();
            int total = 0;

            List<Expression<Func<Reimbursement, bool>>> whereFuncs = new List<Expression<Func<Reimbursement, bool>>>();
            IQueryable<Reimbursement> query = null;
            query = GetList(pageIndex, pageSize, whereFuncs, GlobalConst.OrderByExpression.CREATEDON_DESC, out total);

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                {
                    whereFuncs.Add(x => x.ID == uiSearchObj.ID);
                }
            }

            var queryResult = from q in query
                              join department in DB.Department on q.DepartmentID equals department.ID
                              join apply in DB.Users on q.CreatedBy equals apply.UserID
                              join ws in DB.WorkflowStatus on q.WorkflowStatusID equals ws.ID
                              select new UIReimbursement()
                              {
                                  ID = q.ID,
                                  WorkflowStatusID = q.WorkflowStatusID,
                                  WorkflowStatus = ws.StatusName,
                                  CreatedBy = apply.FullName,
                                  DepartmentName = department.DepartmentName,
                                  ApplyDate = q.ApplyDate,
                              };

            total = query.Count();
            totalRecords = total;
            uiEntitys = queryResult.ToList();
            return uiEntitys;
        }
    }
}
