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
    public class WorkflowRepository : BaseRepository<Workflow>, IWorkflowRepository
    {
        public IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null)
        {
            IList<UIDropdownItem> uiDropdownItems = new List<UIDropdownItem>();

            List<Expression<Func<Workflow, bool>>> whereFuncs = new List<Expression<Func<Workflow, bool>>>();

            whereFuncs.Add(x => x.IsActive == true);

            if (uiSearchObj != null)
            {
                if (uiSearchObj.IncludeItemValues != null
                    && uiSearchObj.IncludeItemValues.Count > 0)
                    whereFuncs.Add(x => uiSearchObj.IncludeItemValues.Contains(x.ID));

                if (!string.IsNullOrEmpty(uiSearchObj.ItemText))
                    whereFuncs.Add(x => x.WorkflowName.Contains(uiSearchObj.ItemText));
            }

            uiDropdownItems = GetList(whereFuncs).Select(x => new UIDropdownItem()
            {
                ItemValue = x.ID,
                ItemText = x.WorkflowName
            }).ToList();

            return uiDropdownItems;
        }

        public IList<int> GetCanAccessWorkflowsByUserID(int userID)
        {
            IList<int> workflowIDs = new List<int>();

            workflowIDs = (from wsu in DB.WorkflowStepUser
                           join ws in DB.WorkflowStep on wsu.WorkflowStepID equals ws.ID
                           where ws.IsDeleted == false
                           && wsu.IsDeleted == false
                           && wsu.UserID == userID
                           group ws by new { ws.WorkflowID } into g
                           select g.Key.WorkflowID).ToList();

            var workflowIDsFromUserGroup = (from wsu in DB.WorkflowStepUserGroup
                                            join ws in DB.WorkflowStep on wsu.WorkflowStepID equals ws.ID
                                            join userGroupUser in DB.UserGroupUser on wsu.UserGroupID equals userGroupUser.UserGroupID
                                            where ws.IsDeleted == false
                                            && wsu.IsDeleted == false
                                            && userGroupUser.IsDeleted == false
                                            && userGroupUser.UserID == userID
                                            group ws by new { ws.WorkflowID } into g
                                            select g.Key.WorkflowID).ToList();

            return workflowIDs.Union(workflowIDsFromUserGroup).ToList();
        }
    }
}
