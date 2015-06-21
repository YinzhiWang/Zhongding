using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Common.Extension;

namespace ZhongDing.Business.Repositories
{
    public class SalarySettleRepository : BaseRepository<SalarySettle>, ISalarySettleRepository
    {
        public IList<Domain.UIObjects.UISalarySettle> GetUIList(Domain.UISearchObjects.UISearchSalarySettle uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UISalarySettle> uiEntitys = new List<UISalarySettle>();
            int total = 0;

            IQueryable<SalarySettle> query = null;

            List<Expression<Func<SalarySettle, bool>>> whereFuncs = new List<Expression<Func<SalarySettle, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                {
                    whereFuncs.Add(x => x.ID == uiSearchObj.ID);
                }
                if (uiSearchObj.WorkflowStatusID > 0)
                {
                    whereFuncs.Add(x => x.WorkflowStatusID == uiSearchObj.WorkflowStatusID);
                }
                if (uiSearchObj.DepartmentID.BiggerThanZero())
                {
                    whereFuncs.Add(x => x.DepartmentID == uiSearchObj.DepartmentID);
                }
                if (uiSearchObj.BeginDate.HasValue)
                {
                    whereFuncs.Add(x => x.SettleDate >= uiSearchObj.BeginDate);
                }
                if (uiSearchObj.EndDate.HasValue)
                {
                    whereFuncs.Add(x => x.SettleDate <= uiSearchObj.EndDate);
                }
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                var queryResult = from q in query
                                  join user in DB.Users on q.CreatedBy equals user.UserID
                                  join workflowStatus in DB.WorkflowStatus on q.WorkflowStatusID equals workflowStatus.ID
                                  join department in DB.Department on q.DepartmentID equals department.ID
                                  select new UISalarySettle()
                                  {
                                      ID = q.ID,

                                      WorkflowStatus = workflowStatus.StatusName,
                                      WorkflowStatusID = q.WorkflowStatusID,
                                      CreatedByUserID = q.CreatedBy.Value,
                                      CreatedByUserName = user.FullName,
                                      DepartmentName = department.DepartmentName,
                                      SettleDate = q.SettleDate


                                  };
                total = queryResult.Count();
                uiEntitys = queryResult.ToList();
            }

            totalRecords = total;
            return uiEntitys;
        }


        public bool HasSalarySettle(int departmentID, DateTime settleDate)
        {
            settleDate = new DateTime(settleDate.Year, settleDate.Month, 1);
            int count = GetList(x => x.SettleDate == settleDate && x.DepartmentID == departmentID).Count();
            return count > 0;
        }
    }
}
