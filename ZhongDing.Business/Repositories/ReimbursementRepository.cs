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
using ZhongDing.Common.Extension;

namespace ZhongDing.Business.Repositories
{
    public class ReimbursementRepository : BaseRepository<Reimbursement>, IReimbursementRepository
    {
        public IList<Domain.UIObjects.UIReimbursement> GetUIList(Domain.UISearchObjects.UISearchReimbursement uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            List<UIReimbursement> uiEntitys = new List<UIReimbursement>();
            int total = 0;

            List<Expression<Func<Reimbursement, bool>>> whereFuncs = new List<Expression<Func<Reimbursement, bool>>>();

            var query = from q in DB.Reimbursement
                        join department in DB.Department on q.DepartmentID equals department.ID
                        join apply in DB.Users on q.CreatedBy equals apply.UserID
                        join ws in DB.WorkflowStatus on q.WorkflowStatusID equals ws.ID
                        where q.IsDeleted == false
                        select new UIReimbursement()
                        {
                            ID = q.ID,
                            WorkflowStatusID = q.WorkflowStatusID,
                            WorkflowStatus = ws.StatusName,
                            CreatedBy = apply.FullName,
                            DepartmentName = department.DepartmentName,
                            ApplyDate = q.ApplyDate,
                            DepartmentID = q.DepartmentID
                        };


            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                {
                    query = query.Where(x => x.ID == uiSearchObj.ID);
                    whereFuncs.Add(x => x.ID == uiSearchObj.ID);
                }
                if (uiSearchObj.DepartmentID.BiggerThanZero())
                {
                    query = query.Where(x => x.DepartmentID == uiSearchObj.DepartmentID);
                }
                if (uiSearchObj.BeginDate.HasValue)
                {
                    query = query.Where(x => x.ApplyDate >= uiSearchObj.BeginDate);
                }
                if (uiSearchObj.EndDate.HasValue)
                {
                    query = query.Where(x => x.ApplyDate <= uiSearchObj.EndDate);
                }
                if (uiSearchObj.EndDate.HasValue)
                {
                    query = query.Where(x => x.ApplyDate <= uiSearchObj.EndDate);
                }
                if (uiSearchObj.IncludeWorkflowStatusIDs != null)
                {
                    query = query.Where(x => uiSearchObj.IncludeWorkflowStatusIDs.Contains(x.WorkflowStatusID));
                }
                if (uiSearchObj.WorkflowStatusID > 0)
                {
                    query = query.Where(x => uiSearchObj.WorkflowStatusID==x.WorkflowStatusID);
                }
                if (uiSearchObj.ApplyUser.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.CreatedBy.Contains(uiSearchObj.ApplyUser));
                }
            }
            total = query.Count();



            totalRecords = total;
            uiEntitys = query.OrderByDescending(x => x.ApplyDate)
                  .Skip(pageSize * pageIndex).Take(pageSize).ToList();

            return uiEntitys;
        }
    }
}
