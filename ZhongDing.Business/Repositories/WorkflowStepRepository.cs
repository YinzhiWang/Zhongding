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
    public class WorkflowStepRepository : BaseRepository<WorkflowStep>, IWorkflowStepRepository
    {
        public IList<UIWorkflowStep> GetUIList(UISearchWorkflowStep uiSearchObj = null)
        {
            IList<UIWorkflowStep> uiEntities = new List<UIWorkflowStep>();

            IQueryable<WorkflowStep> query = null;

            List<Expression<Func<WorkflowStep, bool>>> whereFuncs = new List<Expression<Func<WorkflowStep, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.WorkfolwID > 0)
                    whereFuncs.Add(x => x.WorkflowID.Equals(uiSearchObj.WorkfolwID));

                if (uiSearchObj.UserID > 0)
                    whereFuncs.Add(x => x.WorkflowStepUser.Any(y => y.UserID.Equals(uiSearchObj.UserID)));
            }

            query = GetList(whereFuncs);

            if (query != null)
            {
                uiEntities = (from q in query
                              join wf in DB.Workflow on q.WorkflowID equals wf.ID
                              select new UIWorkflowStep()
                              {
                                  ID = q.ID,
                                  WorkfolwName = wf.WorkflowName,
                                  StepName = q.StepName,
                                  StepUserIDs = q.WorkflowStepUser.Where(x => x.IsDeleted == false).Select(x => x.UserID)
                              }).ToList();

                foreach (var entity in uiEntities)
                {
                    entity.StepUserNames = string.Join(", ", DB.WorkflowStepUser.Where(x => entity.StepUserIDs.Contains(x.ID)).Select(x => x.Users.FullName).ToList());
                }
            }

            return uiEntities;
        }

        public IList<UIWorkflowStep> GetUIList(UISearchWorkflowStep uiSearchObj, int pageIndex, int pageSize, out int totalRecords)
        {
            IList<UIWorkflowStep> uiEntities = new List<UIWorkflowStep>();

            int total = 0;

            IQueryable<WorkflowStep> query = null;

            List<Expression<Func<WorkflowStep, bool>>> whereFuncs = new List<Expression<Func<WorkflowStep, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.ID > 0)
                    whereFuncs.Add(x => x.ID.Equals(uiSearchObj.ID));

                if (uiSearchObj.WorkfolwID > 0)
                    whereFuncs.Add(x => x.WorkflowID.Equals(uiSearchObj.WorkfolwID));

                if (uiSearchObj.UserID > 0)
                    whereFuncs.Add(x => x.WorkflowStepUser.Any(y => y.UserID.Equals(uiSearchObj.UserID)));
            }

            query = GetList(pageIndex, pageSize, whereFuncs, out total);

            if (query != null)
            {
                uiEntities = (from q in query
                              join wf in DB.Workflow on q.WorkflowID equals wf.ID
                              select new UIWorkflowStep()
                              {
                                  ID = q.ID,
                                  WorkfolwName = wf.WorkflowName,
                                  StepName = q.StepName,
                                  StepUserIDs = q.WorkflowStepUser.Where(x => x.IsDeleted == false).Select(x => x.UserID)
                              }).ToList();

                foreach (var entity in uiEntities)
                {
                    entity.StepUserNames = string.Join(", ", DB.Users.Where(x => entity.StepUserIDs.Contains(x.UserID)).Select(x => x.FullName).ToList());
                }
            }

            totalRecords = total;

            return uiEntities;
        }

        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<WorkflowStep, bool>>> whereFuncs = new List<Expression<Func<WorkflowStep, bool>>>();

            if (uiSearchObj != null)
            {
                if (uiSearchObj.IncludeItemValues != null
                    && uiSearchObj.IncludeItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeItemValues.Contains(x.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => x.StepName.Contains(uiSearchObj.ItemText));

                if (uiSearchObj.Extension != null)
                {
                    if (uiSearchObj.Extension.WorkflowID > 0)
                        whereFuncs.Add(x => x.WorkflowID == uiSearchObj.Extension.WorkflowID);
                }
            }

            uiDropdownItems = GetList(whereFuncs).Select(x => new UIDropdownItem()
            {
                ItemValue = x.ID,
                ItemText = x.StepName
            }).ToList();

            return uiDropdownItems;
        }
    }
}
